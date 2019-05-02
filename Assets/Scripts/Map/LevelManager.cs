using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[System.Serializable]
public struct LevelInfo
{
    public string name;
    public string path;
    public int difficulty;
}

// class for lazy loading level list
public static class LevelManager
{
    private static List<LevelInfo> m_levels = null;
    public const string levelPath = "levels";

    public static List<LevelInfo> levels {
        get
        {
            if(m_levels == null)
            {
                ListAllLevels();
            }
            return m_levels;
        }
    }

    public static void ListAllLevels()
    {
        string path = Path.Combine(Application.streamingAssetsPath, "levels.json");

        using (FileStream file = new FileStream(path, FileMode.Open))
        using (StreamReader reader = new StreamReader(file))
        {
            m_levels = JsonUtility.FromJson<JSONLevelList>(reader.ReadToEnd()).levels;
        }
    }

    public static string FullPath(string _path)
    {
        return Path.Combine(levelPath, _path);
    }
}

[System.Serializable]
public struct JSONLevelList
{
    public List<LevelInfo> levels;
}