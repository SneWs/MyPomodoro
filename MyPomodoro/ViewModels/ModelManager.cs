using System;

namespace MyPomodoro.ViewModels
{
    public class ModelManager
    {
        private readonly Lazy<MainViewModel> _mainViewModel;

        public ModelManager()
        {
            _mainViewModel = new Lazy<MainViewModel>(() => new MainViewModel());
        }

        public MainViewModel MainViewModel
        {
            get { return _mainViewModel.Value; }
        }
    }
}
