using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using Su.WPF.CustomControl.Menu;
using Brush = System.Windows.Media.Brush;
using Color = System.Windows.Media.Color;

namespace Su.WPF.CustomControl.TreeViewEx
{
    public class TreeNodeExIconOptions : ObservableObject
    {
        private ImageSource _icon;
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
        public double Width
        {
            get { return _width; }
            set
            {
                _width = value;
                OnPropertyChanged();
            }
        }

        public TreeNodeExIconOptions(ImageSource icon)
        {
            this.Icon = icon;
        }

        public bool IsShowImageSource => Icon != null && Width > 0;
    }

    public class TreeNodeExTextOptions : ObservableObject
    {
        private string _text;
        public string Text
        {
            get { return _text; }
            set
            {
                _text = value;
                OnPropertyChanged();
            }
        }

        public double FontSize { get; set; }
        public FontWeight FontWeight { get; set; }
    }

    public class TreeNodeEx : ObservableObject
    {
        private TreeNodeEx _fatherNode;
        private string _name;
        private bool isSelected;
        private bool _isEnabled = true;
        private bool _isShowCheckBox;
        private bool _isExpanded;
        public TreeNodeEx FatherNode
        {
            get { return _fatherNode; }
            internal set
            {
                _fatherNode = value;
                OnPropertyChanged(nameof(FatherNode));
            }
        }
        public string Text
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged(nameof(Text));
            }
        }
        public TreeNodeExTextOptions TreeNodeExTextOptions { get; set; } =
            new TreeNodeExTextOptions();

        private object _tooltip;

        public object Tooltip
        {
            get { return _tooltip; }
            set
            {
                _tooltip = value;
                OnPropertyChanged();
            }
        }

        public object Data { get; set; }

        internal const string IsSelectedPropertyName = nameof(IsSelected);

        public bool IsSelected
        {
            get { return isSelected; }
            set
            {
                isSelected = value;
                OnPropertyChanged();
            }
        }

        public bool IsExpanded
        {
            get { return _isExpanded; }
            set
            {
                _isExpanded = value;
                OnPropertyChanged();
            }
        }

        public bool IsEnabled
        {
            get { return _isEnabled; }
            set
            {
                _isEnabled = value;
                OnPropertyChanged();
            }
        }

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

        public Brush HighlightColorBrush => new SolidColorBrush(HighlightColor);

        public Color HighlightColor { get; set; } = Color.FromRgb(0, 191, 255);

        internal void ForceSetIsChecked(bool value)
        {
            _isChecked = value;
            OnPropertyChanged(nameof(IsChecked));

            foreach (var child in Children)
            {
                child.ForceSetIsChecked(value);
            }
        }

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

        public bool IsRightButtonWillShowMenu => MenuItems.Count != 0;
        public ObservableCollection<TreeNodeMenu> MenuItems { get; private set; }

        public ReadOnlyCollection<System.Windows.Controls.MenuItem> ShowMenuItemModels =>
            new([.. MenuItems.Select(x => x.MenuItem)]);

        public ObservableCollection<TreeNodeEx> Children { get; internal set; } = [];

        public TreeNodeExIconOptions TreeNodeExIconOptions { get; set; }

        private TreeNodeEx(string text)
        {
            this.Text = text;
            MenuItems = [];
            MenuItems.CollectionChanged += (o, e) =>
            {
                OnPropertyChanged(nameof(IsRightButtonWillShowMenu));
            };
            TreeNodeExTextOptions = new TreeNodeExTextOptions() { Text = text };

            this.Children.CollectionChanged += (o, e) =>
            {
                if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
                {
                    var nodes = (o as ObservableCollection<TreeNodeEx>);
                    foreach (var item in nodes)
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

        public static TreeNodeEx CreateNode(string text)
        {
            var node = new TreeNodeEx(text);
            return node;
        }

        public void Delete()
        {
            Controller?.DeleteNodes([this]);
        }

        public void AddChild(TreeNodeEx node)
        {
            Children.Add(node);
        }

        public void AddRange(IEnumerable<TreeNodeEx> nodes)
        {
            foreach (var item in nodes)
            {
                Children.Add(item);
            }
        }

        public void DeleteChildByPredicate(Func<TreeNodeEx, bool> predicate)
        {
            foreach (var node in Children)
            {
                if (predicate?.Invoke(node) == true)
                {
                    Controller?.DeleteNodes([node]);
                }
            }
        }

        public void DeleteFirstChildByPredicate(Func<TreeNodeEx, bool> predicate)
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

        private TreeNodeEx() { }

        internal TreeViewController Controller { get; set; }

        public static RelayCommand MouseRightButtonDownCommand { get; } =
            new RelayCommand(treeviewItem =>
            {
                if (treeviewItem is TreeViewItem item)
                {
                    item.Focus();
                }
            });
    }
}
