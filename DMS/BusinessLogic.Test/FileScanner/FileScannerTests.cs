using CommonTypes;
using System.Collections.Generic;
using BusinessLogic.FileScanner.Events;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BusinessLogic.FileScanner.Tests
{
    [TestClass()]
    public class FileScannerTests
    {
        [TestMethod()]
        public void ScanDrivesTest()
        {
            // Arrange
            IFileScanner scanner = new FileScanner();
            List<Drive> drives = new List<Drive>();
            scanner.DriveScanFinished += new DriveScanFinishedEventHandler(x => drives.AddRange(x));

            // Act
            scanner.ScanDrives();

            // Assert
            Assert.IsTrue(drives.Count > 0);
        }

        /// <summary>
        /// Der Test ist schlecht, da er darauf basiert, dass unter C:/temp nur eine Datei existiert.
        /// Es gibt keine Möglichkeit zu garantieren, dass das Verzeichnis auf jedem Entwicklungsrechner gleich aufgebaut ist.
        /// Vorgesehene Verzeichnisstruktur:
        /// C:/temp
        ///     +---temp_root.txt
        /// </summary>
        [TestMethod()]
        public void ScanDirectory_NonRecursive_Test()
        {
            // Arrange
            IFileScanner scanner = new FileScanner();
            List<File> files = new List<File>();
            scanner.FileScanFinished += new FileScanFinishedEventHandler(x => files.AddRange(x));
            string directory = "C:/temp";

            // Act
            scanner.ScanDirectory(directory, false);

            // Assert
            Assert.IsTrue(files.Count == 1, $"Tatsächliche Anzahl: {files.Count}");
        }

        /// <summary>
        /// Der Test ist schlecht, da er darauf basiert, dass unter C:/temp bestimmte Dateien und Ordner existieren.
        /// Es gibt keine Möglichkeit zu garantieren, dass das Verzeichnis auf jedem Entwicklungsrechner gleich aufgebaut ist.
        /// Vorgesehene Verzeichnisstruktur:
        /// C:/temp
        ///     +---temp_root.txt
        ///     +---DMS
        ///         +---dms_root.txt
        ///         +---directory
        ///             +---dir_2.txt
        ///             +---directory2
        ///                 +---end.txt
        /// </summary>
        [TestMethod()]
        public void ScanDirectory_Recursive_Test()
        {
            // Arrange
            IFileScanner scanner = new FileScanner();
            List<File> files = new List<File>();
            scanner.FileScanFinished += new FileScanFinishedEventHandler(x => files.AddRange(x));
            string directory = "C:/temp";

            // Act
            scanner.ScanDirectory(directory, true);

            // Assert
            Assert.IsTrue(files.Count == 4, $"Tatsächliche Anzahl: {files.Count}");
        }
    }
}