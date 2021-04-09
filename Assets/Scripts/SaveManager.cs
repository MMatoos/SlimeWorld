using UnityEngine;
using System.IO;
public static class SaveManager
{
    public static string directory = "/SaveData/";

    public static void Save(LevelSaveData levelSaveData, string name)
    {
        int saveNumber = 0;
        string dir = Application.persistentDataPath + directory;

        if (!Directory.Exists(dir))
        {
            Directory.CreateDirectory(dir);
        }

        while (File.Exists(dir + name + saveNumber + ".txt") && name != "testedLevel")
            saveNumber++;
        GameObject.FindGameObjectWithTag("TestSave").GetComponent<EditorController>().saveNumber = saveNumber;
        string json = JsonUtility.ToJson(levelSaveData);
        if (name == "testedLevel")
        {
            File.WriteAllText(dir + name + ".txt", json);
        }
        else
        {
            File.WriteAllText(dir + name + saveNumber + ".txt", json);
        }
    }

    public static LevelSaveData Load(string name)
    {
        string fullPath;
        LevelSaveData levelSaveData = new LevelSaveData();
        if (name != "tutorial" && name != "zoo" && name != "parkour")
        {
            fullPath = Application.persistentDataPath + directory + name + ".txt";
            if (File.Exists(fullPath))
            {
                string json = File.ReadAllText(fullPath);
                levelSaveData = JsonUtility.FromJson<LevelSaveData>(json);
            }
            else
            {
                Debug.Log("Save file does not exist");
            }
        }
        else
        {
            TextAsset text = Resources.Load<TextAsset>(name);
            string json = text.text;
            levelSaveData = JsonUtility.FromJson<LevelSaveData>(json);
                
        }

        return levelSaveData;
    }
}
