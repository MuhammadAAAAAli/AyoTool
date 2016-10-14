using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using ayo.Static;
using Newtonsoft.Json;

namespace ayo.ProcessPDF
{
    public class Serialize
    {
        private readonly Dictionary<string, Dictionary<string, int>> _allPdfWords =
            new Dictionary<string, Dictionary<string, int>>();

        private readonly string _outputFolderPath;
        private readonly int _procentage;

        public Serialize(string folderPath, string outputFolderPath, int procentage = 80)
        {
            _outputFolderPath = outputFolderPath;
            _procentage = procentage;
            if (Directory.Exists(folderPath))
                foreach (var filePath in Directory.GetFiles(folderPath))
                {
                    Output.ToConsole("Processing file " + Path.GetFileName(filePath) + " ...");
                    Process(filePath);
                }
            CleanItUp();
            SerializeAndSaveToDisk();
            PrintJson2Txt();
        }

        #region Process pdf, reorder, clean it up

        private void CleanItUp()
        {
            Output.ToConsole("Cleaning up ...");
            var _newAllPdfWords = new Dictionary<string, Dictionary<string, int>>();
            for (var i = 0; i < _allPdfWords.Count; i++)
            {
                var reorderedListForOneWord = ReorderListForOneWord(_allPdfWords.ElementAt(i).Value);
                var mainString = _allPdfWords.ElementAt(i).Key;
                _newAllPdfWords.Add(mainString, reorderedListForOneWord);
            }
            _allPdfWords.Clear();
            foreach (var word in _newAllPdfWords.Where(word => word.Value.Count > 0))
            {
                _allPdfWords.Add(word.Key, word.Value);
            }
        }

        private Dictionary<string, int> ReorderListForOneWord(Dictionary<string, int> listOfSecondaryWords)
        {
            var initilCount = listOfSecondaryWords.Count;
            var newDic = new Dictionary<string, int>();
            while (newDic.Count < Convert.ToInt32(initilCount*(100 - _procentage)/100))
            {
                string rememebreString = null;
                var rememberInt = -1;
                foreach (var test in listOfSecondaryWords)
                {
                    if (test.Value > rememberInt)
                    {
                        rememberInt = test.Value;
                        rememebreString = test.Key;
                    }
                }
                listOfSecondaryWords.Remove(rememebreString);
                newDic.Add(rememebreString, rememberInt);
            }
            return newDic;
        }

        public string[] GetFiles(string folderPath)
        {
            return Directory.GetFiles(folderPath);
        }

        public void Process(string filePath)
        {
            var text = PdfParser.ExtractTextFromPdf(filePath);
            var cleanText =
                Regex.Replace(text, "[^a-zA-Z.' ]+", "", RegexOptions.Compiled).Replace(".", " . ").ToLower();
            var textSplitedInToWords = cleanText.Split(' ');
            var word = textSplitedInToWords[0];
            var futureWordNotNeedConnection = false;
            foreach (var futureWord in textSplitedInToWords)
            {
                if (string.IsNullOrEmpty(futureWord)) continue;

                if (futureWord == word) continue; // pt ptima iteratie

                if (word == null) // daca suntem in prop noua
                {
                    word = futureWord;
                    continue;
                }

                if (!_allPdfWords.ContainsKey(word)) // add level 2 dictionary
                {
                    _allPdfWords.Add(word, new Dictionary<string, int>());
                }
                if (string.Equals(futureWord.Trim(), "."))
                {
                    word = null;
                    continue;
                }
                var okFutureWord = futureWord;
                if (futureWord.Contains('.')) // if end of sentence
                {
                    okFutureWord = futureWord.Replace(".", "");
                    futureWordNotNeedConnection = true;
                }

                var allPdfWordsLevel2 = _allPdfWords[word];
                if (!allPdfWordsLevel2.ContainsKey(okFutureWord))
                    allPdfWordsLevel2.Add(okFutureWord,
                        allPdfWordsLevel2.ContainsKey(okFutureWord) ? allPdfWordsLevel2[okFutureWord] + 1 : 1);
                else
                {
                    allPdfWordsLevel2[okFutureWord] += 1;
                }

                if (futureWordNotNeedConnection)
                {
                    word = null;
                    futureWordNotNeedConnection = false;
                }
                else
                {
                    word = okFutureWord;
                }
            }
            _allPdfWords.Remove(".");
        }

        public void PrintJson2Txt() // needs to be deleted or arranged
        {
            foreach (var mainWord in _allPdfWords)
            {
                Output.WriteJsonForDebug(mainWord.Key + " ----- ");
                var x = _allPdfWords[mainWord.Key];
                foreach (var followingWords in x)
                {
                    Output.WriteJsonForDebug("\t\t " + followingWords.Key + " - " + followingWords.Value + " !");
                }
            }
            Output.WriteJsonForDebug("end ---------------------- ");
        }

        public void SerializeAndSaveToDisk()
        {
            SerializeObject(_allPdfWords, _outputFolderPath + "\\serialized.json");
        }

        #endregion

        #region Serioalization and deserialization

        public void SerializeObject<T>(T serializableObject, string serializedJsonPath)
        {
            if (serializableObject == null)
            {
                return;
            }

            try
            {
                var json = JsonConvert.SerializeObject(serializableObject);
                File.WriteAllText(serializedJsonPath, json);
            }
            catch (Exception ex)
            {
                //Log exception here
            }
        }

        public static T DeSerializeObject<T>(string serializedObjectPath)
        {
            if (string.IsNullOrEmpty(serializedObjectPath))
            {
                return default(T);
            }

            var objectOut = default(T);

            try
            {
                var json = File.ReadAllText(serializedObjectPath);
                objectOut = JsonConvert.DeserializeObject<T>(json);
            }
            catch (Exception ex)
            {
                //Log exception here
            }

            return objectOut;
        }

        #endregion
    }
}