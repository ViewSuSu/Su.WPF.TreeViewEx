using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using UserControl = System.Windows.Controls.UserControl;

namespace Su.WPF.CustomControl.TreeViewEx
{
    /// <summary>
    /// TreeViewPanelEx.xaml 的交互逻辑
    /// </summary>
    public partial class TreeViewPanel : UserControl
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
