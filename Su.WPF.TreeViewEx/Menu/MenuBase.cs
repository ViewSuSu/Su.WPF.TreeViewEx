using System.Windows.Controls;
using System.Windows.Media;

namespace Su.WPF.CustomControl.Menu
{
    /// <summary>
    /// 菜单基类
    /// </summary>
    public abstract class MenuBase
    {
        /// <summary>
        /// WPF菜单项控件
        /// </summary>
        public System.Windows.Controls.MenuItem MenuItem { get; } =
            new System.Windows.Controls.MenuItem();

        private string _header;
        private ImageSource _icon;

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
        /// 构造函数
        /// </summary>
        /// <param name="header">菜单标题</param>
        /// <param name="mouseLeftButtonClick">鼠标左键点击事件处理</param>
        public MenuBase(string header)
        {
            this._header = header;
            MenuItem.Header = header;
            MenuItem.Click += (o, e) => OnMouseLeftButtonClick();
        }

        protected abstract void OnMouseLeftButtonClick();
    }
}
