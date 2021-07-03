using CommonTypes.Utility;

namespace DMS.MVVM.ViewModel
{
    class MainViewModel : Bindable
    {
        private RelayCommand<object> _HomeViewCommand;
        public RelayCommand<object> HomeViewCommand
        {
            get => _HomeViewCommand = _HomeViewCommand == null ? new RelayCommand<object>(x => CurrentView = HomeVM) : _HomeViewCommand;
        }

        private RelayCommand<object> _DiscoveryViewCommand;
        public RelayCommand<object> DiscoveryViewCommand 
        {
            get => _DiscoveryViewCommand = _DiscoveryViewCommand == null ? new RelayCommand<object>(x => CurrentView = DicoveryVM) : _DiscoveryViewCommand;
        }

        private RelayCommand<ICloseable> _CloseCommand;
        public RelayCommand<ICloseable> CloseCommand
        {
            get => _CloseCommand = _CloseCommand == null ? new RelayCommand<ICloseable>(x => x.Close()) : _CloseCommand;
        }

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

        public MainViewModel()
        {
            HomeVM = new HomeViewModel();
            DicoveryVM = new DiscoveryViewModel();
            CurrentView = HomeVM;
        }
    }
}
