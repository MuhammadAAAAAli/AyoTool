using System;
using System.Collections.Generic;
using ayo.Interfaces;
using ayo.Static;

namespace ayo.Alghoritms
{
    public class BaseDataSmartAlghoritm
    {
        protected IAppMode _appMode;

        public BaseDataSmartAlghoritm(IAppMode appMode)
        {
            _appMode = appMode;
            GenerateImportantNumbers();
            GenerateSpecialCharacters();
        }


        public Queue<Func<object>> Queue { get; set; } = new Queue<Func<object>>();
        public List<int> GetImportantNumbers { get; } = new List<int>();
        public List<string> GetSpecialChars { get; } = new List<string>();

        #region Generate relevant numbers and special characters

        private void GenerateImportantNumbers()
        {
            var index = 0;
            while (index < 2020)
            {
                GetImportantNumbers.Add(index);
                index++;
                if (index == 1000)
                    index = 1900;
            }
            Output.ToConsole("Generated numbers total - " + GetImportantNumbers.Count + " values ");
        }

        private void GenerateSpecialCharacters()
        {
            GetSpecialChars.Add(" ");
            GetSpecialChars.Add("!");
            GetSpecialChars.Add("\"");
            GetSpecialChars.Add("#");
            GetSpecialChars.Add("$");
            GetSpecialChars.Add("%");
            GetSpecialChars.Add("&");
            GetSpecialChars.Add("'");
            GetSpecialChars.Add("(");
            GetSpecialChars.Add(")");
            GetSpecialChars.Add("*");
            GetSpecialChars.Add("+");
            GetSpecialChars.Add(",");
            GetSpecialChars.Add("-");
            GetSpecialChars.Add(".");
            GetSpecialChars.Add("/");
        }

        #endregion
    }
}