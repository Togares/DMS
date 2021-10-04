using CommonTypes.Utility;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using BusinessLogic;
using BusinessLogic.FileScanner;

namespace DMS.MVVM.ViewModel
{
    class MainViewModel : Bindable
    {
        private IFileScanner _FileScanner = new FileScanner();

        public MainViewModel()
        {
            SearchVM = new SearchViewModel();
            DirectoryVM = new DirectoryViewModel(_FileScanner);
            CurrentView = DirectoryVM;
        }

        #region Commands

        private RelayCommand<object> _HomeViewCommand;
        public RelayCommand<object> HomeViewCommand => _HomeViewCommand = _HomeViewCommand == null ? new RelayCommand<object>(x => CurrentView = SearchVM) : _HomeViewCommand;

        private RelayCommand<object> _DiscoveryViewCommand;
        public RelayCommand<object> DiscoveryViewCommand => _DiscoveryViewCommand = _DiscoveryViewCommand == null ? new RelayCommand<object>(x => CurrentView = DirectoryVM) : _DiscoveryViewCommand;

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
        public ICommand OpenLinkCommand => this.openLinkCommand = this.openLinkCommand ?? new RelayCommand<object>(c => OpenLink());

        private RelayCommand<object> _SearchCommand;
        public RelayCommand<object> SearchCommand => _SearchCommand = _SearchCommand ?? new RelayCommand<object>(x => Search(), x => Database.HasConnection);

        #endregion Commands

        private object _currentView;
        private string _SearchText = string.Empty;

        #region Properties

        public Database Database { get; } = new Database();
        public SearchViewModel SearchVM { get; set; }
        public DirectoryViewModel DirectoryVM { get; set; }

        public object CurrentView
        {
            get => _currentView;
            set
            {
                _currentView = value;
                OnPropertyChanged();
            }
        }
        public string SearchText
        {
            get => _SearchText;
            set { _SearchText = value; OnPropertyChanged(); }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// opens repository with this project
        /// </summary>
        private void OpenLink()
        {
            Process.Start("https://github.com/Togares/DMS");
        }

        private void Search()
        {
            SearchVM.LoadedFiles.Clear();
            foreach (CommonTypes.File file in Database.Search(SearchText))
            {
                if (!SearchVM.LoadedFiles.Contains(file))
                {
                    SearchVM.LoadedFiles.Add(file);
                }
            }
            CurrentView = SearchVM;
        }

        #endregion Methods

    }
}
