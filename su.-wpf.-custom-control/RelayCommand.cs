using System;
using System.Windows.Input;

namespace Su.WPF.CustomControl
{
    public class RelayCommand : ICommand
    {
        private Action action;
        private Action<object> action2;
        public event EventHandler CanExecuteChanged;

        public RelayCommand(Action action)
        {
            this.action = action;
        }

        public RelayCommand(Action<object> action)
        {
            this.action2 = action;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            action?.Invoke();
            action2?.Invoke(parameter);
        }
    }
}
