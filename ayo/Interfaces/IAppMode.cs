namespace ayo.Interfaces
{
    public interface IAppMode
    {
        string GetNextRawWord(string lastWord = null);
    }
}