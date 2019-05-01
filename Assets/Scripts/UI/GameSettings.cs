using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[DefaultExecutionOrder(1000)]
public class GameSettings : MonoBehaviour {

    [SerializeField] Slider masterSlider;
    [SerializeField] Slider musicSlider;
    [SerializeField] Slider sfxSlider;
    [SerializeField] Slider uiSlider;

    //bool started = false;

    // when displayed
    private void Awake () {

        //started = true;

        masterSlider.onValueChanged.AddListener((float value) =>
        {
            DataManager.instance.audio.SetVCAVolume("Master", value);
        });

        musicSlider.onValueChanged.AddListener((float value) =>
        {
            DataManager.instance.audio.SetVCAVolume("Music", value);
        });

        sfxSlider.onValueChanged.AddListener((float value) =>
        {
            DataManager.instance.audio.SetVCAVolume("SFX", value);
        });

        uiSlider.onValueChanged.AddListener((float value) =>
        {
            DataManager.instance.audio.SetVCAVolume("UI", value);
        });
    }

    private void OnEnable()
    {
        masterSlider.value = DataManager.instance.configFile["master-volume"].GetFloat();
        musicSlider.value = DataManager.instance.configFile["music-volume"].GetFloat();
        sfxSlider.value = DataManager.instance.configFile["sfx-volume"].GetFloat();
        uiSlider.value = DataManager.instance.configFile["ui-volume"].GetFloat();

        //if (started)
        //{
        //    soundSlider.Select();
        //}
    }


    public void OnApply()
    {
        Debug.Log("Apply settings");
        DataManager.instance.configFile["master-volume"].SetFloat(masterSlider.value);
        DataManager.instance.configFile["music-volume"].SetFloat(musicSlider.value);
        DataManager.instance.configFile["sfx-volume"].SetFloat(sfxSlider.value);
        DataManager.instance.configFile["ui-volume"].SetFloat(uiSlider.value);


        DataManager.instance.ApplySettings();
    }

    public void OnCancel()
    {
        DataManager.instance.audio.SetVCAVolume("Master", DataManager.instance.configFile["master-volume"].GetFloat());
        DataManager.instance.audio.SetVCAVolume("Music", DataManager.instance.configFile["music-volume"].GetFloat());
        DataManager.instance.audio.SetVCAVolume("SFX", DataManager.instance.configFile["sfx-volume"].GetFloat());
        DataManager.instance.audio.SetVCAVolume("UI", DataManager.instance.configFile["ui-volume"].GetFloat());
    }


}
