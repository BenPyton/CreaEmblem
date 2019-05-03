using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
[RequireComponent(typeof(Image))]
public class SliderCustomFiller : MonoBehaviour
{
    [SerializeField] Slider slider;
    [SerializeField] float paddingMin = 0.0f;
    [SerializeField] float paddingMax = 0.0f;

    private Image m_image = null;
    private Image image {
        get {
            if (m_image == null)
            {
                m_image = GetComponent<Image>();
            }
            return m_image;
        }
    }

    [ExecuteInEditMode]
    private void OnEnable()
    {
        if (slider != null)
        {
            slider.onValueChanged.AddListener((float _value) =>
            {
                float range = 1.0f - paddingMin - paddingMax;
                image.fillAmount = _value * range + paddingMin;
            });
        }
        else
        {
            Debug.LogWarning("No slider set");
        }
    }
}
