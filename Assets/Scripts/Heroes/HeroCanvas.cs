using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[DefaultExecutionOrder(100)]
public class HeroCanvas : MonoBehaviour
{
    [SerializeField] Hero hero;
    [SerializeField] Image lifebar;
    [SerializeField] Text lifeText;
    [SerializeField] Image weaponIcon;
    [SerializeField] List<Color> playerColors;

    int prevLife = 0;

    // Start is called before the first frame update
    void Start()
    {
        if(hero)
        {
            weaponIcon.sprite = hero.data.weapon.icon;
            prevLife = hero.life;
            lifeText.text = hero.life.ToString();

            Color color = Color.white;
            if(hero.team >= 0 && hero.team < playerColors.Count)
            {
                color = playerColors[hero.team];
            }
            lifebar.color = color;
            lifeText.color = color;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (hero != null && prevLife != hero.life)
        {
            lifebar.fillAmount = hero.life / (float)hero.maxLife;
            lifeText.text = hero.life.ToString();
        }
    }
}
