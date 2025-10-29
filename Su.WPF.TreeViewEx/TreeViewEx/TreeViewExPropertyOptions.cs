using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using Su.WPF.CustomControl.Menu;
using MenuItem = System.Windows.Controls.MenuItem;

namespace Su.WPF.CustomControl.TreeViewEx
{
    /// <summary>
    /// 树视图属性选项配置类
    /// </summary>
    public sealed class TreeViewExPropertyOptions : ObservableObjectBase
    {
        private readonly TreeViewPanel treeViewPanel;

        /// <summary>
        /// 显示的菜单项模型只读集合
        /// </summary>
        public ReadOnlyCollection<MenuItem> MenuItems { get; private set; }

        /// <summary>
        /// 菜单项集合
        /// </summary>
        public BindingList<TreeViewMenu> MenuItemModels { get; } = [];

        /// <summary>
        /// 是否显示右键菜单（当有菜单项时显示）
        /// </summary>
        public bool IsRightButtonWillShowMenu => MenuItemModels.Count != 0;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="treeViewPanel">树视图面板</param>
        internal TreeViewExPropertyOptions(TreeViewPanel treeViewPanel)
        {
            this.treeViewPanel = treeViewPanel;
            MenuItemModels.ListChanged += (o, e) =>
            {
                OnPropertyChanged(nameof(IsRightButtonWillShowMenu));
                var currentItem = e.NewIndex >= 0 ? MenuItemModels[e.NewIndex] : null;

                if (
                    e.ListChangedType == ListChangedType.ItemAdded
                    || e.ListChangedType == ListChangedType.ItemDeleted
                    || e.ListChangedType == ListChangedType.ItemMoved
                )
                {
                    MenuItems = new ReadOnlyCollection<MenuItem>(
                        [.. MenuItemModels.Select(x => x.MenuItem)]
                    );
                }

                if (
                    currentItem != null
                    && !currentItem.Header.Contains(currentItem.ShortcutDisplay)
                )
                {
                    InputBinding inputBinding = new InputBinding(
                        new MyRelayCommand(currentItem.mouseLeftButtonClick),
                        new KeyGesture(currentItem.Shortcut.Key, currentItem.Shortcut.ModifierKeys)
                    );
                    this.treeViewPanel.InputBindings.Add(inputBinding);
                    currentItem.Header += currentItem.ShortcutDisplay;
                }
            };
        }

        /// <summary>
        /// 私有默认构造函数
        /// </summary>
        private TreeViewExPropertyOptions() { }
    }
}
