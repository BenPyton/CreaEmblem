using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatsUI : MonoBehaviour
{
    [SerializeField] Controller controller;
    [SerializeField] GameObject container;
    [SerializeField] Image portrait;
    [SerializeField] Image weaponIcon;
    [SerializeField] Text heroName;
    [SerializeField] Text heroLevel;
    [SerializeField] Text heroHP;
    [SerializeField] Text heroMaxHP;
    [SerializeField] Text heroAtk;
    [SerializeField] Text heroDef;
    [SerializeField] Text heroRes;
    [SerializeField] Text heroSpd;

    // Start is called before the first frame update
    void Start()
    {
        controller.onHeroClicked.AddListener(UpdateStats);
        UpdateStats(null);

        DataManager.instance.onEndTurn.AddListener((int x) => UpdateStats(null));
        DataManager.instance.onEndGame.AddListener((int x) => gameObject.SetActive(false));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateStats(Hero hero)
    {
        container.SetActive(hero != null);
        if (hero != null)
        {
            GetComponent<Image>().color = hero.team > 0 ? new Color(1.0f, 0.6f, 0.6f) : new Color(0.5f, 0.7f, 1.0f);
            portrait.sprite = hero.data.icon;
            weaponIcon.sprite = hero.data.weapon.icon;
            heroName.text = hero.data.displayName;
            heroLevel.text = hero.level.ToString();
            heroHP.text = hero.life.ToString();
            heroMaxHP.text = hero.maxLife.ToString();
            heroAtk.text = hero.attack.ToString();
            heroDef.text = hero.defense.ToString();
            heroRes.text = hero.resistance.ToString();
            heroSpd.text = hero.speed.ToString();
        }
        else
        {
            GetComponent<Image>().color = new Color(0.8f, 0.8f, 0.8f);
        }
    }

    public void EndTurn()
    {
        DataManager.instance.EndTurn();
    }
}
