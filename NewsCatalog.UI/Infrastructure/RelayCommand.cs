using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace NewsCatalog.UI.Infrastructure
{
    public class RelayCommand : ICommand
    {
        private Action<object> action;
        private Predicate<object> predicate;

        public RelayCommand(Action<object> execute, Predicate<object> predicate = null)
        {
            this.action = execute;
            this.predicate = predicate;
        }

        public event EventHandler CanExecuteChanged
        {
            add
            {
                CommandManager.RequerySuggested += value;
            }
            remove
            {
                CommandManager.RequerySuggested -= value;
            }
        }

        public bool CanExecute(object parameter) => predicate == null ? true : predicate(parameter);

        public void Execute(object parameter) => action(parameter);
    }
}
