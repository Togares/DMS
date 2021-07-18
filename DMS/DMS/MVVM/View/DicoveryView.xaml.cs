using DMS.MVVM.ViewModel;
using System.Windows;
using System.Windows.Controls;

namespace DMS.MVVM.View
{
    /// <summary>
    /// Interaction logic for DicoveryView.xaml
    /// </summary>
    public partial class DicoveryView : UserControl
    {
        public DicoveryView()
        {
            InitializeComponent();
        }

        private void TreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (e.NewValue.GetType() == typeof(CommonTypes.Drive))
            {
                (DataContext as DiscoveryViewModel).SelectedDrive = e.NewValue as CommonTypes.Drive;
            }
            else if (e.NewValue.GetType() == typeof(CommonTypes.Directory))
            {
                (DataContext as DiscoveryViewModel).SelectedDirectory = e.NewValue as CommonTypes.Directory;
            }
        }

        private void FileSystemTree_Expanded(object sender, RoutedEventArgs e)
        {
            (DataContext as DiscoveryViewModel).LoadSubHierarchie((e.OriginalSource as TreeViewItem).Header as CommonTypes.Hierarchical);
        }
    }
}
