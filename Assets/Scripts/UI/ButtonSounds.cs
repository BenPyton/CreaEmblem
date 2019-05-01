using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonSounds : MonoBehaviour {
    [SerializeField] string soundName = "ButtonClick";

    // Use this for initialization
    void Awake () {
        GetComponent<Button>().onClick.AddListener(() => {
            DataManager.instance.audio[soundName].Play();
        });
    }
}
