using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatsUI : MonoBehaviour
{
    [SerializeField] Controller controller;
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
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateStats(Hero hero)
    {
        if (hero != null)
        {
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
    }
}
