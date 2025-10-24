namespace Su.WPF.CustomControl.TreeViewEx
{
    public class FileterOptions
    {
        /// <summary>
        /// 名字关键字
        /// </summary>
        public string NameKey { get; set; }

        /// <summary>
        /// 保留树结构选项
        /// </summary>
        public TreeViewExProviderKeyType Option { get; internal set; } =
            TreeViewExProviderKeyType.KeepTreeNodeStru;

        internal FileterOptions() { }
    }
}
