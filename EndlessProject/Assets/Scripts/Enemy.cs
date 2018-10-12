using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {



    protected int direction = 1;

    protected Player player;

    protected virtual void Start()
    {
        player = Player.player;
    }
    protected virtual void Flip()
    {
        direction *= -1;
        transform.localScale = new Vector3(transform.localScale.x * direction, transform.localScale.y, transform.localScale.z);
    }

    public virtual void OnDeath()
    {

    }
}
