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
    [SerializeField] Image frame;
    [SerializeField] Image weaponIcon;
    [SerializeField] Image highlightImage;
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


    private bool m_highligthed = false;
    public bool highligthed
    {
        get { return m_highligthed; }
        set { m_highligthed = value; UpdateHighlighted(); }
    }

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
            portrait.gameObject.SetActive(true);
            HeroData data = DataManager.instance.heroes.GetHeroDataFromInfo(info);
            portrait.sprite = data.icon;
            weaponIcon.sprite = data.weapon.icon;
            heroName.text = data.displayName;
            heroLvl.text = info.level.ToString();
        }
        else
        {
            container.SetActive(false);
            portrait.gameObject.SetActive(false);
        }
    }

    private void UpdateSelected()
    {
        button.interactable = !m_selected;
        portrait.color = m_selected ? Color.grey : Color.white;
        SetBGColor();
    }

    private void UpdateHighlighted()
    {
        highlightImage.gameObject.SetActive(m_highligthed);
    }

    private void SetBGColor()
    {
        switch(team)
        {
            case 0:
                background.color = new Color(0.6f, 0.6f, 1.0f);
                frame.color = new Color(0.6f, 0.6f, 1.0f);
                break;
            case 1:
                background.color = new Color(1.0f, 0.6f, 0.6f);
                frame.color = new Color(1.0f, 0.6f, 0.6f);
                break;
            default:
                background.color = new Color(1.0f, 1.0f, 1.0f);
                frame.color = new Color(1.0f, 1.0f, 1.0f);
                break;
        }
    }

}
