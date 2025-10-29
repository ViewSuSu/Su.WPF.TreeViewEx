using Su.WPF.CustomControl.TreeViewEx;

namespace Su.WPF.CustomControl.Menu
{
    /// <summary>
    /// 树节点菜单类
    /// </summary>
    public class TreeNodeMenu : MenuBase
    {
        private readonly Action<TreeNodeEx> action;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="header">菜单标题</param>
        /// <param name="action">菜单点击动作，接收当前的树节点参数</param>
        public TreeNodeMenu(string header, Action<TreeNodeEx> action)
            : base(header)
        {
            this.action = action;
        }

        /// <summary>
        /// 鼠标左键点击事件处理
        /// </summary>
        protected override void OnMouseLeftButtonClick()
        {
            if (MenuItem.DataContext is TreeNodeEx treeNodeEx)
            {
                action?.Invoke(treeNodeEx);
            }
        }
    }
}
