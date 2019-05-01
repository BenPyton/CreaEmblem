using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameEvent : UnityEvent<int> { }

[DefaultExecutionOrder(-1000)]
public class DataManager : MonoBehaviour
{
    static public DataManager instance = null;

    public int nbTeam { get; } = 2;

    private int m_teamPlaying = -1;
    public int teamPlaying { get { return m_teamPlaying; } }

    public bool blockInput = false;

    public GameEvent onStartTurn = new GameEvent();
    public GameEvent onEndTurn = new GameEvent();

    public new AudioManager audio = new AudioManager();
    public DataFile configFile = new DataFile();

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
        if(Input.GetKeyDown(KeyCode.Space))
        {
            NewTurn();
        }
    }

    public void NewTurn()
    {
        blockInput = false;
        m_teamPlaying = (m_teamPlaying + 1) % nbTeam;
        onStartTurn.Invoke(teamPlaying);
    }

    public void EndTurn()
    {
        blockInput = true;
        onEndTurn.Invoke(teamPlaying);
    }

    public void ApplySettings()
    {
        audio.SetVCAVolume("Master", configFile["master-volume"].GetFloat());
        audio.SetVCAVolume("Music", configFile["music-volume"].GetFloat());
        audio.SetVCAVolume("SFX", configFile["sfx-volume"].GetFloat());
        audio.SetVCAVolume("UI", configFile["ui-volume"].GetFloat());
    }
}
