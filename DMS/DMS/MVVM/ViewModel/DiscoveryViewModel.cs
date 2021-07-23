using BusinessLogic;
using BusinessLogic.FileOpener;
using BusinessLogic.FileScanner;
using CommonTypes;
using CommonTypes.Utility;
using DMS.MVVM.FileViewModel;
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
    public class DiscoveryViewModel : FileViewOpenerModel
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
            insertWatcher.EventArrived += new EventArrivedEventHandler((sender, evt) => Application.Current.Dispatcher.Invoke(() => ScanDrives()));
            insertWatcher.Start();

            WqlEventQuery removeQuery = new WqlEventQuery("SELECT * FROM __InstanceDeletionEvent WITHIN 2 WHERE TargetInstance ISA 'Win32_USBHub'");
            ManagementEventWatcher removeWatcher = new ManagementEventWatcher(removeQuery);
            removeWatcher.EventArrived += new EventArrivedEventHandler((sender, evt) => Application.Current.Dispatcher.Invoke(() => ScanDrives()));
            removeWatcher.Start();

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

        #endregion Properties

        #region Commands

        private RelayCommand<object> _SaveCommand;
        public RelayCommand<object> SaveCommand => _SaveCommand = _SaveCommand ?? new RelayCommand<object>(x => SaveSelectedFile(), x => _Database.HasConnection && _SelectedFile != null);

        #endregion Commands

        #region Methods

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

        private void SaveSelectedFile()
        {
            _FileScanner.ExtractContent(SelectedFile);
            _Database.Save(SelectedFile);
        }

        #endregion Methods

    }
}
