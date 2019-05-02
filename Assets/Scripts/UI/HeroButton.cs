using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
[RequireComponent(typeof(Button))]
public class HeroButton : MonoBehaviour
{
    [SerializeField] GameObject container;
    [SerializeField] Image portrait;
    [SerializeField] Image weaponIcon;
    [SerializeField] Text heroName;
    [SerializeField] Text heroLvl;

    Button button = null;
    Image background = null;

    private HeroInfo m_info;
    public HeroInfo info {
        get { return m_info; }
        set { m_info = value; UpdateButton(); }
    }

    private bool m_selected = false;
    public bool selected
    {
        get { return m_selected; }
        set { m_selected = value; UpdateSelected(); }
    }

    [SerializeField] public int team = -1;


    private void Start()
    {
        button = GetComponent<Button>();
        background = GetComponent<Image>();
        UpdateButton();
        SetBGColor();
    }

    private void UpdateButton()
    {
        if (info != null)
        {
            container.SetActive(true);
            HeroData data = DataManager.instance.heroes.GetHeroDataFromInfo(info);
            portrait.sprite = data.icon;
            weaponIcon.sprite = data.weapon.icon;
            heroName.text = data.displayName;
            heroLvl.text = info.level.ToString();
        }
        else
        {
            container.SetActive(false);
        }
    }

    private void UpdateSelected()
    {
        button.interactable = !m_selected;
        SetBGColor();
    }

    private void SetBGColor()
    {
        switch(team)
        {
            case 0:
                background.color = new Color(0.5f, 0.5f, 0.9f);
                break;
            case 1:
                background.color = new Color(0.9f, 0.5f, 0.5f);
                break;
            default:
                background.color = new Color(0.8f, 0.8f, 0.8f);
                break;
        }
    }

}
