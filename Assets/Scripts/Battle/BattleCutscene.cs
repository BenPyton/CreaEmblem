using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Animator))]
public class BattleCutscene : MonoBehaviour
{
    [SerializeField] BattleManager battle;
    [SerializeField] Image rightHero;
    [SerializeField] Image leftHero;

    Animator anim;

    Hero attacker = null;
    Hero attacked = null;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();

        battle.onStartBattle.AddListener((Hero _attacker, Hero _attacked) =>
        {
            rightHero.sprite = _attacker.team != 0 ? _attacker.data.spriteSheet[0, 0] : _attacked.data.spriteSheet[0, 0];
            leftHero.sprite = _attacker.team == 0 ? _attacker.data.spriteSheet[0, 0] : _attacked.data.spriteSheet[0, 0];

            anim.SetTrigger("StartBattle");
        });

        battle.onEndBattle.AddListener(() =>
        {
            anim.SetTrigger("EndBattle");
        });

        battle.onHeroAttack.AddListener((Hero _attacker, Hero _attacked) =>
        {
            attacker = _attacker;
            attacked = _attacked;
            anim.SetTrigger(_attacker.team == 0 ? "LeftAttack" : "RightAttack");
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ContinueBattle()
    {
        battle.ContinueBattle();
    }

    public void Attack()
    {
        attacked.GetDamage(attacker);
    }
}
