namespace Su.WPF.CustomControl.TreeViewEx
{
    /// <summary>
    /// 树视图面板提供者类
    /// </summary>
    public sealed class TreeViewExProvider : ObservableObjectBase
    {
        /// <summary>
        /// 树视图控制器
        /// </summary>
        public TreeViewExController Controller { get; private set; }

        /// <summary>
        /// 树视图用户控件
        /// </summary>
        public System.Windows.Controls.UserControl TreeView { get; private set; }

        /// <summary>
        /// 私有默认构造函数
        /// </summary>
        private TreeViewExProvider() { }

        /// <summary>
        /// 获取树视图面板提供者实例
        /// </summary>
        /// <param name="treeNodeExes">树节点列表</param>
        /// <returns>树视图面板提供者实例</returns>
        public static TreeViewExProvider GetTreeViewPanelProvider(IList<TreeNodeEx> treeNodeExes)
        {
            var treeView = new TreeViewPanel();
            var controller = TreeViewExController.GetTreeViewExProvider(treeNodeExes, treeView);
            treeView.DataContext = new TreeViewPanelExViewModel(controller);
            return new TreeViewExProvider() { Controller = controller, TreeView = treeView };
        }
    }
}
