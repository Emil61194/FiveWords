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

        private Dictionary<uint, List<UnmaskedValue>> GetDataTemplate()
        {
            Dictionary<uint, List<UnmaskedValue>> data = new Dictionary<uint, List<UnmaskedValue>> ();
            string supportedLetters = "qwertyuiopasdfghjklzxcvbnm";
            foreach (char c in supportedLetters)
            {
                uint mask = 0;
                data.Add(mask |= 1u << (c - 'a'), new List<UnmaskedValue>());
            }
            return data;
        }
        private MaskValues[] DataToUint(string[] data, bool onlyUniques)
        {
            Dictionary<uint, List<UnmaskedValue>> convertedData = GetDataTemplate();
            HashSet<uint> uniqueMasks = new HashSet<uint>();

            foreach (string word in data)
            {
                uint mask = 0;

                uint firstLetterMask = 0;
                char firstLetter = word[0];
                firstLetterMask |= 1u << (firstLetter - 'a');
                if (!convertedData.ContainsKey(firstLetterMask))
                {
                    continue;
                }

                foreach (char c in word)
                {

                    mask |= 1u << (c - 'a');
                }
                if (onlyUniques == false || uniqueMasks.Add(mask))
                {
                    convertedData[firstLetterMask].Add(new UnmaskedValue { mask = mask, unmask = word});
                }

            }

            List<MaskValues> maskValues = new List<MaskValues>();
            foreach(KeyValuePair<uint, List<UnmaskedValue>> kvp in convertedData)
            {
                maskValues.Add(new MaskValues {mask = kvp.Key, value = kvp.Value.ToArray() });
            }

            return maskValues.ToArray();
        }
        public List<List<string>> GetCombinations(string filePath, int wordLength, bool onlyUniques)
        {
            string[] data = GetData(filePath);
            data = KeepWordsWithSpecificLength(data, wordLength);
            data = FilterRepeatingCharsInSameWord(data);
            MaskValues[] newData = DataToUint(data, onlyUniques);

            List<List<UnmaskedValue>> combinations = new List<List<UnmaskedValue>>();
            FindCombinations(newData, new List<UnmaskedValue>(), new uint(), ref combinations, 0, 0);
            
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

        private void FindCombinations(MaskValues[] data, List<UnmaskedValue> combination, uint wordMask, ref List<List<UnmaskedValue>> combinations, int startIndex, int listIndex)
        {

            if (combination.Count == 5)
            {
                //combinations += 1;
                combinations.Add(combination.ToList());
                Console.WriteLine(combinations.Count);
                return;
            }

            for (int i = startIndex; i < data.Length; i++)
            {
                MaskValues kvp = data[i];
                if ((kvp.mask & wordMask) == 0)
                {
                    int ii = listIndex;
                    for (; ii < kvp.value.Length; ii++)
                    {
                        UnmaskedValue word = kvp.value[ii];
                        if ((word.mask & wordMask) != 0)
                        {
                            continue;
                        }
                        //combinationCount += 1;
                        combination.Add(word);
                        FindCombinations(data, combination, wordMask | word.mask, ref combinations, i, ii + 1);
                        //combinationCount -= 1;
                        combination.RemoveAt(combination.Count - 1);
                    }
                }
                listIndex = 0;
            }
        }
    }
}
