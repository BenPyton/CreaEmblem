using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField] GameObject titleScreen;
    [SerializeField] GameObject teamSelectionScreen;
    [SerializeField] GameObject levelSelectionScreen;
    [SerializeField] GameObject settingsScreen;
    [SerializeField] GameObject creditsScreen;
    [SerializeField] ParticleSystem particles;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void ShowTitleScreen()
    {
        DataManager.instance.SetMusicType(MusicType.Menu);
        titleScreen.SetActive(true);
        teamSelectionScreen.SetActive(false);
        levelSelectionScreen.SetActive(false);
        settingsScreen.SetActive(false);
        creditsScreen.SetActive(false);
        particles.enableEmission = true;
    }
    
    public void ShowSettingsScreen()
    {
        titleScreen.SetActive(false);
        teamSelectionScreen.SetActive(false);
        levelSelectionScreen.SetActive(false);
        settingsScreen.SetActive(true);
        creditsScreen.SetActive(false);
        particles.enableEmission = false;
    }
    
    public void ShowCreditsScreen()
    {
        titleScreen.SetActive(false);
        teamSelectionScreen.SetActive(false);
        levelSelectionScreen.SetActive(false);
        settingsScreen.SetActive(false);
        creditsScreen.SetActive(true);
        particles.enableEmission = false;
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void SelectTeam()
    {
        DataManager.instance.SetMusicType(MusicType.Selection);
        titleScreen.SetActive(false);
        teamSelectionScreen.SetActive(true);
        levelSelectionScreen.SetActive(false);
        settingsScreen.SetActive(false);
        creditsScreen.SetActive(false);
        particles.enableEmission = false;
    }

    public void SelectLevel()
    {
        titleScreen.SetActive(false);
        teamSelectionScreen.SetActive(false);
        levelSelectionScreen.SetActive(true);
        settingsScreen.SetActive(false);
        creditsScreen.SetActive(false);
        particles.enableEmission = false;
    }
}
