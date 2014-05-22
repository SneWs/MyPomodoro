using System;
using System.Threading;
using System.Windows.Input;
using FirstFloor.ModernUI.Presentation;

namespace MyPomodoro.ViewModels
{
    internal enum PomodoroState
    {
        Stopped,
        Paused,
        Running
    }

    public class MainViewModel : BaseModel
    {
        private readonly RelayCommand _startPauseStopCommand;
        private readonly Timer _tickTimer;
        private TimeSpan _timeLeft;
        private PomodoroState _currentState;
        private bool _isFirstRun;
        private int _pomodorosCompleted;
        private Action _showPomodoroCompletedDialogInvoker;

        public MainViewModel()
        {
            _startPauseStopCommand = new RelayCommand(ExecuteStartStopPause);

            _tickTimer = new Timer(OnTimerTick, null, -1, -1);
            //_timeLeft = TimeSpan.FromMinutes(0.02d);
            _currentState = PomodoroState.Stopped;
            _isFirstRun = true;
            _pomodorosCompleted = 0;
            
            ResetTimeLeft();
        }

        public void RegisterPomodoroCompleteInvoker(Action showPomodoroCompletedDialog)
        {
            _showPomodoroCompletedDialogInvoker = showPomodoroCompletedDialog;
        }

        public string WindowTitle
        {
            get { return "Pomodoro"; }
        }

        public string TimeLeft
        {
            get { return _timeLeft.ToString("mm") + ":" + _timeLeft.ToString("ss"); }
        }

        public string CurrentButtonText
        {
            get
            {
                switch (_currentState)
                {
                    case PomodoroState.Running:
                        return "Pause";

                    case PomodoroState.Paused:
                        return "Resume";

                    default:
                        return "Start";
                }
            }
        }

        public ICommand StartStopPauseCommand
        {
            get { return _startPauseStopCommand; }
        }

        public bool IsPlayButtonEnabled
        {
            get { return _currentState != PomodoroState.Running; }
        }

        public bool IsPauseButtonEnabled
        {
            get { return _currentState == PomodoroState.Running; }
        }

        public double Pomodoro1
        {
            get { return _pomodorosCompleted < 1 ? 0.20d : 1.0d; }
        }

        public double Pomodoro2
        {
            get { return _pomodorosCompleted < 2 ? 0.20d : 1.0d; }
        }

        public double Pomodoro3
        {
            get { return _pomodorosCompleted < 3 ? 0.20d : 1.0d; }
        }

        public double Pomodoro4
        {
            get { return _pomodorosCompleted < 4 ? 0.20d : 1.0d; }
        }

        private void OnTimerTick(object state)
        {
            _timeLeft -= TimeSpan.FromSeconds(1);

            if (_timeLeft <= TimeSpan.FromSeconds(0))
            {
                _currentState = PomodoroState.Stopped;
                StopTimer();

                UpdateCompletedPomodoros();

                _timeLeft = TimeSpan.FromSeconds(0);

                BeginInvoke(() => _showPomodoroCompletedDialogInvoker());
            }

            BeginInvoke(() => OnPropertyChanged("TimeLeft"));
        }

        private void UpdateCompletedPomodoros()
        {
            _pomodorosCompleted++;
            for (var i = 1; i <= 4; i++)
            {
                OnPropertyChanged(string.Format("Pomodoro{0}", i));
            }
        }

        private void ExecuteStartStopPause(object obj)
        {
            if (_currentState == PomodoroState.Stopped ||
                _currentState == PomodoroState.Paused)
            {
                var needsReset = _currentState == PomodoroState.Stopped && !_isFirstRun;
                _currentState = PomodoroState.Running;

                if (needsReset)
                {
                    ResetTimeLeft();
                }

                StartTimer();
                _isFirstRun = false;
            }
            else
            {
                _currentState = PomodoroState.Paused;
                StopTimer();
            }
        }

        private void ResetTimeLeft()
        {
            _timeLeft = TimeSpan.FromMinutes(25.0D);
        }

        private void StartTimer()
        {
            _tickTimer.Change(TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(1));
            
            BeginInvoke(() => OnPropertyChanged("CurrentButtonText"));
            BeginInvoke(() => OnPropertyChanged("IsPlayButtonEnabled"));
            BeginInvoke(() => OnPropertyChanged("IsPauseButtonEnabled"));
        }

        private void StopTimer()
        {
            _tickTimer.Change(-1, -1);

            BeginInvoke(() => OnPropertyChanged("CurrentButtonText"));
            BeginInvoke(() => OnPropertyChanged("IsPlayButtonEnabled"));
            BeginInvoke(() => OnPropertyChanged("IsPauseButtonEnabled"));
        }
    }
}
