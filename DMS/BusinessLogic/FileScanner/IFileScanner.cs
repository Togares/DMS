using CommonTypes;
using System.Collections.Generic;
using BusinessLogic.FileScanner.Events;

namespace BusinessLogic.FileScanner
{
    namespace Events
    {
        public delegate void DriveScanFinishedEventHandler(IEnumerable<Drive> drives);
        public delegate void FileScanFinishedEventHandler(IEnumerable<File> files);
    }

    public interface IFileScanner
    {
        event DriveScanFinishedEventHandler DriveScanFinished;
        event FileScanFinishedEventHandler FileScanFinished;

        void ScanDrives();

        void GetDirectories(Hierarchical hierachical);
        void ExtractContent(File file);

        void ScanDirectory(string path, bool recursive);

    }
}
