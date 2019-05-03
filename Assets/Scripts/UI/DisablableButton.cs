using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

[RequireComponent(typeof(Button))]
public class DisablableButton : MonoBehaviour
{
    Button button = null;

    // Start is called before the first frame update
    void Awake()
    {
        button = GetComponent<Button>();
    }

    // Update is called once per frame
    void Update()
    {
        button.interactable = DataManager.instance.gameState == GameState.Playing;
    }
}
