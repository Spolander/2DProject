using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseHealth : MonoBehaviour {

    [SerializeField]
    protected int hitPoints;

    protected int maxHitPoints;

    private void Awake()
    {
        maxHitPoints = hitPoints;
    }

    public virtual void TakeDamage(int damage = 1)
    {
        if (hitPoints <= 0)
            return;

        hitPoints -= damage;

        if (hitPoints <= 0)
            Death();
    }

    public virtual void Death()
    {
        Destroy(gameObject);
    }

    public virtual void Heal(int amount)
    {
        hitPoints += amount;

        if (hitPoints > maxHitPoints)
            hitPoints = maxHitPoints;
    }


}
