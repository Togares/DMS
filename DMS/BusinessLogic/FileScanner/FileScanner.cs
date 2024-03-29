﻿using CommonTypes;
using System.Collections.Generic;
using BusinessLogic.FileScanner.Events;
using System.IO;
using Visualis.Extractor;
using System.Text;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace BusinessLogic.FileScanner
{
    public class FileScanner : IFileScanner
    {
        public event DriveScanFinishedEventHandler DriveScanFinished;
        public event FileScanFinishedEventHandler FileScanFinished;

        private TextExtractorD _TextExtractor;

        public FileScanner()
        {
            _TextExtractor = new TextExtractorD();
        }

        /// <summary>
        /// Scant alle Laufwerke des Rechners und Baut CommonTypes.Drive Objekte entsprechend auf.
        /// Nachdem der Vorgang beendet ist, wird DriveScanFinished ausgelöst.
        /// </summary>
        public async void ScanDrives()
        {
            await Task.Run(() =>
            {
                List<Drive> drives = new List<Drive>();

                foreach (DriveInfo driveInfo in DriveInfo.GetDrives())
                {
                    Drive drive = new Drive();
                    drive.Name = driveInfo.Name;
                    drive.Type = driveInfo.DriveType.ToString();
                    try
                    { // nicht benannte oder nicht bereite laufwerke lösen eine exception aus
                        drive.Label = driveInfo.VolumeLabel;
                    }
                    catch (UnauthorizedAccessException)
                    {
                        continue; // Laufwerke auf die nicht Zugegriffen werden kann, werden nicht angezeigt
                    }
                    catch (IOException)
                    {
                        drive.Label = string.Empty;
                    }
                    drives.Add(drive);
                }

                DriveScanFinished?.Invoke(drives);
            });
        }

        /// <summary>
        /// Fügt der Liste von Directory des <paramref name="drive"/> eine Auflistung 
        /// von unter ihm liegenden Verzeichnissen hinzu
        /// Zudem werden alle Dateien auf der Ebende des Laufwerks eingescannt
        /// </summary>
        /// <param name="drive">Das Laufwerk, auf dem nach Unterverzeichnissen gesucht wird</param>
        public void GetDirectories(Drive drive)
        {
            string[] directories;
            try
            {
                directories = System.IO.Directory.GetDirectories(drive.Name);
            }
            catch (IOException)
            {
                return; // wenn das Laufwerk nicht bereit ist
            }
            foreach (string dir in directories)
            {
                CommonTypes.Directory directory = new CommonTypes.Directory();
                directory.Name = dir.Substring(dir.LastIndexOf("\\") + 1);
                directory.Path = dir.Substring(0, dir.Length - directory.Name.Length);
                if (!drive.Directories.Contains(directory))
                {
                    drive.Directories.Add(directory);
                    foreach (CommonTypes.File file in GetFiles(drive.Name, false))
                    {
                        if (!drive.Files.Contains(file))
                        {
                            drive.Files.Add(file);
                        }
                    }
                }
                GetDirectories(directory);
            }
        }

        /// <summary>
        /// Fügt der Liste von Directory des <paramref name="hierachical"/> eine Auflistung
        /// von unter ihm liegende Verzeichnissen hinzu
        /// </summary>
        /// <param name="directory">Das Verzeichnis, in dem nach Unterverzeichnissen gesucht wird</param>
        public void GetDirectories(Hierarchical hierachical)
        {
            InternalGetDirectories(hierachical);
            foreach (Hierarchical hierarchical in hierachical.Directories)
            {
                InternalGetDirectories(hierarchical);
            }
        }

        private void InternalGetDirectories(Hierarchical hierarchical)
        {
            string fullPath = hierarchical.Qualifier;
            if (!fullPath.EndsWith("\\"))
            {
                fullPath += "\\";
            }
            string[] directories;
            try
            {
                directories = System.IO.Directory.GetDirectories(fullPath);
            }
            catch (IOException)
            {
                return;
            }
            catch (UnauthorizedAccessException)
            {
                return;
            }
            foreach (string dir in directories)
            {
                DirectoryInfo info = new DirectoryInfo(dir);

                // Versteckte Verzeichnisse werden nicht angezeigt
                if (info.Attributes.HasFlag(FileAttributes.Hidden))
                {
                    continue;
                }

                CommonTypes.Directory d = new CommonTypes.Directory();

                d.Name = info.Name;//dir.Substring(dir.LastIndexOf("\\") + 1);
                d.Path = dir.Substring(0, dir.Length - d.Name.Length);
                if (!hierarchical.Directories.Contains(d))
                {
                    hierarchical.Directories.Add(d);
                }
            }
            foreach (CommonTypes.File file in GetFiles(fullPath, false))
            {
                if (!hierarchical.Files.Contains(file))
                {
                    hierarchical.Files.Add(file);
                }
            }

        }

        /// <summary>
        /// Scant in einem Verzeichnis nach Dateien.
        /// Insofern rekursiv gescant wird, werden auch alle Unterverzeichnisse nach Dateien durchsucht.
        /// Nachdem der Vorgang beendet wurde, wird FileScanFinished ausgelöst.
        /// </summary>
        /// <param name="path">Das Verzeichnis, Vollqualifiziert</param>
        /// <param name="recursive">Angabe, ob rekursiv gescannt wird</param>
        public async void ScanDirectory(string path, bool recursive)
        {
            await Task.Run(() =>
            {
                List<CommonTypes.File> files = new List<CommonTypes.File>();

                files.AddRange(GetFiles(path, true));

                if (recursive)
                {
                    foreach (string directory in System.IO.Directory.GetDirectories(path))
                    {
                        ScanDirectory(directory, true);
                    }
                }

                FileScanFinished?.Invoke(files);
            });
        }

        /// <summary>
        /// Baut CommonTypes.File Objekte aus alles Dateien, die unter path gefunden werden
        /// </summary>
        /// <param name="path">Der Pfad unter dem nach Dateien gesucht wird</param>
        /// <param name="withContent">Angabe, ob die Datei direkt ausgelesen werden soll</param>
        /// <returns>Eine Liste mit CommonTypes.File Objekten, die die Dateien des Dateisystems repräsentieren</returns>
        private List<CommonTypes.File> GetFiles(string path, bool withContent)
        {
            List<CommonTypes.File> files = new List<CommonTypes.File>();
            string[] paths;
            try
            {
                paths = System.IO.Directory.GetFiles(path);
            }
            catch (IOException e)
            {
                return files;
            }

            // nicht in der foreach, da der Iterationsvariable nichts zugewiesen werden kann
            for (int i = 0; i < paths.Length; ++i)
            {
                paths[i] = paths[i].Replace("\\", "/");
            }

            foreach (string filePath in paths)
            {
                CommonTypes.File file = new CommonTypes.File();
                file.Path = filePath.Substring(0, filePath.LastIndexOf("/"));

                int lastDS = filePath.LastIndexOf("/") + 1;

                string nameAndType = filePath.Substring(lastDS);
                // falls der datei kein typ gegeben wurde
                if (nameAndType.Contains("."))
                {
                    int dot = filePath.LastIndexOf(".");
                    file.Name = filePath.Substring(lastDS, dot - lastDS);
                    file.Type = filePath.Substring(dot);
                }
                else
                {
                    file.Name = filePath.Substring(lastDS);
                    file.Type = string.Empty;
                }

                file.Created = System.IO.File.GetCreationTime(filePath);
                file.Modified = System.IO.File.GetLastWriteTime(filePath);

                if(withContent)
                {
                    ExtractContent(file);
                }

                files.Add(file);
            }
            return files;
        }

        public void ExtractContent(CommonTypes.File file)
        {
            string filePath = file.Qualifier;
            if (file.Type.Equals(".pdf"))
            {
                file.Content = _TextExtractor.ExtractPdf(filePath);
            }
            else
            {
                file.Content = _TextExtractor.Extract(filePath);
            }
        }
    }
}
