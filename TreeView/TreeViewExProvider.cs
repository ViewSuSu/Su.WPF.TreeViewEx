using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace Su.WPF.CustomControl.TreeView
{
    public enum TreeViewExProviderKeyType
    {
        /// <summary>
        /// 保持父节点结构
        /// </summary>
        KeepFather,

        /// <summary>
        /// 只保留目标节点
        /// </summary>
        OnlyTargetNode,
    }

    public class TreeViewExProviderOption
    {
        /// <summary>
        /// 过滤选项
        /// </summary>
        public FileterOption FileterOption { get; internal set; } = null;
    }

    public class FileterOption
    {
        /// <summary>
        /// 名字关键字
        /// </summary>
        public string NameKey { get; set; }

        /// <summary>
        /// 保留树结构选项
        /// </summary>
        public TreeViewExProviderKeyType Option { get; internal set; } =
            TreeViewExProviderKeyType.KeepFather;

        /// <summary>
        /// 是否是深拷贝
        /// </summary>
        public bool IsDeepCopy { get; set; } = false;
    }

    public class TreeViewExProvider
    {
        public ContentControl TreeView { get; internal set; }
        public IEnumerable<TreeNodeEx> TreeNodeExes { get; internal set; }

        internal TreeViewExProvider() { }

        public static TreeViewExProvider GetTreeViewExProvider(
            IEnumerable<TreeNodeEx> treeNodeExes,
            TreeViewExProviderOption treeViewExProviderOption = null
        )
        {
            var filteredNodes = treeNodeExes;

            // 应用过滤选项
            if (treeViewExProviderOption?.FileterOption != null)
            {
                var filterOption = treeViewExProviderOption.FileterOption;

                if (!string.IsNullOrEmpty(filterOption.NameKey))
                {
                    filteredNodes = FilterTreeNodes(treeNodeExes, filterOption);
                }
            }

            var provider = new TreeViewExProvider() { TreeNodeExes = filteredNodes };
            return provider;
        }

        /// <summary>
        /// 过滤树节点
        /// </summary>
        private static IEnumerable<TreeNodeEx> FilterTreeNodes(
            IEnumerable<TreeNodeEx> nodes,
            FileterOption filterOption
        )
        {
            switch (filterOption.Option)
            {
                case TreeViewExProviderKeyType.KeepFather:
                    return FilterWithFatherStructure(
                        nodes,
                        filterOption.NameKey,
                        filterOption.IsDeepCopy
                    );

                case TreeViewExProviderKeyType.OnlyTargetNode:
                    return FilterOnlyTargetNodes(
                        nodes,
                        filterOption.NameKey,
                        filterOption.IsDeepCopy
                    );

                default:
                    return nodes;
            }
        }

        /// <summary>
        /// 保持父节点结构的过滤方式
        /// </summary>
        private static IEnumerable<TreeNodeEx> FilterWithFatherStructure(
            IEnumerable<TreeNodeEx> nodes,
            string nameKey,
            bool isDeepCopy
        )
        {
            var result = new BindingList<TreeNodeEx>();

            foreach (var node in nodes)
            {
                // 克隆节点
                var clonedNode = CloneTreeNode(node, isDeepCopy);

                if (clonedNode != null)
                {
                    // 过滤子节点
                    if (clonedNode.Childrens != null && clonedNode.Childrens.Count > 0)
                    {
                        var filteredChildren = FilterWithFatherStructure(
                            clonedNode.Childrens,
                            nameKey,
                            isDeepCopy
                        );

                        // 清空并重新添加过滤后的子节点
                        clonedNode.Childrens = [];
                        foreach (var child in filteredChildren)
                        {
                            clonedNode.Childrens.Add(child);
                            child.FatherNode = clonedNode; // 维护父子关系
                        }
                    }

                    // 如果节点本身匹配关键字，或者有匹配的子节点，则保留该节点
                    if (
                        NodeMatchesFilter(clonedNode, nameKey)
                        || (clonedNode.Childrens != null && clonedNode.Childrens.Count > 0)
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
        private static IEnumerable<TreeNodeEx> FilterOnlyTargetNodes(
            IEnumerable<TreeNodeEx> nodes,
            string nameKey,
            bool isDeepCopy
        )
        {
            var result = new BindingList<TreeNodeEx>();

            foreach (var node in nodes)
            {
                // 检查当前节点是否匹配
                if (NodeMatchesFilter(node, nameKey))
                {
                    // 只添加匹配的节点，不包含其子节点结构
                    var matchedNode = CloneTreeNode(node, isDeepCopy);
                    if (matchedNode != null)
                    {
                        matchedNode.Childrens = new BindingList<TreeNodeEx>(); // 清空子节点
                        result.Add(matchedNode);
                    }
                }

                // 递归检查子节点
                if (node.Childrens != null && node.Childrens.Count > 0)
                {
                    var matchedChildren = FilterOnlyTargetNodes(
                        node.Childrens.ToList(),
                        nameKey,
                        isDeepCopy
                    );
                    foreach (var child in matchedChildren)
                    {
                        result.Add(child);
                    }
                }
            }

            return result.ToList();
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

        /// <summary>
        /// 克隆树节点
        /// </summary>
        private static TreeNodeEx CloneTreeNode(TreeNodeEx source, bool isDeepCopy)
        {
            return source.Copy(isDeepCopy, isDeepCopy);
        }

        /// <summary>
        /// 重建父子关系
        /// </summary>
        private static void RebuildFatherRelationships(IEnumerable<TreeNodeEx> nodes)
        {
            foreach (var node in nodes)
            {
                if (node.Childrens != null && node.Childrens.Count > 0)
                {
                    foreach (var child in node.Childrens)
                    {
                        child.FatherNode = node;
                    }
                    RebuildFatherRelationships(node.Childrens.ToList());
                }
            }
        }
    }
}
