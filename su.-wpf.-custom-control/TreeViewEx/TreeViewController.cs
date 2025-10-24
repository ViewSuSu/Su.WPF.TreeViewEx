using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Su.WPF.CustomControl.TreeViewEx
{
    public enum TreeViewExProviderKeyType
    {
        /// <summary>
        /// 保持树结构
        /// </summary>
        KeepTreeNodeStru,

        /// <summary>
        /// 只保留目标节点
        /// </summary>
        OnlyTargetNode,
    }

    public class TreeViewController : ObservableObject
    {
        private BindingList<TreeNodeEx> _sourceTreeNodes;

        /// <summary>
        /// 源数据，外部可修改
        /// </summary>
        public BindingList<TreeNodeEx> SourceTreeNodes
        {
            get => _sourceTreeNodes;
            set
            {
                if (_sourceTreeNodes != null)
                    _sourceTreeNodes.ListChanged -= SourceTreeNodes_ListChanged;

                _sourceTreeNodes = value;

                if (_sourceTreeNodes != null)
                    _sourceTreeNodes.ListChanged += SourceTreeNodes_ListChanged;

                RefreshDisplayList();
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// 显示数据，只读
        /// </summary>
        public ReadOnlyCollection<TreeNodeEx> ShowTreeNodeList { get; private set; }

        public TreeViewPropertyOptions Options { get; internal set; }

        private TreeNodeEx _selectedTreeNode;
        public TreeNodeEx SelectedTreeNode
        {
            get { return _selectedTreeNode; }
            set
            {
                _selectedTreeNode = value;
                OnPropertyChanged();
            }
        }

        internal TreeViewController(IList<TreeNodeEx> sourceTreeNodes, TreeViewPanel treeViewPanel)
        {
            this.SourceTreeNodes = new BindingList<TreeNodeEx>(sourceTreeNodes);
            Options = new TreeViewPropertyOptions(treeViewPanel);
            ShowTreeNodeList = new ReadOnlyCollection<TreeNodeEx>([]);

            // 绑定事件
            SourceTreeNodes.ListChanged += SourceTreeNodes_ListChanged;

            RefreshDisplayList();
        }

        internal static TreeViewController GetTreeViewExProvider(
            IList<TreeNodeEx> treeNodeExes,
            TreeViewPanel treeViewPanel
        )
        {
            var provider = new TreeViewController(treeNodeExes, treeViewPanel);
            return provider;
        }

        /// <summary>
        /// 源数据变化时自动刷新显示列表
        /// </summary>
        private void SourceTreeNodes_ListChanged(object sender, ListChangedEventArgs e)
        {
            RefreshDisplayList();
        }

        /// <summary>
        /// 刷新显示列表（应用所有过滤条件）
        /// </summary>
        private void RefreshDisplayList()
        {
            if (SourceTreeNodes == null || SourceTreeNodes.Count == 0)
            {
                UpdateShowTreeNodeList(new BindingList<TreeNodeEx>());
                return;
            }

            var filteredNodes = SourceTreeNodes;

            if (Options?.FileterOption != null)
            {
                var filterOption = Options.FileterOption;
                if (!string.IsNullOrEmpty(filterOption.NameKey))
                {
                    filteredNodes = ApplyKeywordFilter(SourceTreeNodes, filterOption);
                }
            }

            UpdateShowTreeNodeList(new BindingList<TreeNodeEx>(filteredNodes));
        }

        /// <summary>
        /// 更新显示列表
        /// </summary>
        private void UpdateShowTreeNodeList(BindingList<TreeNodeEx> newList)
        {
            ShowTreeNodeList = new ReadOnlyCollection<TreeNodeEx>(newList);
            OnPropertyChanged(nameof(ShowTreeNodeList));
        }

        /// <summary>
        /// 应用关键字过滤
        /// </summary>
        private BindingList<TreeNodeEx> ApplyKeywordFilter(
            BindingList<TreeNodeEx> nodes,
            FileterOptions filterOption
        )
        {
            switch (filterOption.Option)
            {
                case TreeViewExProviderKeyType.KeepTreeNodeStru:
                    return FilterWithFatherStructure(nodes, filterOption.NameKey);

                case TreeViewExProviderKeyType.OnlyTargetNode:
                    return FilterOnlyTargetNodes(nodes, filterOption.NameKey);

                default:
                    return nodes;
            }
        }

        /// <summary>
        /// 保持父节点结构的过滤方式
        /// </summary>
        private static BindingList<TreeNodeEx> FilterWithFatherStructure(
            IEnumerable<TreeNodeEx> nodes,
            string nameKey
        )
        {
            var result = new BindingList<TreeNodeEx>();

            foreach (var node in nodes)
            {
                var clonedNode = node.ShallowCopy();

                if (clonedNode != null)
                {
                    if (node.Children != null && node.Children.Count > 0)
                    {
                        var filteredChildren = FilterWithFatherStructure(node.Children, nameKey);
                        foreach (var child in filteredChildren)
                        {
                            clonedNode.Children.Add(child);
                            child.FatherNode = clonedNode;
                        }
                    }

                    if (
                        NodeMatchesFilter(clonedNode, nameKey)
                        || (clonedNode.Children != null && clonedNode.Children.Count > 0)
                    )
                    {
                        result.Add(clonedNode);
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// 只保留目标节点的过滤方式
        /// </summary>
        private static BindingList<TreeNodeEx> FilterOnlyTargetNodes(
            IEnumerable<TreeNodeEx> nodes,
            string nameKey
        )
        {
            var result = new BindingList<TreeNodeEx>();

            foreach (var node in nodes)
            {
                if (NodeMatchesFilter(node, nameKey))
                {
                    var matchedNode = node.ShallowCopy();
                    if (matchedNode != null)
                    {
                        matchedNode.Children = new BindingList<TreeNodeEx>();
                        result.Add(matchedNode);
                    }
                }

                if (node.Children != null && node.Children.Count > 0)
                {
                    var matchedChildren = FilterOnlyTargetNodes(node.Children, nameKey);
                    foreach (var child in matchedChildren)
                    {
                        result.Add(child);
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// 检查节点是否匹配过滤条件
        /// </summary>
        private static bool NodeMatchesFilter(TreeNodeEx node, string nameKey)
        {
            if (string.IsNullOrEmpty(nameKey))
                return true;

            return node.Name?.IndexOf(nameKey, StringComparison.OrdinalIgnoreCase) >= 0;
        }
    }
}
