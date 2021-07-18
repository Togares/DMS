using System.Collections.ObjectModel;

namespace CommonTypes
{
    public class Hierarchical : Item
    {
        private ObservableCollection<Directory> _Directories = new ObservableCollection<Directory>();

        public ObservableCollection<Directory> Directories
        {
            get { return _Directories; }
            set { _Directories = value; OnPropertyChanged(); }
        }

        private ObservableCollection<File> _Files = new ObservableCollection<File>();

        public ObservableCollection<File> Files
        {
            get { return _Files; }
            set { _Files = value; OnPropertyChanged(); }
        }

        public virtual string Qualifier { get; }
    }
}
