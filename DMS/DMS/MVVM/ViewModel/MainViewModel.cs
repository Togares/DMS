using CommonTypes.Utility;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
﻿using BusinessLogic;

namespace DMS.MVVM.ViewModel
{
    class MainViewModel : Bindable
    {

        public MainViewModel()
        {
            HomeVM = new HomeViewModel();
            DicoveryVM = new DiscoveryViewModel();
            CurrentView = HomeVM;

        }

        #region Commands

        private RelayCommand<object> _HomeViewCommand;
        public RelayCommand<object> HomeViewCommand => _HomeViewCommand = _HomeViewCommand == null ? new RelayCommand<object>(x => CurrentView = HomeVM) : _HomeViewCommand;
        
        private RelayCommand<object> _DiscoveryViewCommand;
        public RelayCommand<object> DiscoveryViewCommand => _DiscoveryViewCommand = _DiscoveryViewCommand == null ? new RelayCommand<object>(x => CurrentView = DicoveryVM) : _DiscoveryViewCommand;

        private RelayCommand<ICloseable> _CloseCommand;
        public RelayCommand<ICloseable> CloseCommand => _CloseCommand = _CloseCommand == null ? new RelayCommand<ICloseable>(x => x.Close()) : _CloseCommand;

        private RelayCommand<object> _MinimizeCommand;
        public RelayCommand<object> MinimizeCommand => _MinimizeCommand = _MinimizeCommand == null ? new RelayCommand<object>(x => Application.Current.MainWindow.WindowState = WindowState.Minimized) : _MinimizeCommand;

        private RelayCommand<object> _MaximizeCommand;
        public RelayCommand<object> MaximizeCommand => _MaximizeCommand = _MaximizeCommand == null ? new RelayCommand<object>(x => Application.Current.MainWindow.WindowState = WindowState.Maximized) : _MaximizeCommand;
        
        private RelayCommand<object> _NormalizeCommand;
        public RelayCommand<object> NormalizeCommand => _NormalizeCommand = _NormalizeCommand == null ? new RelayCommand<object>(x => Application.Current.MainWindow.WindowState = WindowState.Normal) : _NormalizeCommand;
    
        private ICommand openLinkCommand;
        // created as command to open the link in the default users browser in a new tab
        public ICommand OpenLinkCommand => this.openLinkCommand = this.openLinkCommand == null ? new RelayCommand<object>(c => OpenLink()) : OpenLinkCommand;
        
        #endregion Commands

        public HomeViewModel HomeVM { get; set; }
        public DiscoveryViewModel DicoveryVM { get; set; }

        private object _currentView;

        public object CurrentView
        {
            get => _currentView;
            set
            {
                _currentView = value;
                OnPropertyChanged();
            }
        }


        /// <summary>
        /// opens repository with this project
        /// </summary>
        private void OpenLink()
        {
            Process.Start("https://github.com/Togares/DMS");
        }
    }
}
