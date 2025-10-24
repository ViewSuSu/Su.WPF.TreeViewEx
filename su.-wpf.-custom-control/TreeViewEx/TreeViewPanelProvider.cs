namespace Su.WPF.CustomControl.TreeViewEx
{
    public class TreeViewPanelProvider : ObservableObject
    {
        public TreeViewController Controller { get; private set; }
        public System.Windows.Controls.UserControl TreeView { get; private set; }

        private TreeViewPanelProvider() { }

        public static TreeViewPanelProvider GetTreeViewPanelProvider(IList<TreeNodeEx> treeNodeExes)
        {
            var treeView = new TreeViewPanel();
            var controller = TreeViewController.GetTreeViewExProvider(treeNodeExes, treeView);
            treeView.DataContext = new TreeViewPanelExViewModel(controller);
            return new TreeViewPanelProvider() { Controller = controller, TreeView = treeView };
        }
    }
}
