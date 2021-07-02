using CommonTypes;
using System.Collections.Generic;
using BusinessLogic.FileScanner.Events;
using System.IO;
using Visualis.Extractor;
using System.Text;

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
        public void ScanDrives()
        {
            List<Drive> drives = new List<Drive>();

            foreach (DriveInfo driveInfo in DriveInfo.GetDrives())
            {
                Drive drive = new Drive();
                drive.Name = driveInfo.Name;
                string type = string.Empty;
                switch (driveInfo.DriveType)
                {
                    case DriveType.Unknown:
                        type = "Unknown"; break;
                    case DriveType.NoRootDirectory:
                        type = "NoRootDirectory"; break;
                    case DriveType.Removable:
                        type = "Removable"; break;
                    case DriveType.Fixed:
                        type = "Fixed"; break;
                    case DriveType.Network:
                        type = "Network"; break;
                    case DriveType.CDRom:
                        type = "CDRom"; break;
                    case DriveType.Ram:
                        type = "Ram"; break;
                }
                drive.Type = type;
                try
                { // nicht benannte oder nicht bereite laufwerke lösen eine exception aus
                    drive.Label = driveInfo.VolumeLabel;
                }
                catch (IOException)
                {
                    drive.Label = string.Empty;
                }
                drives.Add(drive);
            }

            DriveScanFinished?.Invoke(drives);
        }

        /// <summary>
        /// Scant in einem Verzeichnis nach Dateien.
        /// Insofern rekursiv gescant wird, werden auch alle Unterverzeichnisse nach Dateien durchsucht.
        /// Nachdem der Vorgang beendet wurde, wird FileScanFinished ausgelöst.
        /// </summary>
        /// <param name="path">Das Verzeichnis, Vollqualifiziert</param>
        /// <param name="recursive">Angabe, ob rekursiv gescannt wird</param>
        public void ScanDirectory(string path, bool recursive)
        {
            List<CommonTypes.File> files = new List<CommonTypes.File>();

            files.AddRange(GetFiles(path));

            if (recursive)
            {
                files.AddRange(ScanSubdirectories(path));
            }

            FileScanFinished?.Invoke(files);
        }

        /// <summary>
        /// Baut CommonTypes.File Objekte aus alles Dateien, die unter path gefunden werden
        /// </summary>
        /// <param name="path">Der Pfad unter dem nach Dateien gesucht wird</param>
        /// <returns>Eine Liste mit CommonTypes.File Objekten, die die Dateien des Dateisystems repräsentieren</returns>
        private List<CommonTypes.File> GetFiles(string path)
        {
            List<CommonTypes.File> files = new List<CommonTypes.File>();
            string[] paths = Directory.GetFiles(path);

            // nicht in der foreach, da der Iterationsvariable nichts zugewiesen werden kann
            for(int i = 0; i < paths.Length; ++i)
            {
                paths[i] = paths[i].Replace("\\", "/");
            }

            foreach (string filePath in paths)
            {
                CommonTypes.File file = new CommonTypes.File();                
                file.Path = filePath.Substring(0, filePath.LastIndexOf("/") - 1);
                file.Name = filePath.Substring(filePath.LastIndexOf("/") + 1);
                file.Type = filePath.Substring(filePath.LastIndexOf("."));
                file.Created = System.IO.File.GetCreationTime(filePath);
                file.Modified = System.IO.File.GetLastWriteTime(filePath);

                if (file.Type.Equals(".pdf"))
                {
                    file.Content = Encoding.UTF8.GetBytes(_TextExtractor.ExtractPdf(filePath));
                }
                else
                {
                    file.Content = Encoding.UTF8.GetBytes(_TextExtractor.Extract(filePath));
                }
                files.Add(file);
            }
            return files;
        }
        /// <summary>
        /// Scannt die Dateien eines Verzeichnisses mitsamt seinen Unterverzeichnissen
        /// </summary>
        /// <param name="path">Der Vollqualifizierte Pfad</param>
        /// <returns></returns>
        private List<CommonTypes.File> ScanSubdirectories(string path)
        {
            List<CommonTypes.File> files = new List<CommonTypes.File>();
            foreach (string directory in Directory.GetDirectories(path))
            {
                files.AddRange(GetFiles(directory));
                files.AddRange(ScanSubdirectories(directory));
            }
            return files;
        }
    }
}
