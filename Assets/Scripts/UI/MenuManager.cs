using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField] GameObject titleScreen;
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
        titleScreen.SetActive(true);
        settingsScreen.SetActive(false);
        creditsScreen.SetActive(false);
        particles.enableEmission = true;
    }
    
    public void ShowSettingsScreen()
    {
        titleScreen.SetActive(false);
        settingsScreen.SetActive(true);
        creditsScreen.SetActive(false);
        particles.enableEmission = false;
    }
    
    public void ShowCreditsScreen()
    {
        titleScreen.SetActive(false);
        settingsScreen.SetActive(false);
        creditsScreen.SetActive(true);
        particles.enableEmission = false;
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void PlayGame()
    {
        SceneManager.LoadScene(1);
    }
}
