using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameEvent : UnityEvent<int> { }

public enum GameState
{
    None,
    Start,
    Playing,
    Battle,
    End,
    NbState
}

[System.Serializable]
public struct HeroTeam
{
    public int team;
    public HeroData data;
}

[DefaultExecutionOrder(-1000)]
public class DataManager : MonoBehaviour
{
    static public DataManager instance = null;

    public int nbTeam { get; } = 2;

    private int m_teamPlaying = -1;
    public int teamPlaying { get { return m_teamPlaying; } }

    public bool blockInput = false;

    public new AudioManager audio = new AudioManager();
    public DataFile configFile = new DataFile();

    public GameState gameState = GameState.None;
    
    [SerializeField] public List<HeroTeam> heroToSpawn = new List<HeroTeam>(); 

    // Events
    public GameEvent onStartTurn = new GameEvent();
    public GameEvent onEndTurn = new GameEvent();
    public UnityEvent onStartGame = new UnityEvent();
    public GameEvent onEndGame = new GameEvent();
    public HeroEvent onHeroDeath = new HeroEvent();



    void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            // Read config file to set audio volumes settings
            configFile.Read("config.txt");
            if (configFile.GetData().Length == 0) // setting default values when file is created
            {
                configFile["master-volume"].SetFloat(1.0f);
                configFile["music-volume"].SetFloat(1.0f);
                configFile["sfx-volume"].SetFloat(1.0f);
                configFile["ui-volume"].SetFloat(1.0f);
            }
            else
            {
                ApplySettings();
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        if(instance == this)
        {
            configFile.Write("config.txt");
            audio.Clear();
        }
    }

    private void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        CheckEndGame();
        CheckEndTurn();
    }

    void CheckEndTurn()
    {
        if (gameState == GameState.Playing)
        {
            bool canPlay = false;
            foreach (Hero hero in MapManager.GetAllHeroes(teamPlaying))
            {
                canPlay |= hero.canPlay;
            }
            
            if (!canPlay)
            {
                EndTurn();
            }
        }
    }

    void CheckEndGame()
    {
        if (gameState == GameState.Playing)
        {
            int winner = -1;
            bool endGame = true;
            for (int i = 0; i < nbTeam; i++)
            {
                // at least one hero alive in the team
                if (MapManager.GetAllHeroes(i).Count > 0)
                {
                    endGame &= winner == -1;
                    winner = i;
                }
            }
            if (endGame)
            {
                EndGame(winner);
            }
        }
    }

    public void StartGame()
    {
        if (gameState == GameState.None)
        {
            Debug.Log("Start Game");
            gameState = GameState.Start;
            m_teamPlaying = -1;
            onStartGame.Invoke();
            EndTurn();
        }
    }

    public void EndGame(int _winner)
    {
        if(gameState == GameState.Playing)
        {
            Debug.Log("Endgame, winner: " + _winner);
            gameState = GameState.End;
            onEndGame.Invoke(_winner);
        }
    }

    public void BeginTurn()
    {
        gameState = GameState.Playing;
        blockInput = false;
        m_teamPlaying = (m_teamPlaying + 1) % nbTeam;
        onStartTurn.Invoke(teamPlaying);
    }

    public void EndTurn()
    {
        StartCoroutine(EndTurnCoroutine());
    }

    public void ApplySettings()
    {
        audio.SetVCAVolume("Master", configFile["master-volume"].GetFloat());
        audio.SetVCAVolume("Music", configFile["music-volume"].GetFloat());
        audio.SetVCAVolume("SFX", configFile["sfx-volume"].GetFloat());
        audio.SetVCAVolume("UI", configFile["ui-volume"].GetFloat());
    }

    IEnumerator EndTurnCoroutine()
    {
        yield return new WaitUntil(() => gameState != GameState.Battle);
        blockInput = true;
        onEndTurn.Invoke(teamPlaying);
    }
}
