using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[DefaultExecutionOrder(1000)]
public class GameOverUI : MonoBehaviour
{
    [SerializeField] GameObject gameOver;
    [SerializeField] Text winnerText;
    // Start is called before the first frame update
    void Start()
    {
        DataManager.instance.onEndGame.AddListener((int winner) =>
        {
            winnerText.text = "Player " + (winner + 1) + " win!";
            gameOver.SetActive(true);
        });
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if(DataManager.instance.gameState == GameState.End)
        {
            if(Input.anyKeyDown)
            {
                DataManager.instance.gameState = GameState.None;
                SceneManager.LoadScene(0);
                DataManager.instance.SetMusicType(MusicType.Menu);
            }
        }
    }
}
