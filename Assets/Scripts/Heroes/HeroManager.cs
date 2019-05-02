using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;

// struct will contain saved informations
[System.Serializable]
public class HeroInfo
{
    public string bundleName; // unique name for each hero bundle (it's their id)
    public int level; // save current player level
    public int exp; // save current exp amount
}

public class HeroManager
{
    const string fileName = "heroes.json";

    private List<BundleFile> heroBundles = new List<BundleFile>();

    private List<HeroInfo> availableHeroes = new List<HeroInfo>();
    public List<HeroInfo> heroes
    {
        get { return availableHeroes; }
    }

    public HeroData GetHeroDataFromInfo(HeroInfo _info)
    {
        HeroData data = null;
        foreach(BundleFile bundleFile in heroBundles)
        {
            if(bundleFile.name == _info.bundleName)
            {
                AssetBundle bundle = AssetBundleManager.LoadBundle(bundleFile);
                List<HeroData> heroData = AssetBundleManager.LoadAllAssets<HeroData>(bundle);

                if(heroData.Count > 0)
                {
                    data = heroData[0];
                }

                break;
            }
        }
        return data;
    }


    // Get all hero infos of player saved file
    public void LoadHeroes()
    {
        heroBundles = AssetBundleManager.ListBundles("heroes");

        string path = Path.Combine(DirectoryManager.DataDirectory.FullName, fileName);

        if (!File.Exists(path))
        {
            Debug.Log("Heroes save file not found !");
        }
        else
        {
            using (FileStream file = new FileStream(path, FileMode.Open))
            using (StreamReader reader = new StreamReader(file))
            {
                availableHeroes = JsonUtility.FromJson<JSONHeroList>(reader.ReadToEnd()).heroes;
            }
        }

        CreateMissingHeroes();
    }

    void CreateMissingHeroes()
    {
        foreach(BundleFile bundle in heroBundles)
        {
            if(!availableHeroes.Exists(x => x.bundleName == bundle.name))
            {
                availableHeroes.Add(new HeroInfo() {
                    bundleName = bundle.name,
                    exp = 0,
                    level = 1
                });
            }
        }
    }


    public void SaveHeroes()
    {
        string path = Path.Combine(DirectoryManager.DataDirectory.FullName, fileName);

        using (FileStream file = new FileStream(path, File.Exists(path) ? FileMode.Truncate : FileMode.Create))
        using (StreamWriter writer = new StreamWriter(file))
        {
            JSONHeroList heroList = new JSONHeroList();
            heroList.heroes = new List<HeroInfo>();
            foreach(HeroInfo hero in availableHeroes)
            {
                heroList.heroes.Add(hero);
            }
            writer.Write(JsonUtility.ToJson(heroList, true));
        }
    }
}

[System.Serializable]
public struct JSONHeroList
{
    public List<HeroInfo> heroes;
}