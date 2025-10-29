using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using UserControl = System.Windows.Controls.UserControl;

namespace Su.WPF.CustomControl.TreeViewEx
{
    /// <summary>
    /// TreeViewPanelEx.xaml 的交互逻辑
    /// </summary>
    internal partial class TreeViewPanel : UserControl
    {
        internal TreeViewPanel()
        {
            InitializeComponent();
        }

        private void StackPanel_PreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is DependencyObject dependencyObject)
            {
                var treeViewItem = VisualTreeHelperUtils.FindParent<TreeViewItem>(dependencyObject);
                if (treeViewItem == null)
                    return;
                treeViewItem.Focus();
            }
        }
    }
}
