using FiveWords;
using Microsoft.Win32;
using System.CodeDom.Compiler;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WordApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            Executor executor = new Executor();

            string filePath = FileLabel.Content?.ToString() ?? "";
            string wordLength = WordLength.Text;
            bool onlyUnique = OnlyUnique.IsChecked ?? true;
            RunResult runResult = await Task.Run(() => executor.Run(filePath, wordLength, onlyUnique));
            executor.ClearResultsOnScreen(this);
            executor.SetResultOnScreen(this, runResult);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == true)
            {
                FileLabel.Content = ofd.FileName;
            }
        }
        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            Executor executor = new Executor();
            executor.ClearResultsOnScreen(this);
            RunResult runResult = executor.Run(FileLabel.Content?.ToString() ?? "", WordLength.Text, OnlyUnique.IsChecked ?? true);
            executor.SetResultOnScreen(this, runResult);
        }
    }
}