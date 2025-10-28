using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Su.WPF.CustomControl.TreeViewEx;

namespace Su.WPF.Test
{
    public class MainWindowViewModel
    {
        public ImageSource Icon { get; set; }

        public List<TreeNodeEx> TreeNodeExs { get; set; }

        public MainWindowViewModel()
        {
            string assemblyName = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;
            string packUri = $"pack://application:,,,/{assemblyName};component/仓鼠.png";
            Icon = new BitmapImage(new Uri(packUri, UriKind.Absolute));

            var node = TreeNodeEx.CreateNode("头节点");
            node.TreeNodeExIconOptions = new TreeNodeExIconOptions(Icon);
            node.IsShowCheckBox = true;

            node.MenuItems.Add(
                new CustomControl.Menu.TreeNodeMenu(
                    "123123",
                    node.Delete // node.DeleteSelf())
                )
            );

            var menu = new CustomControl.Menu.TreeNodeMenu("删除自身1", node.Delete);

            node.MenuItems.Add(menu);

            var childNode1 = TreeNodeEx.CreateNode("2222");
            childNode1.IsShowCheckBox = true;
            childNode1.MenuItems.Add(menu);
            var menu2 = new CustomControl.Menu.TreeNodeMenu(
                "删除自身",
                () =>
                {
                    childNode1.Delete();
                }
            );
            childNode1.MenuItems.Add(menu2);

            var c1 = TreeNodeEx.CreateNode("123213");
            c1.IsShowCheckBox = true;
            childNode1.Children.Add(c1);

            // Add child1 to root
            node.Children.Add(childNode1);

            // Additional children under root
            for (int i = 0; i < 4; i++)
            {
                var cn = TreeNodeEx.CreateNode("123213");
                cn.IsShowCheckBox = true;
                node.Children.Add(cn);
            }

            this.Provider = TreeViewPanelProvider.GetTreeViewPanelProvider([node]);

            var menuItem2 = new CustomControl.Menu.TreeViewMenu(
                "新增",
                () => Provider.Controller.SourceTreeNodes.Add(TreeNodeEx.CreateNode("新增节点"))
            )
            {
                Icon = Icon,
            };

            var menuItem3 = new CustomControl.Menu.TreeViewMenu(
                "对头节点进行展开/收缩",
                () =>
                {
                    var head = Provider.Controller.SourceTreeNodes.FirstOrDefault();
                    head.IsExpanded = !head.IsExpanded;
                }
            )
            {
                Icon = Icon,
            };

            var menuItem4 = new CustomControl.Menu.TreeViewMenu(
                "选中头节点",
                () =>
                {
                    var head = Provider.Controller.SourceTreeNodes.FirstOrDefault();
                    head.IsChecked = !(head.IsChecked ?? false);
                }
            )
            {
                Icon = Icon,
            };

            var menuItem5 = new CustomControl.Menu.TreeViewMenu(
                "查看当前选中的所有节点数量",
                () =>
                {
                    Debug.WriteLine(Provider.Controller.SelectedNodes.Count);
                }
            )
            {
                Icon = Icon,
            };

            Provider.Controller.Options.MenuItems.Add(menuItem2);
            Provider.Controller.Options.MenuItems.Add(menuItem3);
            Provider.Controller.Options.MenuItems.Add(menuItem4);
            Provider.Controller.Options.MenuItems.Add(menuItem5);
        }

        public TreeViewPanelProvider Provider { get; }
    }
}
