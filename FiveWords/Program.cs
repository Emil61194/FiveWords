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
            //string[] data = dh.GetData("C:\\Users\\admin\\source\\repos\\FiveWords\\FiveWords\\words_beta.txt");
            string[] data = dh.GetData("C:\\Users\\admin\\source\\repos\\FiveWords\\FiveWords\\words_alpha.txt");
            data = dh.KeepOnlyFiveLetterWords(data);
            data = dh.FilterRepeatingCharsInSameWord(data);
            Dictionary<uint, List<uint>> newData = dh.DataToUint(data);
            dh.GetCombinations(newData);

            sw.Stop();
            TimeSpan timeSpan = sw.Elapsed;
            Console.WriteLine(timeSpan.ToString(@"hh\:mm\:ss\:fff"));
        }
    }
}
