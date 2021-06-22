using CommonTypes.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonTypes
{
    public class Drive : Bindable
    {
		private string _Name;

		public string Name
		{
			get { return _Name; }
			set { _Name = value; OnPropertyChanged(); }
		}
	}
}
