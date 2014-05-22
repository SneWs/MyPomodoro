using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using MyPomodoro.MVVM;

namespace MyPomodoro.ViewModels
{
    public abstract class BaseModel : INotifyPropertyChanged
    {
        private readonly IInjectableDispatcher _dispatcher;
        public event PropertyChangedEventHandler PropertyChanged;

        protected BaseModel()
            : this(new InjectableDispatcher(System.Windows.Threading.Dispatcher.CurrentDispatcher))
        { }

        protected BaseModel(IInjectableDispatcher dispatcher)
        {
            _dispatcher = dispatcher;
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public void BeginInvoke(Action d)
        {
            _dispatcher.BeginInvoke(d);
        }

        public void BeginInvoke(Action d, params object[] args)
        {
            _dispatcher.BeginInvoke(d, args);
        }

        public void Invoke(Action action)
        {
            _dispatcher.Invoke(action);
        }
    }
}
