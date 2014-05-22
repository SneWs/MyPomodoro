using System;

namespace MyPomodoro.MVVM
{
    public interface IInjectableDispatcher
    {
        void BeginInvoke(Action d);

        void BeginInvoke(Action d, params object[] args);

        void Invoke(Action action);
    }
}