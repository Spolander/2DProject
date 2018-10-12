using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseHealth : MonoBehaviour {

    [SerializeField]
    protected int hitPoints;

    public virtual void TakeDamage(int damage = 1)
    {
        hitPoints -= damage;

        if (hitPoints <= 0)
            Death();
    }

    public virtual void Death()
    {
        Destroy(gameObject);
    }


}
