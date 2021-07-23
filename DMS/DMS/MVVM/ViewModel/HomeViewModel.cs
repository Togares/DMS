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

		private ObservableCollection<File> _LoadedFiles = new ObservableCollection<File>();

		public ObservableCollection<File> LoadedFiles
		{
			get { return _LoadedFiles; }
			set { _LoadedFiles = value; OnPropertyChanged(); }
		}

	}
}
