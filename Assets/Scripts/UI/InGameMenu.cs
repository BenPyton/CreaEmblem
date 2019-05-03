using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameMenu : MonoBehaviour
{
    [SerializeField] GameObject mainMenu;
    [SerializeField] GameObject settingsMenu;

    GameState previousState = GameState.None;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Pause(bool pause)
    {
        previousState = DataManager.instance.gameState;
        DataManager.instance.gameState = GameState.Pause;
        DisplayMenu();
        Time.timeScale = 0.0f;
    }

    public void Resume()
    {
        DataManager.instance.gameState = previousState;
        mainMenu.SetActive(false);
        settingsMenu.SetActive(false);
        Time.timeScale = 1.0f;
    }

    public void DisplayMenu()
    {
        mainMenu.SetActive(true);
        settingsMenu.SetActive(false);
    }

    public void DisplaySettings()
    {
        mainMenu.SetActive(false);
        settingsMenu.SetActive(true);
    }

    public void Abandon()
    {
        Time.timeScale = 1.0f;
        DataManager.instance.gameState = previousState;
        mainMenu.SetActive(false);
        settingsMenu.SetActive(false);
        DataManager.instance.EndGame(DataManager.instance.nextTeam);
    }
}
