using BusinessLogic.FileOpener;
using CommonTypes;
using CommonTypes.Utility;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMS.MVVM.FileViewModel
{
    public class FileViewOpenerModel : Bindable
    {
        private ObservableCollection<File> _LoadedFiles = new ObservableCollection<File>();
        public ObservableCollection<File> LoadedFiles
        {
            get { return _LoadedFiles; }
            set { _LoadedFiles = value; OnPropertyChanged(); }
        }

        public File _SelectedFile;
        public File SelectedFile
        {
            get { return _SelectedFile; }
            set
            {
                _SelectedFile = value;
                OnPropertyChanged();
            }
        }

        private RelayCommand<object> _OpenCommand;
        public RelayCommand<object> OpenCommand => _OpenCommand = _OpenCommand ?? new RelayCommand<object>(x => FileOpener.OpenFile(SelectedFile), x => _SelectedFile != null);

        private RelayCommand<object> _ShowExplorerCommand;

        public RelayCommand<object> ShowExplorerCommand => _ShowExplorerCommand = _ShowExplorerCommand ?? new RelayCommand<object>(x => FileOpener.ShowExplorer(SelectedFile), x => SelectedFile != null);

    }
}
