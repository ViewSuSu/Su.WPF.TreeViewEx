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

            var node = new TreeNodeEx("头节点")
            {
                TreeNodeExIconOptions = new TreeNodeExIconOptions(Icon),
            };
            node.MenuItems.Add(
                new CustomControl.Menu.MenuItemModel(
                    "123123",
                    () =>
                    {
                        string aa = "123";
                    }
                )
            );
            var childNode1 = new TreeNodeEx("123213") { IsShowCheckBox = true };
            childNode1.Children.Add(new TreeNodeEx("123213") { IsShowCheckBox = true });
            node.Children.Add(childNode1);
            node.Children.Add(new TreeNodeEx("123213") { IsShowCheckBox = true });
            node.Children.Add(new TreeNodeEx("123213") { IsShowCheckBox = true });
            node.Children.Add(new TreeNodeEx("123213") { IsShowCheckBox = true });
            node.Children.Add(new TreeNodeEx("123213") { IsShowCheckBox = true });
            this.Provider = TreeViewPanelProvider.GetTreeViewPanelProvider([node]);
            var menuItem = new CustomControl.Menu.MenuItemModel(
                "全选",
                () =>
                {
                    string aa = "123";
                }
            )
            {
                Icon = Icon,
                Shortcut = new CustomControl.Menu.MenuItemModel.MenuShortcut(
                    System.Windows.Input.ModifierKeys.Control,
                    System.Windows.Input.Key.A
                ),
            };
            var menuItem2 = new CustomControl.Menu.MenuItemModel(
                "新增",
                () =>
                {
                    Provider.Controller.SourceTreeNodes.Add(new TreeNodeEx("新增节点"));
                }
            )
            {
                Icon = Icon,
            };
            var menuItem3 = new CustomControl.Menu.MenuItemModel(
                "对头节点进行展开/收缩",
                () =>
                {
                    if (!Provider.Controller.ShowTreeNodeList.FirstOrDefault().IsExpanded)
                        Provider.Controller.ShowTreeNodeList.FirstOrDefault().IsExpanded = true;
                    else
                        Provider.Controller.ShowTreeNodeList.FirstOrDefault().IsExpanded = false;
                }
            )
            {
                Icon = Icon,
            };
            var menuItem4 = new CustomControl.Menu.MenuItemModel(
                "选中头节点",
                () =>
                {
                    if (!Provider.Controller.ShowTreeNodeList.FirstOrDefault().IsChecked.Value)
                        Provider.Controller.ShowTreeNodeList.FirstOrDefault().IsChecked = true;
                    else
                        Provider.Controller.ShowTreeNodeList.FirstOrDefault().IsChecked = false;
                }
            )
            {
                Icon = Icon,
            };
            var menuItem5 = new CustomControl.Menu.MenuItemModel(
                "查看当前选中的所有节点数量",
                () =>
                {
                    Debug.WriteLine(Provider.Controller.GetSelectedNodes().Count);
                }
            )
            {
                Icon = Icon,
            };
            Provider.Controller.Options.MenuItems.Add(menuItem);
            Provider.Controller.Options.MenuItems.Add(menuItem2);
            Provider.Controller.Options.MenuItems.Add(menuItem3);
            Provider.Controller.Options.MenuItems.Add(menuItem4);
            Provider.Controller.Options.MenuItems.Add(menuItem5);
        }

        public TreeViewPanelProvider Provider { get; }
    }
}
