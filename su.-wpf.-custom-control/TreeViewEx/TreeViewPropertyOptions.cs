using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using Su.WPF.CustomControl.Menu;
using MenuItem = System.Windows.Controls.MenuItem;

namespace Su.WPF.CustomControl.TreeViewEx
{
    public class TreeViewPropertyOptions : ObservableObject
    {
        public ReadOnlyCollection<MenuItem> ShowMenuItemModels { get; private set; }

        /// <summary>
        /// 过滤选项
        /// </summary>
        public FileterOptions FileterOption { get; internal set; } = new FileterOptions();

        private readonly TreeViewPanel treeViewPanel;

        public BindingList<MenuItemModel> MenuItems { get; set; }
        public bool IsRightButtonWillShowMenu => MenuItems.Count != 0;

        internal TreeViewPropertyOptions(TreeViewPanel treeViewPanel)
        {
            this.treeViewPanel = treeViewPanel;
            MenuItems = [];
            MenuItems.ListChanged += (o, e) =>
            {
                OnPropertyChanged(nameof(IsRightButtonWillShowMenu));
                var currentItem = e.NewIndex >= 0 ? MenuItems[e.NewIndex] : null;
                if (
                    e.ListChangedType == ListChangedType.ItemAdded
                    || e.ListChangedType == ListChangedType.ItemDeleted
                    || e.ListChangedType == ListChangedType.ItemMoved
                )
                {
                    ShowMenuItemModels = new ReadOnlyCollection<MenuItem>(
                        [.. MenuItems.Select(x => x.MenuItem)]
                    );
                }
                if (
                    currentItem != null
                    && !currentItem.Header.Contains(currentItem.ShortcutDisplay)
                )
                {
                    InputBinding inputBinding = new InputBinding(
                        new RelayCommand(currentItem.MouseLeftButtonClick),
                        new KeyGesture(currentItem.Shortcut.Key, currentItem.Shortcut.ModifierKeys)
                    );
                    this.treeViewPanel.InputBindings.Add(inputBinding);
                    currentItem.Header = currentItem.Header + currentItem.ShortcutDisplay;
                }
            };
        }

        private TreeViewPropertyOptions() { }
    }
}
