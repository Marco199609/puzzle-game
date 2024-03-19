using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.IO;
using UnityEngine;

public class FileDataHandler
{
    private string dataDirPath = string.Empty;
    private string fileName = string.Empty;
    private bool useEncryption = false;
    private readonly string encryptioncodeWord = "word";

    public FileDataHandler(string dataDirPath, string fileName, bool useEncryption)
    {
        this.dataDirPath = dataDirPath;
        this.fileName = fileName;
        this.useEncryption = useEncryption;
    }

    public GameData Load()
    {
        ///Use Path.Combine to account for different OS's having different path separators
        string fullPath = Path.Combine(dataDirPath, fileName);
        GameData loadedData = null;
        if(File.Exists(fullPath))
        {
            try
            {
                ///Load the serialized data from the file
                string dataToLoad = string.Empty;

                using(FileStream fileStream = new FileStream(fullPath, FileMode.Open))
                {
                    using(StreamReader reader = new StreamReader(fileStream))
                    {
                        dataToLoad = reader.ReadToEnd();
                    }
                }

                ///Optionally decrypt data
                if (useEncryption)
                {
                    dataToLoad = EncryptDecrypt(dataToLoad);
                }

                ///Deserialize data from Json back into the C# object
                loadedData = JsonConvert.DeserializeObject<GameData>(dataToLoad);
            }
            catch (Exception e)
            {
                UnityEngine.Debug.LogError("Error occured when trying to load data from file: " + fullPath + "\n" + e);
            }

        }
        return loadedData;
    }

    public void Save(GameData data)
    {
        ///Use Path.Combine to account for different OS's having different path separators
        string fullPath = Path.Combine(dataDirPath, fileName);

        try
        {
            ///Create the directory the file will be written to if it doesn't already exist
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

            ///Serialize the C# game data object into Json
            string dataToStore = JsonConvert.SerializeObject(data);

            ///Optionally encrypt data
            if(useEncryption)
            {
                dataToStore = EncryptDecrypt(dataToStore);
            }

            ///Write the serialized data to the file
            using(FileStream fileStream = new FileStream(fullPath, FileMode.Create))
            {
                using(StreamWriter writer = new StreamWriter(fileStream))
                {
                    writer.Write(dataToStore);
                }
            }
        }
        catch(Exception e)
        {
            UnityEngine.Debug.LogError("Error occured when trying to save data to file: " + fullPath + "\n" + e);
        }
    }

    ///Below is a simple implementation of XOR encryption
    private string EncryptDecrypt(string data)
    {
        string modifiedData = string.Empty;

        for(int i = 0; i < data.Length; i++)
        {
            modifiedData += (char) (data[i] ^ encryptioncodeWord[i % encryptioncodeWord.Length]);
        }

        return modifiedData;
    }
}
