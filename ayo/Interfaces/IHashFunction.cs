namespace ayo.Interfaces
{
    public interface IHashFunction
    {
        string DoHash(string ssidName, string pass);
    }
}