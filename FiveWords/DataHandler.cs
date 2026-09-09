using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
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
            int combinations = 0;
            FindCombinations(data.ToArray(), 0, new uint(), ref combinations, 0);
            Console.WriteLine("Total combinations: "+combinations);
        }

        private void FindCombinations(uint[] data, int combinationCount, uint wordMask, ref int combinations, int start)
        {

            if (combinationCount == 5)
            {
                combinations += 1;
                //Console.WriteLine(combinations);
                return;
            }

            for (int i = start; i < data.Length; i++)
            {
                uint word = data[i];

                if((word & wordMask) != 0)
                {
                    continue;
                }

                combinationCount += 1;
                FindCombinations(data, combinationCount, wordMask | word, ref combinations, i + 1);
                combinationCount -= 1;
            }
        }
    }
}
