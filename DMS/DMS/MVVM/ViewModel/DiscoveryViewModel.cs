using BusinessLogic;
using BusinessLogic.FileScanner;
using CommonTypes;
using CommonTypes.Utility;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Management;
using System.Windows;

namespace DMS.MVVM.ViewModel
{
    public class DiscoveryViewModel : Bindable
    {
        private IFileScanner _FileScanner;
        private Database _Database = new Database();

        public DiscoveryViewModel()
        {

        }

        public DiscoveryViewModel(IFileScanner fileScanner)
        {
            _FileScanner = fileScanner;
            _FileScanner.DriveScanFinished += DriveScanFinished;
            ScanDrives();

            //https://stackoverflow.com/a/19435744
            // Hier wird auf das Windows-Event reagiert, dass aufeglöst wird, wenn ein Laufwerk hinzugefügt respektive entfernt wird
            WqlEventQuery insertQuery = new WqlEventQuery("SELECT * FROM __InstanceCreationEvent WITHIN 2 WHERE TargetInstance ISA 'Win32_USBHub'");

            ManagementEventWatcher insertWatcher = new ManagementEventWatcher(insertQuery);
            insertWatcher.EventArrived += new EventArrivedEventHandler(DeviceInsertedEvent);
            insertWatcher.Start();

            WqlEventQuery removeQuery = new WqlEventQuery("SELECT * FROM __InstanceDeletionEvent WITHIN 2 WHERE TargetInstance ISA 'Win32_USBHub'");
            ManagementEventWatcher removeWatcher = new ManagementEventWatcher(removeQuery);
            removeWatcher.EventArrived += new EventArrivedEventHandler(DeviceRemovedEvent);
            removeWatcher.Start();

        }
        private void DeviceInsertedEvent(object sender, EventArrivedEventArgs e)
        {
            Application.Current.Dispatcher.Invoke(() => ScanDrives());
        }

        private void DeviceRemovedEvent(object sender, EventArrivedEventArgs e)
        {
            Application.Current.Dispatcher.Invoke(() => ScanDrives());
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
        /// Listener für DriveScanFinished des Scanners
        /// </summary>
        /// <param name="drives">Liste der gefundenen Laufwerke</param>
        private void DriveScanFinished(IEnumerable<Drive> drives)
        {
            foreach (Drive drive in drives)
            {
                if (!Drives.Contains(drive))
                {
                    Drives.Add(drive);
                    LoadSubHierarchie(drive);
                }
            }
            // Laufwerke, die in der Collection im ViewModel sind, aber nicht beim Scannen nach Laufwerken
            // gefunden wurden, sind Laufwerke, die entfernt wurden.
            var removedDrives = Drives.Where(x => !drives.Contains(x)).ToList();
            foreach (Drive drive1 in removedDrives)
            {
                Drives.Remove(drive1);
            }
        }

        #region Properties

        private ObservableCollection<Item> _Drives = new ObservableCollection<Item>();

        public ObservableCollection<Item> Drives
        {
            get { return _Drives; }
            set { _Drives = value; OnPropertyChanged(); }
        }

        private Hierarchical _SelectedHierarchical;

        public Hierarchical SelectedHierarchical
        {
            get { return _SelectedHierarchical; }
            set { _SelectedHierarchical = value; OnPropertyChanged(); }
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
            set
            {
                _SelectedFile = value;
                OnPropertyChanged();
            }
        }

        #endregion Properties

        #region Commands

        private RelayCommand<object> _OpenCommand;
        public RelayCommand<object> OpenCommand => _OpenCommand = _OpenCommand ?? new RelayCommand<object>(x => OpenSelectedFile());

        private RelayCommand<object> _SaveCommand;
        public RelayCommand<object> SaveCommand => _SaveCommand = _SaveCommand ?? new RelayCommand<object>(x => SaveSelectedFile());

        #endregion Commands
        private void SaveSelectedFile()
        {
            _FileScanner.ExtractContent(SelectedFile);
            _Database.Save(SelectedFile);
        }

        private void OpenSelectedFile()
        {
            string path = @SelectedFile.Qualifier;
            ProcessStartInfo psi = new ProcessStartInfo();
            psi.FileName = System.IO.Path.GetFileName(path);
            psi.WorkingDirectory = System.IO.Path.GetDirectoryName(path);
            psi.ErrorDialog = true;
            try
            {
                Process.Start(psi);
            }
            catch (Win32Exception e)
            {
                if (e.NativeErrorCode == 1223) // vom Benutzer abgebrochen
                    return;
                else throw;
            }
        }

    }
}
