using System.Windows.Input;

namespace Su.WPF.CustomControl
{
    public class MyRelayCommand : ICommand
    {
        private Action action;
        private Action<object> action2;

        public event EventHandler CanExecuteChanged;

        public MyRelayCommand(Action action)
        {
            this.action = action;
        }

        public MyRelayCommand(Action<object> action)
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
