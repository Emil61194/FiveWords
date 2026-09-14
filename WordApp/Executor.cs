using FiveWords;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows;

namespace WordApp
{
    public class Executor
    {
        public RunResult Run(string filePath, string wordLengthStr, bool onlyUnique, int combinationLength, IProgress<int>? progress = null)
        {
            int wordLength = 0;
            int.TryParse(wordLengthStr, out wordLength);

            Stopwatch sw = new Stopwatch();
            sw.Start();

            DataHandler dh = new DataHandler(progress);
            List<List<string>> combinations = dh.GetCombinations(filePath, wordLength, onlyUnique, combinationLength);

            sw.Stop();
            TimeSpan timeSpan = sw.Elapsed;

            return new RunResult { elapsed = timeSpan, combinations = combinations};
        }

        public void ClearResultsOnScreen(MainWindow mainWindow)
        {
            mainWindow.Runtime.Content = "Runtime: ";
            mainWindow.Combination.Content = $"Combinations: ";
            mainWindow.CombinationList.Items.Clear();
            mainWindow.ProgressBar.Visibility = Visibility.Visible;
            mainWindow.ProgressBar.Value = 0;
        }
        public void SetResultOnScreen(MainWindow mainWindow, RunResult runResult)
        {
            mainWindow.ProgressBar.Visibility = Visibility.Hidden;
            mainWindow.Runtime.Content = $"Runtime: {runResult.elapsed.ToString(@"hh\:mm\:ss\:fff")}";
            mainWindow.Combination.Content = $"Combinations: {runResult.combinations.Count}";

            foreach (List<string> list in runResult.combinations)
            {
                string combination = "";
                foreach (string word in list)
                {
                    combination += word + " ";
                }
                mainWindow.CombinationList.Items.Add(combination.Trim());
            }
        }
    }
}
