using System.Collections.Generic;

public interface IDataPersistence
{
    void LoadData(GameData data);
    void SaveData(GameData data);
}
