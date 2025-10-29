using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Su.WPF.CustomControl.TreeViewEx
{
    internal class TreeViewPanelExViewModel : INotifyPropertyChanged
    {
        public TreeViewExController TreeViewExProvider { get; set; }

        public TreeViewPanelExViewModel(TreeViewExController treeViewExProvider)
        {
            this.TreeViewExProvider = treeViewExProvider;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
