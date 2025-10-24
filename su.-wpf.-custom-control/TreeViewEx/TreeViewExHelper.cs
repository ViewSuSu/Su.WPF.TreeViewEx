using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using TreeView = System.Windows.Controls.TreeView;

namespace Su.WPF.CustomControl.TreeViewEx
{
    internal class TreeViewExHelper
    {
        static TreeViewExHelper()
        {
            EventManager.RegisterClassHandler(
                typeof(TreeView),
                TreeView.SelectedItemChangedEvent,
                new RoutedEventHandler(TreeView_SelectedItemChanged)
            );
        }

        private static void TreeView_SelectedItemChanged(object sender, RoutedEventArgs e)
        {
            e.Handled = true;
            var treeView = sender as TreeView;
            if (treeView == null)
                return;

            var ee = e as RoutedPropertyChangedEventArgs<object>;
            if (ee == null)
                return;

            SetSelectedItem(treeView, ee.NewValue);
        }

        private static Dictionary<DependencyObject, TreeViewSelectedItemBehavior> behaviors =
            new Dictionary<DependencyObject, TreeViewSelectedItemBehavior>();

        public static object GetSelectedItem(DependencyObject obj)
        {
            return (object)obj.GetValue(SelectedItemProperty);
        }

        public static void SetSelectedItem(DependencyObject obj, object value)
        {
            obj.SetValue(SelectedItemProperty, value);
        }

        // Using a DependencyProperty as the backing store for SelectedItem.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedItemProperty =
            DependencyProperty.RegisterAttached(
                "SelectedItem",
                typeof(object),
                typeof(TreeViewExHelper),
                new UIPropertyMetadata(null, SelectedItemChanged)
            );

        private static void SelectedItemChanged(
            DependencyObject obj,
            DependencyPropertyChangedEventArgs e
        )
        {
            if (!(obj is TreeView))
                return;

            if (!behaviors.ContainsKey(obj))
                behaviors.Add(obj, new TreeViewSelectedItemBehavior(obj as TreeView));

            TreeViewSelectedItemBehavior view = behaviors[obj];
            view.ChangeSelectedItem(e.NewValue);
        }

        private class TreeViewSelectedItemBehavior
        {
            private TreeView view;

            public TreeViewSelectedItemBehavior(TreeView view)
            {
                this.view = view;
                view.SelectedItemChanged += (sender, e) => SetSelectedItem(view, e.NewValue);
            }

            internal void ChangeSelectedItem(object p)
            {
                var item = FindItemByDataContext(view, p);
                if (item != null)
                {
                    item.IsSelected = true;
                }
            }

            private TreeViewItem FindItemByDataContext(TreeView treeView, object dataContext)
            {
                for (int i = 0; i < treeView.Items.Count; i++)
                {
                    var treeItem =
                        treeView.ItemContainerGenerator.ContainerFromIndex(i) as TreeViewItem;
                    if (treeItem == null)
                        continue;

                    var result = FindItemByDataContext(treeItem, dataContext);
                    if (result != null)
                    {
                        return result;
                    }
                }
                return null;
            }

            private TreeViewItem FindItemByDataContext(TreeViewItem item, object dataContext)
            {
                if (item.DataContext == dataContext)
                {
                    return item;
                }

                for (int i = 0; i < item.Items.Count; i++)
                {
                    var subItem = item.ItemContainerGenerator.ContainerFromIndex(i) as TreeViewItem;
                    if (subItem == null)
                        continue;

                    var result = FindItemByDataContext(subItem, dataContext);
                    if (result != null)
                    {
                        return result;
                    }
                }
                return null;
            }
        }
    }
}
