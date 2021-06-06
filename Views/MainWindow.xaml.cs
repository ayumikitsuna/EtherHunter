using AndersL.EtherHunter.ViewModels;
using System.Windows;
using System.Windows.Input;
using System.Text.RegularExpressions;

namespace AndersL.EtherHunter
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            DataContext = new MainViewModel(this);
            InitializeComponent();
        }

        private void TextBox_OnPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !Regex.IsMatch(e.Text, @"^[0-9]");
        }

        private void AboutButton_Click(object sender, RoutedEventArgs e)
        {
            AboutWindow aboutWindow = new AboutWindow();
            aboutWindow.Show();
        }

        private void DisplayKeysButton_Click(object sender, RoutedEventArgs e)
        {
            KeysWindow keysWindow = new KeysWindow();
            keysWindow.DataContext = new KeysViewModel();
            keysWindow.Show();
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            ThreadBox.Text = ((MainViewModel) DataContext).ThreadCount.ToString();
            GweiBox.Text = ((MainViewModel) DataContext).GweiCount.ToString();
        }
    }
}
