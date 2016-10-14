using System.Linq;
using System.Text;
using ayo.Interfaces;
using ayo.Static;

namespace ayo.Alghoritms
{
    public class SmartAlghoritm : BaseDataSmartAlghoritm, ISmartAlghoritm
    {
        private bool _capitalNextLetter;
        private bool _wordWithCapitalLetters;
        private string lastWord;

        public SmartAlghoritm(IAppMode appMode) : base(appMode)
        {
        }

        public string GetNextPassword()
        {
            var pass = new StringBuilder();
            foreach (var userOption in Queue)
            {
                pass.Append(userOption());
            }
            return pass.ToString();
        }

        #region Generate mode options

        public string NextWordWithCapitalFirstLetter()
        {
            _capitalNextLetter = true;
            return string.Empty;
        }

        public string NextWordWithCapitalLetters()
        {
            _wordWithCapitalLetters = true;
            return string.Empty;
        }

        public string GetNumber()
        {
            return GetImportantNumbers.ElementAt(RandomNumber.Get(0, GetImportantNumbers.Count)).ToString();
        }

        public string GetSpecialChar()
        {
            return GetSpecialChars.ElementAt(RandomNumber.Get(0, GetSpecialChars.Count()));
        }

        public string GetRawWordAndAddOptions()
        {
            var word = _appMode.GetNextRawWord(lastWord);
            if (word == null)
                word = _appMode.GetNextRawWord();
            lastWord = word;

            if (_capitalNextLetter)
            {
                _capitalNextLetter = false;
                return word.First().ToString().ToUpper() + word.Substring(1);
            }
            if (_wordWithCapitalLetters)
            {
                _wordWithCapitalLetters = false;
                return word.ToUpper();
            }
            return word;
        }

        #endregion
    }
}