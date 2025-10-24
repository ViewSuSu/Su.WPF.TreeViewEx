using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using Su.WPF.CustomControl.Menu;

namespace Su.WPF.CustomControl.TreeViewEx
{
    public enum ImageSourceLocation
    {
        Left,
        Middle,
        Right,
    }

    public class TreeNodeEx : ObservableObject
    {
        private TreeNodeEx _fatherNode;
        private string _name;
        private ImageSource _imageSource;
        private ImageSourceLocation _imageSourceLocation;
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
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        public object Data { get; set; }

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

        public bool IsRightButtonWillShowMenu => MenuItems.Count != 0;
        public BindingList<MenuItemModel> MenuItems { get; private set; }

        public ReadOnlyCollection<System.Windows.Controls.MenuItem> ShowMenuItemModels =>
            new([.. MenuItems.Select(x => x.MenuItem)]);

        public BindingList<TreeNodeEx> Children { get; internal set; } = [];

        public ImageSource icon
        {
            get { return _imageSource; }
            set
            {
                _imageSource = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsShowImageSource));
            }
        }

        public ImageSourceLocation ImageSourceLocation
        {
            get { return _imageSourceLocation; }
            set
            {
                _imageSourceLocation = value;
                OnPropertyChanged();
            }
        }

        public bool IsShowImageSource => icon != null;

        public TreeNodeEx(string name)
        {
            this.Name = name;
            MenuItems = [];
            MenuItems.ListChanged += (o, e) =>
            {
                OnPropertyChanged(nameof(IsRightButtonWillShowMenu));
            };
        }

        /// <summary>
        /// 浅拷贝当前节点（不拷贝子节点）
        /// </summary>
        public TreeNodeEx ShallowCopy()
        {
            return new TreeNodeEx(Name)
            {
                Data = this.Data, // 浅拷贝Data引用
                FatherNode = this.FatherNode,
                Children =
                    new BindingList<TreeNodeEx>() // 新的空列表
                ,
            };
        }

        /// <summary>
        /// 拷贝当前节点
        /// </summary>
        /// <param name="copyChildren">是否拷贝子节点</param>
        public TreeNodeEx Copy(bool copyChildren = false)
        {
            if (!copyChildren)
            {
                return ShallowCopy();
            }

            // 拷贝当前节点和所有子节点
            var copiedNode = ShallowCopy();

            // 递归拷贝所有子节点
            foreach (var child in this.Children)
            {
                var copiedChild = child.Copy(true); // 递归拷贝子节点
                copiedChild.FatherNode = copiedNode;
                copiedNode.Children.Add(copiedChild);
            }

            return copiedNode;
        }

        public RelayCommand MouseRightButtonDownCommand { get; set; } =
            new RelayCommand(treeviewItem =>
            {
                if (treeviewItem is TreeViewItem item)
                {
                    item.Focus();
                }
            });
    }
}
