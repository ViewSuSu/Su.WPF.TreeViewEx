using System;
using System.Collections.Generic;
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
            var node = new TreeNodeEx("123");
            node.MenuItems.Add(
                new CustomControl.Menu.MenuItemModel(
                    "123123",
                    () =>
                    {
                        string aa = "123";
                    }
                )
            );
            node.Children.Add(new TreeNodeEx("123213"));
            node.Children.Add(new TreeNodeEx("123213"));
            node.Children.Add(new TreeNodeEx("123213"));
            node.Children.Add(new TreeNodeEx("123213"));
            node.Children.Add(new TreeNodeEx("123213"));
            this.Provider = TreeViewPanelProvider.GetTreeViewPanelProvider([node]);
            string assemblyName = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;
            string packUri = $"pack://application:,,,/{assemblyName};component/仓鼠.png";
            Icon = new BitmapImage(new Uri(packUri, UriKind.Absolute));
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
                "节点展开测试",
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
            Provider.Controller.Options.MenuItems.Add(menuItem);
            Provider.Controller.Options.MenuItems.Add(menuItem2);
            Provider.Controller.Options.MenuItems.Add(menuItem3);
        }

        public TreeViewPanelProvider Provider { get; }
    }
}
