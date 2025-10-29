using System.Windows.Input;

namespace Su.WPF.CustomControl.Menu
{
    /// <summary>
    /// 树视图菜单类（支持快捷键）
    /// </summary>
    public class TreeViewMenu : MenuBase
    {
        internal const string ShortcutPropertyName = nameof(Shortcut);
        internal readonly Action mouseLeftButtonClick;

        /// <summary>
        /// 菜单快捷键
        /// </summary>
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

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="header">菜单标题</param>
        /// <param name="mouseLeftButtonClick">鼠标左键点击事件处理</param>
        public TreeViewMenu(string header, Action mouseLeftButtonClick)
            : base(header)
        {
            this.mouseLeftButtonClick = mouseLeftButtonClick;
        }

        protected override void OnMouseLeftButtonClick()
        {
            mouseLeftButtonClick?.Invoke();
        }
    }
}
