using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor.ShaderGraph.Serialization;
using UnityEngine;

public class JsonDataService : IDataService
{
    public bool SaveData<T>(string relativePath, T data, bool encrypted = false)
    {
        string path = Application.persistentDataPath + relativePath;

        try
        {
            if (File.Exists(path))
            {
                Debug.LogWarning("Deleting old file and creating new");
                File.Delete(path);
            }
            else
            {
                Debug.Log("Writing new file");
            }
                
            using FileStream stream = File.Create(path);
            stream.Close();
            File.WriteAllText(path, JsonConvert.SerializeObject(data, Formatting.Indented, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore, TypeNameHandling = TypeNameHandling.All}));
            return true;
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Error writing save to file: {e.Message} | {e.StackTrace}");
            return false;
        }
    }

    public T LoadData<T>(string relativePath, bool encrypted = false)
    {
        string path = Application.persistentDataPath + relativePath;

        if (!File.Exists(path)) 
        {
            Debug.LogError($"File doesnt exist at {path}");
            throw new FileNotFoundException($"{path} doesnt exist");
        }

        try
        {
            T data = JsonConvert.DeserializeObject<T>(File.ReadAllText(path));
            return data;
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Failed to load data due to {e.Message} | {e.StackTrace}");
            throw e;
        }
    }

    public System.Object LoadData(string relativePath, bool encrypted = false)
    {
        string path = Application.persistentDataPath + relativePath;

        if (!File.Exists(path))
        {
            Debug.LogError($"File doesnt exist at {path}");
            throw new FileNotFoundException($"{path} doesnt exist");
        }

        try
        {
            System.Object data = JsonConvert.DeserializeObject(File.ReadAllText(path), new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All});
            return data;
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Failed to load data due to {e.Message} | {e.StackTrace}");
            throw e;
        }
    }

}
