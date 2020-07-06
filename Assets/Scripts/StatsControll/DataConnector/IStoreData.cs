public interface IStoreData
{
    void SaveString(string value, string name);
    void SaveInt(int value, string name);
    string LoadString(string name);
    int LoadInt(string name);
}
