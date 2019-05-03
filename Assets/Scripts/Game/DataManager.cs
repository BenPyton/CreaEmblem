using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.IO;
using System.Linq;

public class GameEvent : UnityEvent<int> { }

public enum MusicType
{
    Menu,
    Selection,
    Battle,
    Victory
}

public enum GameState
{
    None,
    Start,
    Playing,
    Pause,
    Battle,
    Transition,
    End,
    NbState
}

[System.Serializable]
public struct HeroTeam
{
    public int team;
    public HeroInfo info;
}

[DefaultExecutionOrder(-1000)]
public class DataManager : MonoBehaviour
{
    static public DataManager instance = null;

    public int nbTeam { get; } = 2;

    private int m_teamPlaying = -1;
    public int teamPlaying { get { return m_teamPlaying; } }
    public int nextTeam { get { return (m_teamPlaying + 1) % nbTeam; } }

    public new AudioManager audio = new AudioManager();
    public DataFile configFile = new DataFile();
    public HeroManager heroes = new HeroManager();

    public GameState gameState = GameState.None;
    
    [SerializeField] public List<HeroTeam> heroToSpawn = new List<HeroTeam>(); 

    // Events
    public GameEvent onStartTurn = new GameEvent();
    public GameEvent onEndTurn = new GameEvent();
    public UnityEvent onStartGame = new UnityEvent();
    public GameEvent onEndGame = new GameEvent();
    public HeroEvent onHeroDeath = new HeroEvent();

    public MapData level = null;

    Coroutine endTurnCoroutine = null;
    Coroutine endGameCoroutine = null;

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

            AssetBundleManager.LoadAllBundlesFrom("terrains/palettes");
            AssetBundleManager.LoadAllBundlesFrom("terrains/zones");
            AssetBundleManager.LoadAllBundlesFrom("stats");
            AssetBundleManager.LoadAllBundlesFrom("weapons");
            AssetBundleManager.LoadAllBundlesFrom("mounts");

            heroes.LoadHeroes();

            foreach(HeroInfo data in heroes.heroes)
            {
                Debug.Log("Hero bundle name: " + data.bundleName);
            }

            heroToSpawn.Clear();

            HeroTeam hero1 = new HeroTeam() { team = 1, info = new HeroInfo() };
            hero1.info = heroes.heroes[0];
            heroToSpawn.Add(hero1);

            HeroTeam hero2 = new HeroTeam() { team = 0, info = new HeroInfo() };
            hero2.info = heroes.heroes[1];
            heroToSpawn.Add(hero2);

            HeroTeam hero3 = new HeroTeam() { team = 1, info = new HeroInfo() };
            hero3.info = heroes.heroes[0];
            heroToSpawn.Add(hero3);

            HeroTeam hero4 = new HeroTeam() { team = 0, info = new HeroInfo() };
            hero4.info = heroes.heroes[1];
            heroToSpawn.Add(hero4);
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
            heroes.SaveHeroes();
            configFile.Write("config.txt");
            audio.Clear();
        }
    }

    private void Start()
    {
        audio["Musics/BGM"].Play();
    }

    // Update is called once per frame
    void Update()
    {
        CheckEndGame();
        CheckEndTurn();
    }

    public void SetMusicType(MusicType _type)
    {
        audio["Musics/BGM"].Set("Type", (float)_type);
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
        // Stop a potentially end turn coroutine
        if(endTurnCoroutine != null)
        {
            StopCoroutine(endTurnCoroutine);
        }

        if (endGameCoroutine == null)
        {
            endGameCoroutine = StartCoroutine(EndGameCoroutine(_winner));
        }
    }

    public void BeginTurn()
    {
        gameState = GameState.Playing;
        m_teamPlaying = nextTeam;
        onStartTurn.Invoke(teamPlaying);
    }

    public void EndTurn()
    {
        if (endTurnCoroutine == null && endGameCoroutine == null)
        {
            endTurnCoroutine = StartCoroutine(EndTurnCoroutine());
        }
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
        yield return new WaitUntil(() => gameState == GameState.Playing || gameState == GameState.Start);
        gameState = GameState.Transition;
        onEndTurn.Invoke(teamPlaying);
        endTurnCoroutine = null;
        audio["Game/ChangeTurn"].Play();
    }


    IEnumerator EndGameCoroutine(int _winner)
    {
        yield return new WaitUntil(() => gameState == GameState.Playing);

        Debug.Log("Endgame, winner: " + _winner);
        gameState = GameState.End;
        SetMusicType(MusicType.Victory);
        onEndGame.Invoke(_winner);
        endGameCoroutine = null;
    }
}
