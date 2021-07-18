using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace CommonTypes
{
    public class Drive : Hierarchical
    {
        private string _Label;

        public string Label
        {
            get { return _Label; }
            set { _Label = value; OnPropertyChanged(); }
        }

        private string _Type;

        public string Type
        {
            get { return _Type; }
            set { _Type = value; OnPropertyChanged(); }
        }

        public override string Qualifier => Name;

        public override bool Equals(object obj)
        {
            return obj is Drive drive &&
                   Name == drive.Name &&
                   _Label == drive._Label &&
                   Label == drive.Label &&
                   _Type == drive._Type &&
                   Type == drive.Type;
        }

        public override int GetHashCode()
        {
            int hashCode = -245901650;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Name);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(_Label);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Label);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(_Type);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Type);
            return hashCode;
        }
    }
}
