using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace CommonTypes
{
    public class Directory : Hierarchical
    {
        private string _Path;

        public string Path
        {
            get { return _Path; }
            set { _Path = value; OnPropertyChanged(); }
        }

        public override string Qualifier => Path + Name;

        public override bool Equals(object obj)
        {
            return obj is Directory directory &&
                   Name == directory.Name &&
                   _Path == directory._Path &&
                   Path == directory.Path;
        }

        public override int GetHashCode()
        {
            int hashCode = 1892831829;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Name);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(_Path);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Path);
            return hashCode;
        }
    }
}
