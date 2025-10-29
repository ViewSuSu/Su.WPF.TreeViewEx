using System.Windows;
using System.Windows.Media;

namespace Su.WPF.CustomControl
{
    /// <summary>
    /// VisualTreeHelperUtils
    /// </summary>
    internal static class VisualTreeHelperUtils
    {
        /// <summary>
        /// FindParent
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="child"></param>
        /// <returns></returns>
        public static T FindParent<T>(this DependencyObject child)
            where T : DependencyObject
        {
            if (child is null)
                return null;
            var parentElement = VisualTreeHelper.GetParent(child);

            if (parentElement is T parent)
                return parent;

            return FindParent<T>(parentElement);
        }

        /// <summary>
        /// FindParent
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="child"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static T FindParent<T>(this DependencyObject child, string name)
            where T : FrameworkElement
        {
            if (child is null)
                return null;
            var parentElement = VisualTreeHelper.GetParent(child) as FrameworkElement;

            if (parentElement is T parent)
                if (parent.Name == name)
                    return parent;

            return FindParent<T>(parentElement, name);
        }

        /// <summary>
        /// FindChild
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parent"></param>
        /// <returns></returns>
        public static T FindChild<T>(this DependencyObject parent)
            where T : DependencyObject
        {
            if (parent is null)
                return null;
            for (var i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                var childElement = VisualTreeHelper.GetChild(parent, i);
                if (childElement is T child)
                    return child;

                if (FindChild<T>(childElement) is T result)
                    return result;
            }
            return null;
        }

        /// <summary>
        /// FindChild
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parent"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static T FindChild<T>(this DependencyObject parent, string name)
            where T : FrameworkElement
        {
            if (parent is null)
                return null;
            for (var i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                var childElement = VisualTreeHelper.GetChild(parent, i) as FrameworkElement;
                if (childElement is T child)
                    if (child.Name == name)
                        return child;

                if (FindChild<T>(childElement, name) is T result)
                    return result;
            }
            return null;
        }

        /// <summary>
        /// 查找所有指定类型的子元素
        /// </summary>
        /// <typeparam name="T">要查找的元素类型</typeparam>
        /// <param name="parent">父元素</param>
        /// <returns>所有匹配的子元素列表</returns>
        public static List<T> FindChildren<T>(this DependencyObject parent)
            where T : DependencyObject
        {
            var result = new List<T>();

            if (parent is null)
                return result;

            FindChildrenRecursive(parent, result);

            return result;
        }

        /// <summary>
        /// 递归查找所有指定类型的子元素（辅助方法）
        /// </summary>
        /// <typeparam name="T">要查找的元素类型</typeparam>
        /// <param name="parent">父元素</param>
        /// <param name="result">结果列表</param>
        private static void FindChildrenRecursive<T>(DependencyObject parent, List<T> result)
            where T : DependencyObject
        {
            if (parent is null)
                return;

            for (var i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                var childElement = VisualTreeHelper.GetChild(parent, i);

                if (childElement is T child)
                {
                    result.Add(child);
                }

                // 递归查找子元素的子元素
                FindChildrenRecursive(childElement, result);
            }
        }
    }
}
