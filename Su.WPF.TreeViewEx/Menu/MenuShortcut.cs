using System.Windows.Input;

namespace Su.WPF.CustomControl.Menu
{
    /// <summary>
    /// 菜单快捷键类
    /// </summary>
    public sealed class MenuShortcut
    {
        /// <summary>
        /// 修饰键（Ctrl、Alt、Shift等）
        /// </summary>
        public ModifierKeys ModifierKeys { get; }

        /// <summary>
        /// 快捷键主键
        /// </summary>
        public Key Key { get; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="modifierKeys">修饰键</param>
        /// <param name="key">快捷键主键</param>
        public MenuShortcut(ModifierKeys modifierKeys, Key key)
        {
            this.ModifierKeys = modifierKeys;
            this.Key = key;
        }

        /// <summary>
        /// 私有默认构造函数
        /// </summary>
        private MenuShortcut() { }
    }
}
