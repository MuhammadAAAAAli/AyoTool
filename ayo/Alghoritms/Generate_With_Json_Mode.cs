using System.Collections.Generic;
using System.Linq;
using ayo.Interfaces;
using ayo.ProcessPDF;
using ayo.Static;

namespace ayo.Alghoritms
{
    public class Generate_With_Json_Mode : IAppMode
    {
        public Generate_With_Json_Mode(string jsonPath)
        {
            GetRawWordsFromDisk(jsonPath);
        }

        public Dictionary<string, Dictionary<string, int>> AllPdfWords { get; private set; }

        public string GetNextRawWord(string currentWordNeedsConnection = null)
        {
            if (currentWordNeedsConnection == null)
            {
                return AllPdfWords.ElementAt(RandomNumber.Get(0, AllPdfWords.Count)).Key;
            }
            if (AllPdfWords.ContainsKey(currentWordNeedsConnection))
                return
                    AllPdfWords[currentWordNeedsConnection].Keys.ElementAt(RandomNumber.Get(0,
                        AllPdfWords[currentWordNeedsConnection].Keys.Count));
            return null;
        }

        private void GetRawWordsFromDisk(string wordsInputPath)
        {
            AllPdfWords = Serialize.DeSerializeObject<Dictionary<string, Dictionary<string, int>>>(wordsInputPath);
        }
    }
}