using MiniTappsk.ViewModels;
using System.Windows;


namespace TinyTask
{
    public partial class MainWindow : Window
    {
        private MainViewModel ViewModel => (MainViewModel)DataContext;

        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainViewModel();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.StartNewTask();
            StartView.Visibility = Visibility.Collapsed;
            NewTaskView.Visibility = Visibility.Visible;
        }

        private void CancelNewTask_Click(object sender, RoutedEventArgs e)
        {
            NewTaskView.Visibility = Visibility.Collapsed;
            StartView.Visibility = Visibility.Visible;
        }

        private void SaveNewTask_Click(object sender, RoutedEventArgs e)
        {
            if (!ViewModel.AddNewTask())
            {
                MessageBox.Show("Bitte gib einen Titel für die Aufgabe ein.",
                    "Hinweis",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
                return;
            }

            NewTaskView.Visibility = Visibility.Collapsed;
            StartView.Visibility = Visibility.Visible;
        }
    }
}
