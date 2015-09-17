namespace NBlast.Rest.Configuration
{
    public interface IConfigReader
    {
        string Read(string key);
        int ReadAsInt(string key);
    }
}