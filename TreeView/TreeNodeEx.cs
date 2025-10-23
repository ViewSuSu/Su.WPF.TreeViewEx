using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Su.WPF.CustomControl.TreeView
{
    public class TreeNodeEx : INotifyPropertyChanged
    {
        private TreeNodeEx _fatherNode;
        private string _name;
        public TreeNodeEx FatherNode
        {
            get { return _fatherNode; }
            set
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

        public BindingList<TreeNodeEx> Childrens { get; set; } = [];

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// 浅拷贝当前节点（不拷贝子节点）
        /// </summary>
        public TreeNodeEx ShallowCopy()
        {
            return new TreeNodeEx
            {
                Name = this.Name,
                Data = this.Data, // 浅拷贝Data引用
                FatherNode = this.FatherNode,
                Childrens =
                    new BindingList<TreeNodeEx>() // 新的空列表
                ,
            };
        }

        /// <summary>
        /// 深拷贝整个子树
        /// </summary>
        public TreeNodeEx DeepCopy()
        {
            var copiedNode = new TreeNodeEx
            {
                Name = this.Name,
                Data = DeepCopyData(this.Data), // 深拷贝Data
                FatherNode = null, // 父节点在构建树结构时设置
                Childrens = new BindingList<TreeNodeEx>(),
            };

            // 递归深拷贝所有子节点
            foreach (var child in this.Childrens)
            {
                var copiedChild = child.DeepCopy();
                copiedChild.FatherNode = copiedNode;
                copiedNode.Childrens.Add(copiedChild);
            }

            return copiedNode;
        }

        /// <summary>
        /// 深拷贝Data对象
        /// </summary>
        private object DeepCopyData(object data)
        {
            if (data == null)
                return null;

            // 如果对象实现了ICloneable接口，使用Clone方法
            if (data is ICloneable cloneable)
            {
                return cloneable.Clone();
            }

            // 对于简单类型和字符串，直接返回
            if (data.GetType().IsValueType || data is string)
            {
                return data;
            }

            // 尝试使用序列化进行深拷贝
            try
            {
                using var ms = new System.IO.MemoryStream();
                var formatter =
                    new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                formatter.Serialize(ms, data);
                ms.Seek(0, System.IO.SeekOrigin.Begin);
                return formatter.Deserialize(ms);
            }
            catch
            {
                // 如果序列化失败，返回原始对象引用（浅拷贝）
                return data;
            }
        }

        /// <summary>
        /// 拷贝当前节点（根据参数决定深浅拷贝）
        /// </summary>
        /// <param name="deepCopy">true:深拷贝, false:浅拷贝</param>
        /// <param name="copyChildren">是否拷贝子节点（仅深拷贝有效）</param>
        public TreeNodeEx Copy(bool deepCopy = false, bool copyChildren = true)
        {
            return deepCopy
                ? (copyChildren ? DeepCopy() : DeepCopyWithoutChildren())
                : ShallowCopy();
        }

        /// <summary>
        /// 深拷贝当前节点但不拷贝子节点
        /// </summary>
        private TreeNodeEx DeepCopyWithoutChildren()
        {
            return new TreeNodeEx
            {
                Name = this.Name,
                Data = DeepCopyData(this.Data), // 深拷贝Data
                FatherNode = this.FatherNode,
                Childrens =
                    new BindingList<TreeNodeEx>() // 新的空列表
                ,
            };
        }
    }
}
