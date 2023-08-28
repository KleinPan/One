using System;
using System.Windows.Input;

namespace One.Control.Command
{
    public class ActionCommand<T> : ICommand
    {
        public event EventHandler CanExecuteChanged;

        private Action<T> _action;

        public ActionCommand(Action<T> action)
        {
            _action = action;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            if (_action != null)
            {
                var castParameter = (T)Convert.ChangeType(parameter, typeof(T));
                _action(castParameter);
            }
        }
    }

    /* Xaml:
     * <i:Interaction.Behaviors>
     *  <beh:EventToCommandBehavior Command="{Binding DropCommand}" Event="Drop" PassArguments="True" />
     * </i:Interaction.Behaviors>
     * ViewModel:
     *
     * public ActionCommand<DragEventArgs> DropCommand { get; private set; }
     *
     * this.DropCommand = new ActionCommand<DragEventArgs>(OnDrop);
     *
     * private void OnDrop(DragEventArgs e)
     * {
     *  //
     * }
     *
     */
}