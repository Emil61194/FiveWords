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
        private IProgress<int>? progress = null;
        public DataHandler(IProgress<int>? progress = null)
        {
            this.progress = progress;
        }
        private string[] GetData(string filePath)
        {
            if (!File.Exists(filePath))
            {
                return new string[0];
            }

            string[] data = File.ReadAllLines(filePath);
            return data;
        }

        private string[] KeepWordsWithSpecificLength(string[] data, int wordLength)
        {
            return data.Where(x => x.Length == wordLength).ToArray();
        }

        private string[] FilterRepeatingCharsInSameWord(string[] data)
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

        private UnmaskedValue[] DataToUint(string[] data, bool onlyUniques)
        {
            List<UnmaskedValue> convertedData = new List<UnmaskedValue>();
            HashSet<uint> uniqueMasks = new HashSet<uint>();

            foreach (string word in data)
            {
                uint mask = 0;

                uint firstLetterMask = 0;
                char firstLetter = word[0];
                firstLetterMask |= 1u << (firstLetter - 'a');

                foreach (char c in word)
                {
                    mask |= 1u << (c - 'a');
                }
                if (onlyUniques == false || uniqueMasks.Add(mask))
                {
                    convertedData.Add(new UnmaskedValue { mask = mask, unmask = word });
                }

            }

            return convertedData.ToArray();
        }
        public List<List<string>> GetCombinations(string filePath, int wordLength, bool onlyUniques, int combinationLength)
        {
            string[] data = GetData(filePath);
            data = KeepWordsWithSpecificLength(data, wordLength);
            data = FilterRepeatingCharsInSameWord(data);
            UnmaskedValue[] newData = DataToUint(data, onlyUniques);

            List<List<UnmaskedValue>> combinations = new List<List<UnmaskedValue>>();
            FindCombinations(newData, new List<UnmaskedValue>(), 0, ref combinations, combinationLength);
            List<List<string>> refinedCombinations = new List<List<string>>();

            foreach (List<UnmaskedValue> list in combinations)
            {
                List<string> combination = new List<string>();
                foreach (UnmaskedValue value in list)
                {
                    combination.Add(value.unmask);
                }
                refinedCombinations.Add(combination);
            }
            return refinedCombinations;
        }

        private void FindCombinations(UnmaskedValue[] candidates, List<UnmaskedValue> combination, uint wordMask, ref List<List<UnmaskedValue>> combinations, int combinationLength)
        {
            if (combination.Count == combinationLength)
            {
                combinations.Add(combination.ToList());
                //Console.WriteLine(combinations.Count);
                return;
            }

            int needed = combinationLength - combination.Count;
            if (candidates.Length < needed)
            {
                return;
            }

            for (int i = 0; i < candidates.Length; i++)
            {
                UnmaskedValue word = candidates[i];

                if (progress != null && combination.Count == 0)
                {
                    int percentage = (int)((double)i / candidates.Length * 100);
                    progress?.Report(percentage);
                }

                // If word not eligable for combination "continue"
                if ((word.mask & wordMask) != 0)
                {
                    continue;
                }

                uint newMask = wordMask | word.mask;
                List<UnmaskedValue> remaining = new List<UnmaskedValue>();

                // Shorten list with eligable words before sending to recursive function
                for (int j = i + 1; j < candidates.Length; j++)
                {
                    UnmaskedValue nextWord = candidates[j];

                    if ((nextWord.mask & newMask) == 0)
                    {
                        remaining.Add(nextWord);
                    }
                }

                // Check if enough words in list left to finish combination
                if (remaining.Count < needed - 1)
                {
                    continue;
                }

                combination.Add(word);
                FindCombinations(remaining.ToArray(), combination, newMask, ref combinations, combinationLength);
                combination.RemoveAt(combination.Count - 1);
            }
        }
    }
}
