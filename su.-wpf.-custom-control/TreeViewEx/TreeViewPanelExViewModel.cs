using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Su.WPF.CustomControl.TreeViewEx
{
    internal class TreeViewPanelExViewModel : INotifyPropertyChanged
    {
        public TreeViewController TreeViewExProvider { get; set; }

        public TreeViewPanelExViewModel(TreeViewController treeViewExProvider)
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
