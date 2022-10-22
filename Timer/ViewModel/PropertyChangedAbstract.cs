using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Timer.ViewModel
{
    public abstract class PropertyChangedAbstract : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string? property = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));

        protected bool Set<T>(ref T param, T value, string property = null)
        {
            if (Equals(param, value))
                return false;
            else
            {
                param = value;
                OnPropertyChanged(property);
                return true;
            }
        }
        public virtual void ExecuteExternalCommand(string command)
        {
        }
        public virtual void ExecuteExternalCommand(string command, string commandParameter)
        {
        }
        public virtual void AcceptMessage(object message)
        {
        }
        public virtual void SendMessage(object message, string acceptor)
        {
        }
    }
}
