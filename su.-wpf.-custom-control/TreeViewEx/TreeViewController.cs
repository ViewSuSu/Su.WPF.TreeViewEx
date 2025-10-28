using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Su.WPF.CustomControl.TreeViewEx
{
    public class TreeViewController : ObservableObject
    {
        public List<TreeNodeEx> SelectedNodes { get; set; } = [];

        /// <summary>
        /// 源数据，外部可修改
        /// </summary>
        public ObservableCollection<TreeNodeEx> SourceTreeNodes { get; set; }

        public TreeViewPropertyOptions Options { get; internal set; }

        internal TreeViewController(IList<TreeNodeEx> sourceTreeNodes, TreeViewPanel treeViewPanel)
        {
            this.SourceTreeNodes = new ObservableCollection<TreeNodeEx>(sourceTreeNodes);
            Options = new TreeViewPropertyOptions(treeViewPanel);

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
        /// 删除节点（支持递归）
        /// </summary>
        /// <param name="nodes">要删除的节点列表</param>
        public void DeleteNodes(IEnumerable<TreeNodeEx> nodes)
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
        /// 递归设置控制器引用
        /// </summary>
        private void SetControllerRecursive(
            IEnumerable<TreeNodeEx> nodes,
            TreeViewController controller
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

        internal static TreeViewController GetTreeViewExProvider(
            IList<TreeNodeEx> treeNodeExes,
            TreeViewPanel treeViewPanel
        )
        {
            var provider = new TreeViewController(treeNodeExes, treeViewPanel);
            return provider;
        }
    }
}
