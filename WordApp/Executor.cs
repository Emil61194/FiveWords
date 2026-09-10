using FiveWords;
using System;
using System.Collections.Generic;
using System.Text;

namespace WordApp
{
    public class Executor
    {
        public void Run(string filePath, int wordLength, bool onlyUnique)
        {
            DataHandler dh = new DataHandler();
            List<List<string>> combinations = dh.GetCombinations(filePath, wordLength, onlyUnique);
        }
    }
}
