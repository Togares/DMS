using BusinessLogic.FileScanner;
using CommonTypes;
using CommonTypes.Utility;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace DMS.MVVM.ViewModel
{
    public class HomeViewModel : Bindable
    {
        private IFileScanner _FileScanner;

        public HomeViewModel(IFileScanner fileScanner)
        {            
            _FileScanner = fileScanner;
            _FileScanner.DriveScanFinished += DriveScanFinished;
            _FileScanner.FileScanFinished += FileScanFinished;
            _FileScanner.ScanDrives();
        }

        /// <summary>
        /// Scant nach Laufwerken
        /// </summary>
        public void ScanDrives()
        {
            _FileScanner.ScanDrives();
        }

        /// <summary>
        /// Scant unter dem angegebenen Pfad nach Dateien.
        /// Insofern <paramref name="recursive"/> true ist, werden alle Unterverzeichnisse mitgescannt.
        /// Ergebnisse landen in Files
        /// </summary>
        /// <param name="path">Der Pfad unter dem initial gescannt wird</param>
        /// <param name="recursive">Angabe ob Rekursiv gesucht werden soll</param>
        public void ScanFiles(string path, bool recursive)
        {
            _FileScanner.ScanDirectory(path, recursive);
        }

        /// <summary>
        /// Listener für FileScanFinished des Scanners
        /// </summary>
        /// <param name="drives">Liste der gefundenen Dateien</param>
        private void FileScanFinished(IEnumerable<File> files)
        {
            foreach (File file in files)
            {
                Files.Add(file);
            }
        }

        /// <summary>
        /// Listener für DriveScanFinished des Scanners
        /// </summary>
        /// <param name="drives">Liste der gefundenen Laufwerke</param>
        private void DriveScanFinished(IEnumerable<Drive> drives)
        {
            foreach (Drive drive in drives)
            {
                Drives.Add(drive);
            }
        }

        #region Properties

        private ObservableCollection<File> _Files = new ObservableCollection<File>();

        public ObservableCollection<File> Files
        {
            get { return _Files; }
            set { _Files = value; OnPropertyChanged(); }
        }

        private File _SelectedFile;

        public File SelectedFile
        {
            get { return _SelectedFile; }
            set { _SelectedFile = value; OnPropertyChanged(); }
        }

        private ObservableCollection<Drive> _Drives = new ObservableCollection<Drive>();

        public ObservableCollection<Drive> Drives
        {
            get { return _Drives; }
            set { _Drives = value; OnPropertyChanged(); }
        }

        private Drive _SelectedDrive;

        public Drive SelectedDrive
        {
            get { return _SelectedDrive; }
            set { _SelectedDrive = value; OnPropertyChanged(); }
        }

        #endregion Properties
    }
}
