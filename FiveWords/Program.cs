using System.Diagnostics;

namespace FiveWords
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            DataHandler dh = new DataHandler();
            string[] data = dh.GetData("C:\\Users\\admin\\source\\repos\\FiveWords\\FiveWords\\words_beta.txt");
            data = dh.KeepOnlyFiveLetterWords(data);
            data = dh.FilterRepeatingCharsInSameWord(data);
            dh.GetCombinations(data);

            sw.Stop();
            TimeSpan timeSpan = sw.Elapsed;
            Console.WriteLine(timeSpan.ToString(@"hh\:mm\:ss\:fff"));
        }
    }
}
