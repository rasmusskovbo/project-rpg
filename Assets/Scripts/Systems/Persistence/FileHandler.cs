using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class FileHandler
{
    private string path = "";
    private string filename = "";

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
            
            Debug.Log("TEST STRING OUT: " + dataToStore);

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
}
