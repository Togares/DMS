using CommonTypes.Utility;
using System;

namespace CommonTypes
{
    public class File : Bindable
    {
        public File()
        {

        }

        private long _ID;

        public long ID
        {
            get { return _ID; }
            set { _ID = value; OnPropertyChanged(); }
        }

        private byte[] _Content;

        public byte[] Content
        {
            get { return _Content; }
            set { _Content = value; OnPropertyChanged(); }
        }

        private DateTime _Created;

        public DateTime Created
        {
            get { return _Created; }
            set { _Created = value; OnPropertyChanged(); }
        }
        
        private DateTime _Modified;

        public DateTime Modified
        {
            get { return _Modified; }
            set { _Modified = value; OnPropertyChanged(); }
        }

        private string _Name;

        public string Name
        {
            get { return _Name; }
            set { _Name = value; OnPropertyChanged(); }
        }

        private string _Path;

        public string Path
        {
            get { return _Path; }
            set { _Path = value; OnPropertyChanged(); }
        }

        private string _Type;

        public string Type
        {
            get { return _Type; }
            set { _Type = value; OnPropertyChanged(); }
        }


        /// <summary>
        /// Bytes
        /// </summary>
        public int Size
        {
            get { return _Content.Length; }
        }


    }
}
