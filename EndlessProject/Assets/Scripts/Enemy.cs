﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {



    protected int direction = -1;

    protected Player player;

    [SerializeField]
    protected Material dissolveMaterial;

    [Header("Dissolve properties")]
    [SerializeField]
    protected float dissolveTime;

    [SerializeField]
    protected float dissolveStartingValue = 1;

    [SerializeField]
    protected float deathEffectDelay;

    [SerializeField]
    protected int startingDirection = -1;
    public int StartingDirection { get { return startingDirection; } }
    

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
        Destroy(gameObject);
    }



    protected IEnumerator dissolveAnimation()
    {
        float lerp = 1 - dissolveStartingValue;

        GetComponent<SpriteRenderer>().material = dissolveMaterial;

        Material m = GetComponent<SpriteRenderer>().material;

        yield return new WaitForSeconds(deathEffectDelay);

        while (lerp < 1)
        {
            lerp += Time.deltaTime / dissolveTime;
            m.SetFloat("_DissolvePower", 1-lerp);
            yield return null;
        }

        Destroy(gameObject);
    }
}
