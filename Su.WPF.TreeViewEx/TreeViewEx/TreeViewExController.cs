using System.Collections.ObjectModel;

namespace Su.WPF.CustomControl.TreeViewEx
{
    /// <summary>
    /// 树视图控制器，负责管理树节点的操作和管理数据源
    /// </summary>
    public sealed class TreeViewExController : ObservableObjectBase
    {
        /// <summary>
        /// 当前选中的节点列表
        /// </summary>
        public List<TreeNodeEx> SelectedNodes { get; set; } = [];

        /// <summary>
        /// 源数据树节点集合，外部可修改
        /// </summary>
        public ObservableCollection<TreeNodeEx> SourceTreeNodes { get; private set; }

        /// <summary>
        /// 树视图属性选项配置
        /// </summary>
        public TreeViewExPropertyOptions Options { get; internal set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="sourceTreeNodes">源树节点列表</param>
        /// <param name="treeViewPanel">树视图面板</param>
        internal TreeViewExController(
            IList<TreeNodeEx> sourceTreeNodes,
            TreeViewPanel treeViewPanel
        )
        {
            this.SourceTreeNodes = new ObservableCollection<TreeNodeEx>(sourceTreeNodes);
            Options = new TreeViewExPropertyOptions(treeViewPanel);

            // 为所有节点设置控制器引用
            SetControllerRecursive(SourceTreeNodes, this);

            SourceTreeNodes.CollectionChanged += (o, e) =>
            {
                if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
                {
                    foreach (TreeNodeEx newNode in e.NewItems)
                    {
                        SetControllerRecursive([newNode], this);
                    }
                }
            };
        }

        /// <summary>
        /// 删除节点（支持递归删除节点及其子节点）
        /// </summary>
        /// <param name="nodes">要删除的节点列表</param>
        internal void DeleteNodes(IEnumerable<TreeNodeEx> nodes)
        {
            if (nodes == null)
                return;

            foreach (var node in nodes.ToList()) // 避免遍历时结构变化
            {
                RemoveNode(node);
            }
        }

        /// <summary>
        /// 删除单个节点
        /// </summary>
        /// <param name="node">要删除的节点</param>
        private void RemoveNode(TreeNodeEx node)
        {
            if (node == null)
                return;

            // 递归移除所有子节点（从SelectedNodes中）
            RemoveFromSelectedRecursive(node);

            // 从父节点或SourceTreeNodes中移除
            if (node.FatherNode == null)
            {
                // 根节点：从SourceTreeNodes中移除
                SourceTreeNodes.Remove(node);
            }
            else
            {
                // 非根节点：从父节点的Children中移除
                node.FatherNode.Children.Remove(node);
            }

            // 清理引用
            node.FatherNode = null;
            node.Controller = null;
        }

        /// <summary>
        /// 递归从SelectedNodes中移除节点及其所有子节点
        /// </summary>
        /// <param name="node">要移除的节点</param>
        private void RemoveFromSelectedRecursive(TreeNodeEx node)
        {
            if (node == null)
                return;

            // 移除当前节点
            if (SelectedNodes.Contains(node))
            {
                SelectedNodes.Remove(node);
            }

            // 递归移除所有子节点
            foreach (var child in node.Children)
            {
                RemoveFromSelectedRecursive(child);
            }
        }

        /// <summary>
        /// 递归设置控制器引用到所有节点
        /// </summary>
        /// <param name="nodes">节点集合</param>
        /// <param name="controller">控制器实例</param>
        private void SetControllerRecursive(
            IEnumerable<TreeNodeEx> nodes,
            TreeViewExController controller
        )
        {
            if (nodes == null)
                return;

            foreach (var node in nodes)
            {
                node.Controller = controller;
                SetControllerRecursive(node.Children, controller);

                // 监听子节点集合变化，为新添加的节点设置控制器
                node.Children.CollectionChanged += (s, e) =>
                {
                    if (e.NewItems != null)
                    {
                        foreach (TreeNodeEx newChild in e.NewItems)
                        {
                            newChild.Controller = controller;
                        }
                    }
                };
            }
        }

        /// <summary>
        /// 获取树视图控制器实例
        /// </summary>
        /// <param name="treeNodeExes">树节点列表</param>
        /// <param name="treeViewPanel">树视图面板</param>
        /// <returns>树视图控制器实例</returns>
        internal static TreeViewExController GetTreeViewExProvider(
            IList<TreeNodeEx> treeNodeExes,
            TreeViewPanel treeViewPanel
        )
        {
            var provider = new TreeViewExController(treeNodeExes, treeViewPanel);
            return provider;
        }
    }
}
