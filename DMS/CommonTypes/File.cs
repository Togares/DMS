using CommonTypes.Utility;
using System;
using System.Collections.Generic;

namespace CommonTypes
{
    public class File : Item
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
        public int Size => _Content.Length;

        public string NameAndType => Name + Type;

        public string Qualifier => Path + NameAndType;

        public override bool Equals(object obj)
        {
            return obj is File file &&
                   Name == file.Name &&
                   _ID == file._ID &&
                   ID == file.ID &&
                   _Created == file._Created &&
                   Created == file.Created &&
                   _Modified == file._Modified &&
                   Modified == file.Modified &&
                   _Path == file._Path &&
                   Path == file.Path &&
                   Name == file.Name &&
                   _Type == file._Type &&
                   Type == file.Type;
        }

        public override int GetHashCode()
        {
            int hashCode = -1733013141;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Name);
            hashCode = hashCode * -1521134295 + _ID.GetHashCode();
            hashCode = hashCode * -1521134295 + ID.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<byte[]>.Default.GetHashCode(_Content);
            hashCode = hashCode * -1521134295 + EqualityComparer<byte[]>.Default.GetHashCode(Content);
            hashCode = hashCode * -1521134295 + _Created.GetHashCode();
            hashCode = hashCode * -1521134295 + Created.GetHashCode();
            hashCode = hashCode * -1521134295 + _Modified.GetHashCode();
            hashCode = hashCode * -1521134295 + Modified.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(_Path);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Path);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Name);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(_Type);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Type);
            hashCode = hashCode * -1521134295 + Size.GetHashCode();
            return hashCode;
        }
    }
}
