using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class SliderValue : MonoBehaviour {

    private enum ValueType { Value, Percentage };

    [SerializeField] Slider slider = null;
    [SerializeField] ValueType type = ValueType.Percentage;
    [SerializeField] string unit = "";
    Text label;

	// Use this for initialization
	void Start () {
        slider.onValueChanged.AddListener(SetTextValue);
        label = GetComponent<Text>();
        SetTextValue(slider.value);

    }

    void SetTextValue(float value)
    {
        switch(type)
        {
            case ValueType.Value:
                label.text = value.ToString("F1") + unit;
                break;
            case ValueType.Percentage:
                if (slider.maxValue != slider.minValue)
                {
                    label.text = (100.0f * (value - slider.minValue) / (slider.maxValue - slider.minValue)).ToString("F0") + "%";
                }
                else
                {
                    label.text = "100%";
                }
                break;
        }
    }

}
