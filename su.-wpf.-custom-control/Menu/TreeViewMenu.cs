using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Su.WPF.CustomControl.Menu
{
    public abstract class MenuBase
    {
        public System.Windows.Controls.MenuItem MenuItem { get; } =
            new System.Windows.Controls.MenuItem();
        private string _header;
        private ImageSource _icon;
        internal Action MouseLeftButtonClick { get; set; }

        /// <summary>
        /// 菜单标题
        /// </summary>
        public string Header
        {
            get => _header;
            set
            {
                _header = value;
                MenuItem.Header = value;
            }
        }

        /// <summary>
        /// 菜单图标
        /// </summary>
        public ImageSource Icon
        {
            get => _icon;
            set
            {
                _icon = value;
                MenuItem.Icon =
                    value != null
                        ? new System.Windows.Controls.Image
                        {
                            Source = value,
                            Stretch = Stretch.Uniform,
                        }
                        : null;
            }
        }

        /// <summary>
        /// 构造
        /// </summary>
        public MenuBase(string header, Action mouseLeftButtonClick)
        {
            this._header = header;
            this.MouseLeftButtonClick = mouseLeftButtonClick;
            MenuItem.Header = header;
            MenuItem.Click += (o, e) => this.MouseLeftButtonClick?.Invoke();
        }
    }

    public class TreeNodeMenu : MenuBase
    {
        public TreeNodeMenu(string header, Action mouseLeftButtonClick)
            : base(header, mouseLeftButtonClick) { }
    }

    public class MenuShortcut
    {
        public ModifierKeys ModifierKeys { get; }
        public Key Key { get; }

        public MenuShortcut(ModifierKeys modifierKeys, Key key)
        {
            this.ModifierKeys = modifierKeys;
            this.Key = key;
        }

        private MenuShortcut() { }
    }

    public class TreeViewMenu : MenuBase
    {
        internal const string ShortcutPropertyName = nameof(Shortcut);

        public MenuShortcut Shortcut { get; set; }

        /// <summary>
        /// 快捷键显示文本，如：Ctrl + A
        /// </summary>
        internal string ShortcutDisplay
        {
            get
            {
                if (Shortcut == null)
                    return string.Empty;
                if (Shortcut.ModifierKeys == ModifierKeys.None && Shortcut.Key == Key.None)
                    return string.Empty;
                return Shortcut.ModifierKeys == ModifierKeys.None && Shortcut.Key == Key.None
                    ? ""
                    : $"({new KeyGesture(Shortcut.Key, Shortcut.ModifierKeys).GetDisplayStringForCulture(null)})";
            }
        }

        public TreeViewMenu(string header, Action mouseLeftButtonClick)
            : base(header, mouseLeftButtonClick) { }
    }
}
