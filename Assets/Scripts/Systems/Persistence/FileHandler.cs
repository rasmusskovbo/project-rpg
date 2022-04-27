using UnityEngine;
using System;
using System.IO;
using Newtonsoft.Json;

public class FileHandler
{
    private string path = "";
    private string filename = "";
    private string encryptionSeed = "waldheim";

    public FileHandler(string path, string filename)
    {
        this.path = path;
        this.filename = filename;
    }

    public GameData Load()
    {
        string fullPath = Path.Combine(path, filename);

        GameData loadedData = null;

        if (File.Exists(fullPath))
        {
            try
            {
                string dataToLoad = "";
                using (FileStream stream = new FileStream(fullPath, FileMode.Open))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        dataToLoad = reader.ReadToEnd();
                    }
                }

                //dataToLoad = EncryptDecrypt(dataToLoad);
                
                loadedData = JsonUtility.FromJson<GameData>(dataToLoad);
            }
            catch (Exception e)
            {
                Debug.Log("Error when trying to load data from file: " + fullPath + "\n " + e);   
            }
        }

        return loadedData;
    }

    public void Save(GameData data)
    {
        string fullPath = Path.Combine(path, filename);

        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));
            
            string dataToStore = JsonUtility.ToJson(data, true);
            //dataToStore = EncryptDecrypt(dataToStore);

            using (FileStream stream = new FileStream(fullPath, FileMode.Create))
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(dataToStore);
                }
            }
        }
        catch (Exception e)
        {
            Debug.Log("Error when trying to save data to file: " + fullPath + "\n " + e);
        }
    }

    private String EncryptDecrypt(string data)
    {
        string modifiedData = "";
        
        for (int i = 0; i < data.Length; i++)
        {
            modifiedData += (char) (data[i] ^ encryptionSeed[i % encryptionSeed.Length]);
        }

        return modifiedData;
    }
}
