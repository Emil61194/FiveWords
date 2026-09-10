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

            DataHandler dh = new DataHandler();
            string[] data = dh.GetData("C:\\Users\\admin\\source\\repos\\FiveWords\\FiveWords\\words_beta.txt");
            //string[] data = dh.GetData("C:\\Users\\admin\\source\\repos\\FiveWords\\FiveWords\\words_alpha.txt");
            data = dh.KeepOnlyFiveLetterWords(data);
            data = dh.FilterRepeatingCharsInSameWord(data);
            MaskValues[] newData = dh.DataToUint(data);
            List<List<string>>  combinations = dh.GetCombinations(newData);
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
