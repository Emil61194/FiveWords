using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace FiveWords
{
    public class DataHandler
    {
        public string[] GetData(string filePath)
        {
            string[] data = File.ReadAllLines(filePath);
            return data;
        }

        public string[] KeepOnlyFiveLetterWords(string[] data)
        {
            return data.Where(x => x.Length == 5).ToArray();
        }
        public string[] FilterRepeatingCharsInSameWord(string[] data) {
            List<string> results = new List<string>();          
            
            foreach(string word in data)
            {
                if (!word.GroupBy(x => x).Any(x => x.Count() > 1))
                {
                    results.Add(word);
                }
            }

            return results.ToArray();
        }


        public void GetCombinations(string[] data)
        {
            List<List<string>> combinations = new List<List<string>>();

            FindCombinations(data, new List<string>(), new HashSet<char>(), 0, combinations);

            Console.WriteLine(combinations.Count());

            foreach (var combination in combinations)
            {
                Console.WriteLine(string.Join(" ", combination));
            }
        }

        private void FindCombinations(string[] data, List<string> combination, HashSet<char> usedChars, int start, List<List<string>> combinations)
        {
            if (combination.Count == 5)
            {
                combinations.Add(new List<string>(combination));
                return;
            }

            for (int i = start; i < data.Length; i++)
            {
                string word = data[i];

                if (word.Any(c => usedChars.Contains(c)))
                {
                    continue;
                }

                combination.Add(word);

                foreach (char c in word)
                {
                    usedChars.Add(c);
                }

                FindCombinations(data, combination, usedChars, i + 1, combinations);

                combination.RemoveAt(combination.Count - 1);

                foreach (char c in word)
                {
                    usedChars.Remove(c);
                }
            }
        }
    }
}
