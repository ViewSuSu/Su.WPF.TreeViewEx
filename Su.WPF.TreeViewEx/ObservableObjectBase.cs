using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Su.WPF.CustomControl
{
    public abstract class ObservableObjectBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            try
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
            catch (Exception ex) { }
        }
    }
}
