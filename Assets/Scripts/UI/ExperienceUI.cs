using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[DefaultExecutionOrder(100)]
[RequireComponent(typeof(Animator))]
public class ExperienceUI : MonoBehaviour
{
    [SerializeField] BattleManager battle;
    [SerializeField] GameObject container;
    [SerializeField] GameObject expDisplay;
    [SerializeField] GameObject levelUpDisplay;
    [SerializeField] Image portrait;
    [SerializeField] Image expBar;
    [SerializeField] Text currentExp;
    [SerializeField] Text currentLevelText;
    [SerializeField] Text nextLevelText;
    [SerializeField] Text prevLevelText;
    [SerializeField] Text newLevelText;

    bool continueDisplay = false;

    Animator anim = null;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        battle.onExpGain.AddListener(DisplayExpGain);

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void DisplayExpGain(HeroExpInfo info)
    {
        StartCoroutine(DisplayCoroutine(info));
    }

    IEnumerator DisplayCoroutine(HeroExpInfo info)
    {
        container.SetActive(true);
        expDisplay.SetActive(true);
        levelUpDisplay.SetActive(false);

        portrait.sprite = info.hero.data.icon;
        currentExp.text = info.newExp.ToString();
        expBar.fillAmount = info.newExp / (float)info.hero.nextLevelExp;
        currentLevelText.text = info.newLevel.ToString();
        nextLevelText.text = (info.newLevel + 1).ToString();
        prevLevelText.text = info.prevLevel.ToString();
        newLevelText.text = info.newLevel.ToString();

        anim.SetTrigger("DisplayGain");

        continueDisplay = false;
        yield return new WaitUntil(() => continueDisplay);

        expDisplay.SetActive(false);

        if (info.newLevel != info.prevLevel)
        {
            levelUpDisplay.SetActive(true);

            anim.SetTrigger("DisplayLevelUp");

            continueDisplay = false;
            yield return new WaitUntil(() => continueDisplay);

            levelUpDisplay.SetActive(false);
        }

        container.SetActive(false);
        battle.ContinueBattle();
    }

    // Can be called from animation
    public void ContinueDisplay()
    {
        continueDisplay = true;
    }
}
