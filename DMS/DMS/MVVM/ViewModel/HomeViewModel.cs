using BusinessLogic.FileOpener;
using BusinessLogic.FileScanner;
using CommonTypes;
using CommonTypes.Utility;
using DMS.MVVM.FileViewModel;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace DMS.MVVM.ViewModel
{
    public class HomeViewModel : FileViewOpenerModel
    {
		public HomeViewModel()
		{
			
		}

        public override string ToString()
        {
            return "Search Results";
        }
    }
}
