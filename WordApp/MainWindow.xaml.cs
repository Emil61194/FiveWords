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
            ThreadRun.IsEnabled = false;
            try
            {
                Executor executor = new Executor();

                string filePath = FileLabel.Content?.ToString() ?? "";
                string wordLength = WordLength.Text;
                bool onlyUnique = OnlyUnique.IsChecked ?? true;

                IProgress<int> progress = new Progress<int>(value => { ProgressBar.Value = value; });
                executor.ClearResultsOnScreen(this);
                RunResult runResult = await Task.Run(() => executor.Run(filePath, wordLength, onlyUnique, 5, progress));
                executor.SetResultOnScreen(this, runResult);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                ThreadRun.IsEnabled = true;
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                OpenFileDialog ofd = new OpenFileDialog();
                if (ofd.ShowDialog() == true)
                {
                    FileLabel.Content = ofd.FileName;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            NoThreadRun.IsEnabled = false;
            try
            {
                Executor executor = new Executor();

                string filePath = FileLabel.Content?.ToString() ?? "";
                string wordLength = WordLength.Text;
                bool onlyUnique = OnlyUnique.IsChecked ?? true;

                IProgress<int> progress = new Progress<int>(value => { ProgressBar.Value = value; });
                executor.ClearResultsOnScreen(this);
                RunResult runResult = executor.Run(filePath, wordLength, onlyUnique, 5, progress);
                executor.SetResultOnScreen(this, runResult);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                NoThreadRun.IsEnabled = true;
            }
        }
    }
}