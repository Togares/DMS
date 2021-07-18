using BusinessLogic.FileScanner;
using CommonTypes;
using CommonTypes.Utility;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace DMS.MVVM.ViewModel
{
    public class DiscoveryViewModel : Bindable
    {
        private IFileScanner _FileScanner;

        public DiscoveryViewModel()
        {

        }

        public DiscoveryViewModel(IFileScanner fileScanner)
        {
            _FileScanner = fileScanner;
            _FileScanner.DriveScanFinished += DriveScanFinished;
            _FileScanner.FileScanFinished += FileScanFinished;
            ScanDrives();
        }

        public void LoadSubHierarchie(Hierarchical hierachical)
        {
            _FileScanner.GetDirectories(hierachical);
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
                //currentDrive.Items.Add(file);
                //Files.Add(file);
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
                if(!Drives.Contains(drive))
                {
                    Drives.Add(drive);
                    LoadSubHierarchie(drive);
                }
            }
        }

        #region Properties

        private ObservableCollection<Item> _Drives = new ObservableCollection<Item>();

        public ObservableCollection<Item> Drives
        {
            get { return _Drives; }
            set { _Drives = value; OnPropertyChanged(); }
        }

        private Drive _SelectedDrive;

        public Drive SelectedDrive
        {
            get { return _SelectedDrive; }
            set
            {
                _SelectedDrive = value;
                _FileScanner.GetDirectories(_SelectedDrive);
                OnPropertyChanged();
            }
        }

        private Directory _SelectedDirectory;

        public Directory SelectedDirectory
        {
            get { return _SelectedDirectory; }
            set
            {
                _SelectedDirectory = value;
                _FileScanner.GetDirectories(_SelectedDirectory);
                OnPropertyChanged();
            }
        }
               
        private File _SelectedFile;

        public File SelectedFile
        {
            get { return _SelectedFile; }
            set { _SelectedFile = value; OnPropertyChanged(); }
        }

        #endregion Properties
    }
}
