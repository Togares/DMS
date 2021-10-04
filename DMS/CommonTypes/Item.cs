using CommonTypes.Utility;

namespace CommonTypes
{
    public class Item : Bindable
    {
        private string _Name;

        public string Name
        {
            get { return _Name; }
            set { _Name = value; OnPropertyChanged(); }
        }
        public virtual string Qualifier { get; }
    }
}
