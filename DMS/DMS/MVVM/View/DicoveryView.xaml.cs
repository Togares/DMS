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
            if (DataContext is DiscoveryViewModel discoveryViewModel)
            {
                discoveryViewModel.SelectedHierarchical = e.NewValue as CommonTypes.Hierarchical;
            }
        }

        private void FileSystemTree_Expanded(object sender, RoutedEventArgs e)
        {
            if (DataContext is DiscoveryViewModel discoveryViewModel)
            {
                discoveryViewModel.LoadSubHierarchie((e.OriginalSource as TreeViewItem).Header as CommonTypes.Hierarchical);
            }
        }
    }
}
