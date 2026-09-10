using System.Collections.Specialized;
using System.Diagnostics;

namespace FiveWords
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting operation!");
            Stopwatch sw = new Stopwatch();
            sw.Start();

            string filePath = "";
            int wordLength = 5;
            bool onlyUniques = true;
            filePath = "C:\\Users\\admin\\source\\repos\\FiveWords\\FiveWords\\words_beta.txt";
            //filePath = "C:\\Users\\admin\\source\\repos\\FiveWords\\FiveWords\\words_alpha.txt";

            DataHandler dh = new DataHandler();           
            List<List<string>>  combinations = dh.GetCombinations(filePath, wordLength, onlyUniques, 5);
            sw.Stop();
            TimeSpan timeSpan = sw.Elapsed;

            foreach(List<string> list in combinations)
            {
                string combination = "";
                foreach (string word in list) 
                {
                    combination += word + " ";
                }
                Console.WriteLine(combination);
            }

            Console.WriteLine(timeSpan.ToString(@"hh\:mm\:ss\:fff"));
            Console.WriteLine("Total combinations: " + combinations.Count);
        }
    }
}
