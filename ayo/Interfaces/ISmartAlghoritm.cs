namespace ayo.Interfaces
{
    public interface ISmartAlghoritm
    {
        string GetNextPassword();
        string NextWordWithCapitalFirstLetter();
        string NextWordWithCapitalLetters();
        string GetNumber();
        string GetRawWordAndAddOptions();
        string GetSpecialChar();
    }
}