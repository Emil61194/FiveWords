using FiveWords;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace WordApp
{
    public class Executor
    {
        private MainWindow mainWindow;
        public Executor(MainWindow mainWindow) 
        {
            this.mainWindow = mainWindow;
        }
        public void Run()
        {
            string filePath = mainWindow.FileLabel.Content?.ToString() ?? "";
            int wordLength = 0;
            int.TryParse(mainWindow.WordLength.Text, out wordLength);
            bool onlyUnique = mainWindow.OnlyUnique.IsChecked ?? true;

            mainWindow.CombinationList.Items.Clear();

            Stopwatch sw = new Stopwatch();
            sw.Start();

            DataHandler dh = new DataHandler();
            List<List<string>> combinations = dh.GetCombinations(filePath, wordLength, onlyUnique);

            sw.Stop();
            TimeSpan timeSpan = sw.Elapsed;

            mainWindow.Runtime.Content = $"Runtime: {timeSpan.ToString(@"hh\:mm\:ss\:fff")}";
            mainWindow.Combination.Content = $"Combinations: {combinations.Count}";

            foreach (List<string> list in combinations)
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
