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

    void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
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
}
