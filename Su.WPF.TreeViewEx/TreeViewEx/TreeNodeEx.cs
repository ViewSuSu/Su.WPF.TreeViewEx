using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Media;
using Su.WPF.CustomControl.Menu;
using Brush = System.Windows.Media.Brush;
using Color = System.Windows.Media.Color;

namespace Su.WPF.CustomControl.TreeViewEx
{
    /// <summary>
    /// 树节点图标选项配置类
    /// </summary>
    public class TreeNodeExIconOptions : ObservableObjectBase
    {
        private ImageSource _icon;

        /// <summary>
        /// 图标图像源
        /// </summary>
        public ImageSource Icon
        {
            get { return _icon; }
            set
            {
                _icon = value;
                OnPropertyChanged();
            }
        }

        private double _width = 16;

        /// <summary>
        /// 图标宽度
        /// </summary>
        public double Width
        {
            get { return _width; }
            set
            {
                _width = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// 是否显示图像源（图标不为空且宽度大于0）
        /// </summary>
        public bool IsShowImageSource => Icon != null && Width > 0;
    }

    /// <summary>
    /// 树节点文本选项配置类
    /// </summary>
    public class TreeNodeExTextOptions : ObservableObjectBase
    {
        /// <summary>
        /// 字体大小
        /// </summary>
        public double FontSize { get; set; } = System.Windows.SystemFonts.MessageFontSize;

        /// <summary>
        /// 字体粗细
        /// </summary>
        public FontWeight FontWeight { get; set; } = System.Windows.FontWeights.Normal;

        internal TreeNodeExTextOptions() { }
    }

    /// <summary>
    /// 扩展树节点类
    /// </summary>
    public class TreeNodeEx : ObservableObjectBase
    {
        private TreeNodeEx _fatherNode;
        private string _text;
        private bool isSelected;
        private bool _isEnabled = true;
        private bool _isShowCheckBox;
        private bool _isExpanded;

        /// <summary>
        /// 父节点
        /// </summary>
        public TreeNodeEx FatherNode
        {
            get { return _fatherNode; }
            internal set
            {
                _fatherNode = value;
                OnPropertyChanged(nameof(FatherNode));
            }
        }

        /// <summary>
        /// 节点显示文本
        /// </summary>
        public string Text
        {
            get { return _text; }
            set
            {
                _text = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// 文本选项配置
        /// </summary>
        public TreeNodeExTextOptions TreeNodeExTextOptions { get; } = new TreeNodeExTextOptions();

        private object _tooltip;

        /// <summary>
        /// 工具提示内容
        /// </summary>
        public object Tooltip
        {
            get { return _tooltip; }
            set
            {
                _tooltip = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// 节点关联的数据对象
        /// </summary>
        public object Data { get; set; }

        internal const string IsSelectedPropertyName = nameof(IsSelected);

        /// <summary>
        /// 是否被选中
        /// </summary>
        public bool IsSelected
        {
            get { return isSelected; }
            set
            {
                isSelected = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// 是否展开
        /// </summary>
        public bool IsExpanded
        {
            get { return _isExpanded; }
            set
            {
                _isExpanded = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsEnabled
        {
            get { return _isEnabled; }
            set
            {
                _isEnabled = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// 是否显示复选框
        /// </summary>
        public bool IsShowCheckBox
        {
            get { return _isShowCheckBox; }
            set
            {
                _isShowCheckBox = value;
                OnPropertyChanged();
            }
        }

        private bool? _isChecked = false;

        /// <summary>
        /// 复选框状态（true:选中, false:未选中, null:部分选中）
        /// </summary>
        public bool? IsChecked
        {
            get => _isChecked;
            set
            {
                if (_isChecked == value)
                    return;

                _isChecked = value;
                OnPropertyChanged();

                // 向下递归影响子节点
                if (_isChecked.HasValue)
                {
                    foreach (var child in Children)
                    {
                        child.ForceSetIsChecked(_isChecked.Value);
                    }
                }

                // 向上通知父节点判断状态
                UpdateParentCheckState();
            }
        }

        /// <summary>
        /// 高亮颜色画刷
        /// </summary>
        public Brush HighlightColorBrush => new SolidColorBrush(HighlightColor);

        /// <summary>
        /// 高亮颜色
        /// </summary>
        public Color HighlightColor { get; set; } = Color.FromRgb(0, 191, 255);

        /// <summary>
        /// 强制设置复选框状态（不触发向上通知）
        /// </summary>
        /// <param name="value">复选框状态值</param>
        internal void ForceSetIsChecked(bool value)
        {
            _isChecked = value;
            OnPropertyChanged(nameof(IsChecked));

            foreach (var child in Children)
            {
                child.ForceSetIsChecked(value);
            }
        }

        /// <summary>
        /// 更新父节点复选框状态
        /// </summary>
        private void UpdateParentCheckState()
        {
            if (FatherNode == null)
                return;

            bool allChecked = FatherNode.Children.All(x => x.IsChecked == true);
            bool allUnchecked = FatherNode.Children.All(x => x.IsChecked == false);

            FatherNode._isChecked =
                allChecked ? true
                : allUnchecked ? false
                : (bool?)null;

            FatherNode.OnPropertyChanged(nameof(IsChecked));

            // 向上递归检查
            FatherNode.UpdateParentCheckState();
        }

        /// <summary>
        /// 是否显示右键菜单（当有菜单项时显示）
        /// </summary>
        public bool IsRightButtonWillShowMenu => MenuItemModels.Count != 0;

        /// <summary>
        /// 右键菜单项集合
        /// </summary>
        public ObservableCollection<TreeNodeMenu> MenuItemModels { get; } = [];

        /// <summary>
        /// 显示的菜单项模型（只读）
        /// </summary>
        public ReadOnlyCollection<System.Windows.Controls.MenuItem> MenuItems =>
            new([.. MenuItemModels.Select(x => x.MenuItem)]);

        /// <summary>
        /// 子节点集合
        /// </summary>
        public ObservableCollection<TreeNodeEx> Children { get; internal set; } = [];

        /// <summary>
        /// 图标选项配置
        /// </summary>
        public TreeNodeExIconOptions TreeNodeExIconOptions { get; } = new TreeNodeExIconOptions();

        /// <summary>
        /// 私有构造函数
        /// </summary>
        /// <param name="text">节点文本</param>
        private TreeNodeEx(string text)
        {
            this.Text = text;
            MenuItemModels.CollectionChanged += (o, e) =>
            {
                OnPropertyChanged(nameof(IsRightButtonWillShowMenu));
                OnPropertyChanged(nameof(MenuItems));
            };
            this.Children.CollectionChanged += (o, e) =>
            {
                if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
                {
                    foreach (TreeNodeEx item in e.NewItems)
                    {
                        item.FatherNode = this;
                        // 继承控制器的引用
                        if (this.Controller != null)
                        {
                            item.Controller = this.Controller;
                        }
                    }
                }
            };
        }

        /// <summary>
        /// 创建树节点
        /// </summary>
        /// <param name="text">节点文本</param>
        /// <returns>创建的树节点</returns>
        public static TreeNodeEx CreateNode(string text)
        {
            var node = new TreeNodeEx(text);
            return node;
        }

        /// <summary>
        /// 删除当前节点
        /// </summary>
        public void Delete()
        {
            Controller?.DeleteNodes([this]);
        }

        /// <summary>
        /// 复制当前节点及其所有子节点（递归）
        /// </summary>
        /// <returns>复制的新节点</returns>
        public TreeNodeEx Copy()
        {
            return CopyRecursive(this);
        }

        /// <summary>
        /// 复制当前节点及其所有子节点到指定父节点
        /// </summary>
        /// <param name="targetParent">目标父节点</param>
        /// <returns>复制的新节点</returns>
        public TreeNodeEx CopyTo(TreeNodeEx targetParent)
        {
            var copiedNode = Copy();
            targetParent.AddChild(copiedNode);
            return copiedNode;
        }

        /// <summary>
        /// 递归复制节点及其所有子节点
        /// </summary>
        /// <param name="sourceNode">源节点</param>
        /// <returns>复制的新节点</returns>
        private TreeNodeEx CopyRecursive(TreeNodeEx sourceNode)
        {
            // 创建新节点并复制基本属性
            var newNode = CreateNode($"{sourceNode.Text} - 副本");

            // 复制图标属性
            newNode.TreeNodeExIconOptions.Icon = sourceNode.TreeNodeExIconOptions.Icon;
            newNode.TreeNodeExIconOptions.Width = sourceNode.TreeNodeExIconOptions.Width;

            // 复制文本样式
            newNode.TreeNodeExTextOptions.FontSize = sourceNode.TreeNodeExTextOptions.FontSize;
            newNode.TreeNodeExTextOptions.FontWeight = sourceNode.TreeNodeExTextOptions.FontWeight;

            // 复制其他属性
            newNode.Tooltip = sourceNode.Tooltip;
            newNode.Data = sourceNode.Data; // 注意：这里是浅拷贝，如果需要深拷贝需要额外处理
            newNode.IsEnabled = sourceNode.IsEnabled;
            newNode.IsShowCheckBox = sourceNode.IsShowCheckBox;
            newNode.IsExpanded = sourceNode.IsExpanded;
            newNode.HighlightColor = sourceNode.HighlightColor;

            // 复制复选框状态
            newNode._isChecked = sourceNode._isChecked;

            // 递归复制所有子节点
            foreach (var child in sourceNode.Children)
            {
                var copiedChild = CopyRecursive(child);
                newNode.AddChild(copiedChild);
            }

            // 复制菜单项（注意：这里复制的是菜单定义，不是菜单实例）
            foreach (var menu in sourceNode.MenuItemModels)
            {
                // 由于TreeNodeMenu包含Action委托，需要创建新的实例
                // 这里可以根据需要实现菜单的深度复制
                newNode.MenuItemModels.Add(menu);
            }

            return newNode;
        }

        /// <summary>
        /// 获取当前节点所有子节点中显示复选框且被选中的节点列表
        /// </summary>
        /// <returns>选中的子节点列表</returns>
        public List<TreeNodeEx> GetCheckedChildren()
        {
            var checkedNodes = new List<TreeNodeEx>();
            CollectCheckedChildrenRecursive(this, checkedNodes);
            return checkedNodes;
        }

        /// <summary>
        /// 递归收集显示复选框且被选中的子节点
        /// </summary>
        /// <param name="node">当前节点</param>
        /// <param name="checkedNodes">选中的节点列表</param>
        private void CollectCheckedChildrenRecursive(TreeNodeEx node, List<TreeNodeEx> checkedNodes)
        {
            foreach (var child in node.Children)
            {
                // 如果子节点显示复选框且被选中，添加到列表
                if (child.IsShowCheckBox && child.IsChecked == true)
                {
                    checkedNodes.Add(child);
                }

                // 递归检查子节点的子节点
                CollectCheckedChildrenRecursive(child, checkedNodes);
            }
        }

        /// <summary>
        /// 获取当前节点所有子孙节点中显示复选框且被选中的节点列表（包括间接子节点）
        /// </summary>
        /// <returns>选中的子孙节点列表</returns>
        public List<TreeNodeEx> GetAllCheckedDescendants()
        {
            var checkedNodes = new List<TreeNodeEx>();
            CollectAllCheckedDescendantsRecursive(this, checkedNodes);
            return checkedNodes;
        }

        /// <summary>
        /// 递归收集所有显示复选框且被选中的子孙节点
        /// </summary>
        /// <param name="node">当前节点</param>
        /// <param name="checkedNodes">选中的节点列表</param>
        private void CollectAllCheckedDescendantsRecursive(
            TreeNodeEx node,
            List<TreeNodeEx> checkedNodes
        )
        {
            // 跳过当前节点本身，只检查子孙节点
            foreach (var child in node.Children)
            {
                // 如果子节点显示复选框且被选中，添加到列表
                if (child.IsShowCheckBox && child.IsChecked == true)
                {
                    checkedNodes.Add(child);
                }

                // 递归检查所有子孙节点
                CollectAllCheckedDescendantsRecursive(child, checkedNodes);
            }
        }

        /// <summary>
        /// 获取当前节点所有子节点中显示复选框的节点数量
        /// </summary>
        /// <returns>显示复选框的子节点数量</returns>
        public int GetCheckBoxChildrenCount()
        {
            return CountCheckBoxChildrenRecursive(this);
        }

        /// <summary>
        /// 递归统计显示复选框的子节点数量
        /// </summary>
        /// <param name="node">当前节点</param>
        /// <returns>显示复选框的子节点数量</returns>
        private int CountCheckBoxChildrenRecursive(TreeNodeEx node)
        {
            int count = 0;
            foreach (var child in node.Children)
            {
                if (child.IsShowCheckBox)
                {
                    count++;
                }
                count += CountCheckBoxChildrenRecursive(child);
            }
            return count;
        }

        /// <summary>
        /// 获取当前节点所有子节点中显示复选框且被选中的节点数量
        /// </summary>
        /// <returns>选中的子节点数量</returns>
        public int GetCheckedChildrenCount()
        {
            return CountCheckedChildrenRecursive(this);
        }

        /// <summary>
        /// 递归统计显示复选框且被选中的子节点数量
        /// </summary>
        /// <param name="node">当前节点</param>
        /// <returns>选中的子节点数量</returns>
        private int CountCheckedChildrenRecursive(TreeNodeEx node)
        {
            int count = 0;
            foreach (var child in node.Children)
            {
                if (child.IsShowCheckBox && child.IsChecked == true)
                {
                    count++;
                }
                count += CountCheckedChildrenRecursive(child);
            }
            return count;
        }

        /// <summary>
        /// 检查当前节点是否有任何子节点显示复选框且被选中
        /// </summary>
        /// <returns>如果存在选中的子节点返回true，否则返回false</returns>
        public bool HasCheckedChildren()
        {
            return CheckHasCheckedChildrenRecursive(this);
        }

        /// <summary>
        /// 递归检查是否有显示复选框且被选中的子节点
        /// </summary>
        /// <param name="node">当前节点</param>
        /// <returns>如果存在选中的子节点返回true，否则返回false</returns>
        private bool CheckHasCheckedChildrenRecursive(TreeNodeEx node)
        {
            foreach (var child in node.Children)
            {
                if (child.IsShowCheckBox && child.IsChecked == true)
                {
                    return true;
                }
                if (CheckHasCheckedChildrenRecursive(child))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 添加子节点
        /// </summary>
        /// <param name="node">要添加的子节点</param>
        public void AddChild(TreeNodeEx node)
        {
            Children.Add(node);
        }

        /// <summary>
        /// 添加子节点
        /// </summary>
        /// <param name="text">子节点文本</param>
        public void AddChild(string text)
        {
            var node = CreateNode(text);
            Children.Add(node);
        }

        /// <summary>
        /// 添加多个子节点
        /// </summary>
        /// <param name="nodes">子节点集合</param>
        public void AddRange(IEnumerable<TreeNodeEx> nodes)
        {
            foreach (var item in nodes)
            {
                Children.Add(item);
            }
        }

        /// <summary>
        /// 根据条件删除子节点
        /// </summary>
        /// <param name="predicate">删除条件</param>
        public void DeleteChildrenNodesByPredicate(Func<TreeNodeEx, bool> predicate)
        {
            foreach (var node in Children)
            {
                if (predicate?.Invoke(node) == true)
                {
                    Controller?.DeleteNodes([node]);
                }
            }
        }

        /// <summary>
        /// 根据条件删除第一个匹配的子节点
        /// </summary>
        /// <param name="predicate">删除条件</param>
        public void DeleteFirstChildNodeByPredicate(Func<TreeNodeEx, bool> predicate)
        {
            for (int i = 0; i < Children.Count; i++)
            {
                if (predicate?.Invoke(Children[i]) == true)
                {
                    Children.RemoveAt(i);
                    break;
                }
            }
        }

        /// <summary>
        /// 私有默认构造函数
        /// </summary>
        private TreeNodeEx() { }

        /// <summary>
        /// 树视图控制器
        /// </summary>
        internal TreeViewExController Controller { get; set; }
    }
}
