using System;

public interface IFileHandler
{
    public GameData Load();
    public void Save(GameData data);
    public String EncryptDecrypt(string data);

}
