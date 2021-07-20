using BusinessLogic.FileScanner;
using CommonTypes;
using CommonTypes.Utility;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace DMS.MVVM.ViewModel
{
    public class HomeViewModel : Bindable
    {
		public HomeViewModel()
		{
			
		}

		private ObservableCollection<File> _Files;

		public ObservableCollection<File> Files
		{
			get { return _Files; }
			set { _Files = value; OnPropertyChanged(); }
		}

	}
}
