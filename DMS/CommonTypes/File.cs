using CommonTypes.Utility;

namespace CommonTypes
{
    public class File : Bindable
    {
        private string _Path;

        public string Path
        {
            get { return _Path; }
            set { _Path = value; OnPropertyChanged(); }
        }


        private string _Name;

        public string Name
        {
            get { return _Name; }
            set { _Name = value; OnPropertyChanged(); }
        }

        private int _Size;

        /// <summary>
        /// Bytes
        /// </summary>
        public int Size
        {
            get { return _Size; }
            set { _Size = value; OnPropertyChanged(); }
        }


    }
}
