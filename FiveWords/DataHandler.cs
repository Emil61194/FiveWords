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

        public List<uint> DataToUint(string[] data)
        {
            List<uint> result = new List<uint>();

            foreach (string word in data)
            {
                uint mask = 0;

                foreach (char c in word)
                {
                    mask |= 1u << (c - 'a');
                }

                result.Add(mask);
            }

            return result;
        }
        public void GetCombinations(List<uint> data)
        {
            List<List<uint>> combinations = new List<List<uint>>();

            FindCombinations(data, new List<uint>(), new uint(), combinations, 0);

            foreach (var combination in combinations)
            {
                Console.WriteLine(string.Join(" ", combination));
            }

            Console.WriteLine(combinations.Count());
        }

        private void FindCombinations(List<uint> data, List<uint> combination, uint wordMask, List<List<uint>> combinations, int start)
        {
            if (combination.Count() == 5)
            {
                combinations.Add(new List<uint>(combination));
                Console.WriteLine(combinations.Count());
                return;
            }

            for (int i = start; i < data.Count(); i++)
            {
                uint word = data[i];

                if((word & wordMask) != 0)
                {
                    continue;
                }

                combination.Add(word);
                uint newMask = wordMask | word;

                FindCombinations(data, combination, newMask, combinations, i + 1);

                combination.RemoveAt(combination.Count() - 1);
            }
        }
    }
}
