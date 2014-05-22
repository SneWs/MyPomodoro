using System;
using System.Windows;
using FirstFloor.ModernUI.Windows.Controls;
using MyPomodoro.ViewModels;

namespace MyPomodoro
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : ModernWindow
    {
        public MainWindow()
        {
            ResizeMode = ResizeMode.CanMinimize;
            SourceInitialized += WhenSourceInitialized;
            Loaded += WhenWindowLoaded;
        }

        private void WhenSourceInitialized(object sender, EventArgs e)
        {
            SourceInitialized -= WhenSourceInitialized;
            this.HideMinimizeAndMaximizeButtons();
        }

        private void WhenWindowLoaded(object sender, RoutedEventArgs e)
        {
            Loaded -= WhenWindowLoaded;

            var viewModel = DataContext as MainViewModel;
            viewModel.RegisterPomodoroCompleteInvoker(ShowPomodoroCompletedDialog);
        }

        private void ShowPomodoroCompletedDialog()
        {
            var dlg = new ModernDialog {
                Title = "Pomodoro Complete",
                Content = "Good job!\r\nMake sure to take a short 5 min break before \r\nyou start you're next pomodoro."
            };

            dlg.ShowDialog();
        }
    }
}
