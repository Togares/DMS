using BusinessLogic;
using CommonTypes.Utility;

namespace DMS.MVVM.ViewModel
{
    class MainViewModel : Bindable
    {
        public MainViewModel()
        {
            HomeVM = new HomeViewModel();
            DicoveryVM = new DiscoveryViewModel();
            CurrentView = HomeVM;

            ConnectionTest con = new ConnectionTest();
        }

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
    }
}
