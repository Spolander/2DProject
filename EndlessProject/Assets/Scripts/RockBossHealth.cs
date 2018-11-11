using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockBossHealth : EnemyHealth {

    int quarterHealth;
    int lastHp;
    int startingHP;
    protected override void Start()
    {
        base.Start();
        startingHP = hitPoints;
        lastHp = hitPoints;
        quarterHealth = hitPoints / 4;
    }

    public override void TakeDamage(int damage = 1)
    {

        base.TakeDamage();

        if (hitPoints <= lastHp - quarterHealth)
        {
            lastHp = hitPoints;
            GetComponent<RockBoss>().ChangePhase(RockBoss.AttackPhase.RockStorm);
        }
        if (hitPoints <= startingHP / 3 && GetComponent<Animator>().GetFloat("speedMultiplier") == 1)
        {
            GetComponent<Animator>().SetFloat("speedMultiplier", 1.1f);
        }


    }
}
