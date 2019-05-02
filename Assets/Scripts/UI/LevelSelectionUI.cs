using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[DefaultExecutionOrder(100)]
public class LevelSelectionUI : MonoBehaviour
{
    [SerializeField] Transform container;
    [SerializeField] Button playButton;
    [SerializeField] Button prefabLevelButton;

    public LevelInfo levelToLoad;
    bool levelSelected = false;

    private void Start()
    {
        foreach(LevelInfo info in LevelManager.levels)
        {
            Button b = Instantiate(prefabLevelButton, container).GetComponent<Button>();
            b.GetComponentInChildren<Text>().text = info.name;

            b.onClick.AddListener(() =>
            {
                levelToLoad = info;
                levelSelected = true;
                playButton.interactable = true;
            });
        }
    }

    private void OnEnable()
    {
        levelSelected = false;
        playButton.interactable = false;
    }

    public void LoadLevel()
    {
        if (levelSelected)
        {
            AssetBundle bundle = AssetBundleManager.LoadBundle(LevelManager.FullPath(levelToLoad.path));
            Object[] assets = bundle.LoadAllAssets();

            foreach (Object asset in assets)
            {
                if (asset.GetType() == typeof(MapData))
                {
                    DataManager.instance.level = asset as MapData;
                    SceneManager.LoadScene(1);
                    break;
                }
            }
        }
    }

}
