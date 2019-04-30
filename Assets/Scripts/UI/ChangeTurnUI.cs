using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[DefaultExecutionOrder(1000)]
public class ChangeTurnUI : MonoBehaviour
{
    [SerializeField] Text turnText;

    Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();

        DataManager.instance.onEndTurn.AddListener((_team) =>
        {
            int nextTeam = (_team + 1) % DataManager.instance.nbTeam;

            turnText.text = "Player " + (nextTeam + 1) + " Turn";
            anim.SetTrigger("ChangeTurn");
            Debug.Log("End Turn");
        });
    }

    public void NewTurn()
    {
        DataManager.instance.NewTurn();
        Debug.Log("New Turn");
    }
}
