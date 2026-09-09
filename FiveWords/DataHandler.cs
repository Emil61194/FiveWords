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

        public string[] FilterRepeatingCharsInSameWord(string[] data)
        {
            List<string> results = new List<string>();

            foreach (string word in data)
            {
                if (!word.GroupBy(x => x).Any(x => x.Count() > 1))
                {
                    results.Add(word);
                }
            }

            return results.ToArray();
        }

        public Dictionary<uint, List<uint>> GetDataTemplate()
        {
            Dictionary<uint, List<uint>> data = new Dictionary<uint, List<uint>>();
            string supportedLetters = "qwertyuiopasdfghjklzxcvbnm";
            foreach (char c in supportedLetters)
            {
                uint mask = 0;
                data.Add(mask |= 1u << (c - 'a'), new List<uint>());
            }
            return data;
        }
        public Dictionary<uint, List<uint>> DataToUint(string[] data)
        {
            Dictionary<uint, List<uint>> result = GetDataTemplate();
            HashSet<uint> uniqueMasks = new HashSet<uint>();

            foreach (string word in data)
            {
                uint mask = 0;

                uint firstLetterMask = 0;
                char firstLetter = word[0];
                firstLetterMask |= 1u << (firstLetter - 'a');
                if (!result.ContainsKey(firstLetterMask))
                {
                    continue;
                }

                foreach (char c in word)
                {

                    mask |= 1u << (c - 'a');
                }
                if (uniqueMasks.Add(mask))
                {
                    result[firstLetterMask].Add(mask);
                }

            }

            return result;
        }
        public void GetCombinations(Dictionary<uint, List<uint>> data)
        {
            int combinations = 0;
            FindCombinations(data, 0, new uint(), ref combinations, 0, 0);
            Console.WriteLine("Total combinations: " + combinations);
        }

        private void FindCombinations(Dictionary<uint, List<uint>> data, int combinationCount, uint wordMask, ref int combinations, int startIndex, int listIndex)
        {

            if (combinationCount == 5)
            {
                combinations += 1;
                Console.WriteLine(combinations);
                return;
            }

            int i = startIndex;
            foreach (KeyValuePair<uint, List<uint>> kvp in data.Skip(startIndex))
            {
                if ((kvp.Key & wordMask) == 0)
                {
                    int ii = listIndex;
                    for (; ii < kvp.Value.Count; ii++)
                    {
                        uint word = kvp.Value[ii];
                        if ((word & wordMask) != 0)
                        {
                            continue;
                        }
                        combinationCount += 1;
                        FindCombinations(data, combinationCount, wordMask | word, ref combinations, i, ii + 1);
                        combinationCount -= 1;
                    }
                }
                listIndex = 0;
                i++;
            }
        }
    }
}
