using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-50)]
public class PlayerStart : MonoBehaviour
{
    [SerializeField] public int team = -1;
    
    void Start()
    {
        MapManager.RegisterPlayerStart(this);
    }

    private void OnDestroy()
    {
        MapManager.UnregisterPlayerStart(this);
    }
}
