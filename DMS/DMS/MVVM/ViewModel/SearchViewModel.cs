using BusinessLogic.FileOpener;
using BusinessLogic.FileScanner;
using CommonTypes;
using CommonTypes.Utility;
using DMS.MVVM.FileViewModel;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace DMS.MVVM.ViewModel
{
    public class SearchViewModel : FileViewOpenerModel
    {
		public SearchViewModel()
		{
			
		}

        public override string ToString()
        {
            return "Search Results";
        }
    }
}
