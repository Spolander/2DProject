﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockBoss : Enemy {

    [SerializeField]
    private GameObject throwableRock;

    [SerializeField]
    private GameObject debrisObject;

    public enum AttackPhase {Idle, RockStorm, ThrowRocks, Punch};

    private AttackPhase phase;

    [SerializeField]
    private float rockThrowInterval = 3;
    float lastRockThrowTime;
    [SerializeField]
    private float rockSpeed = 10;

    [SerializeField]
    private int rockThrowCount = 10;

    private int currentRockThrowCount = 0;

    [SerializeField]
    private float shakeDuration;
    [SerializeField]
    private float shakeStrength;

    public enum AttackCycle {OnePunch = 1, ThrowDebris, MultiplePunches, RockStorm};

    [SerializeField]
    private AttackCycle cycle;
    public AttackCycle Cycle { set { cycle = value; } get { return cycle; } }

    Animator anim;

    [SerializeField]
    private float timerBetweenAttacks = 1;

    private float lastAttackTime;
    public float LastAttackTime { set { lastAttackTime = value; } }

    private float comboStartTime;

    [SerializeField]
    private float comboDuration = 7;

    [SerializeField]
    private Transform debrisParent;

    [SerializeField]
    private int debrisCount = 5;

    List<Transform> debris;

    [SerializeField]
    private GameObject dirtParticle;

    [SerializeField]
    private Transform rightHand;

    [SerializeField]
    private Transform leftHand;

    // Use this for initialization
    protected override void Start()
    {
        base.Start();
        anim = GetComponent<Animator>();
        debris = new List<Transform>();


        ChangePhase(AttackPhase.Idle);

        Invoke("StartAttacking", 1);
    }

    //Start attack cycle
    void StartAttacking()
    {
        cycle = AttackCycle.OnePunch;
        ChangePhase(AttackPhase.Punch);
    }
    // Update is called once per frame
    void Update () {
        if (phase == AttackPhase.Idle)
        {
            //don't attack
        }
        else if (phase == AttackPhase.RockStorm)
        {
            if (player == null)
            {
                ChangePhase(AttackPhase.Idle);
            }
            if (Time.time > lastRockThrowTime + rockThrowInterval && anim.GetCurrentAnimatorStateInfo(0).IsName("spawnBoulders") && anim.IsInTransition(0) == false)
            {
                SpawnRock();
            }
        }
        else if (phase == AttackPhase.Punch)
        {
            if (anim.GetCurrentAnimatorStateInfo(0).IsName("Idle") && anim.IsInTransition(0) == false && Time.time > lastAttackTime + timerBetweenAttacks)
            {
                if (cycle == AttackCycle.OnePunch)
                {
                    if (Random.value >= 0.5f)
                        anim.CrossFadeInFixedTime("punch", 0.5f);
                    else
                        anim.CrossFadeInFixedTime("punch2", 0.5f);
                }

                else if (cycle == AttackCycle.MultiplePunches)
                {
                    print("yea yeay ea");
                    comboStartTime = Time.time;
                    anim.SetBool("punching", true);
                    anim.CrossFadeInFixedTime("punch", 0.5f);
                }
            }

            if (Time.time > comboStartTime + comboDuration)
            {
                anim.SetBool("punching", false);
            }

            if (cycle == AttackCycle.ThrowDebris)
            {
                ChangePhase(AttackPhase.ThrowRocks);
            }
        }

       
	}
    public void ChangePhase(AttackPhase phase)
    {

        if (this.enabled == false)
            return;
        this.phase = phase;

        if (this.phase == AttackPhase.Idle)
        {
            anim.CrossFadeInFixedTime("Idle", 0.5f);
        }
        else if (this.phase == AttackPhase.RockStorm)
        {
            cycle = AttackCycle.RockStorm;
            anim.SetBool("punching", false);
            anim.SetBool("rockstorm", true);
        }
        else if (this.phase == AttackPhase.ThrowRocks)
        {
            anim.CrossFadeInFixedTime("throwDebris", 0.5f);
        }
    }


    void SpawnRock()
    {
        lastRockThrowTime = Time.time;

        Vector3 dir = new Vector3(0.2f, -1);
        float lerp = (float)currentRockThrowCount / (float)rockThrowCount;
        Vector3 spawnPos = Camera.main.ScreenToWorldPoint(new Vector3(Mathf.Lerp(-Screen.width*0.05f, Screen.width,lerp), Screen.height));
        spawnPos.z = 0;


        GameObject g = (GameObject)Instantiate(throwableRock, spawnPos, Quaternion.identity);
        g.GetComponent<bouncingProjectile>().Initialize(dir.normalized * rockSpeed);

        currentRockThrowCount++;

        if (currentRockThrowCount >= rockThrowCount)
        {
            anim.SetBool("rockstorm", false);
            currentRockThrowCount = 0;
            cycle = AttackCycle.OnePunch;
            ChangePhase(AttackPhase.Punch);
        }
    }

    public void ShakeCamera()
    {
        CameraFollow.playerCamera.ActivateCameraShake(shakeDuration, shakeStrength);
    }

    public void SpawnDebris()
    {
       
        for (int i = 0; i < debrisCount; i++)
        {
            GameObject d = (GameObject)Instantiate(debrisObject, debrisParent.position, Quaternion.identity);
            d.transform.SetParent(debrisParent);
            d.transform.localPosition = Vector3.zero;
            d.GetComponent<bouncingProjectile>().enabled = false;
            debris.Add(d.transform);
        }

    }

    public void ThrowDebris()
    {
        float xVariance = Random.Range(-3, 3);
        for (int i = 0; i < debris.Count; i++)
        {
            debris[i].transform.SetParent(null);
            debris[i].GetComponent<bouncingProjectile>().enabled = true;
            debris[i].GetComponent<bouncingProjectile>().Initialize(new Vector2(-10-xVariance + i*2, 3+Random.Range(0,3.5f)));
           
        }

        debris.Clear();
    }

    public void SpawnDirt()
    {
        Instantiate(dirtParticle, rightHand.TransformPoint(0.12f,-0.12f,0), Quaternion.identity);
    }
    public void SpawnDirtLeft()
    {
        Instantiate(dirtParticle,leftHand.TransformPoint(0.12f, -0.12f, 0), Quaternion.identity);
    }

    public void PlayDebrisSound()
    {
        soundEngine.soundMaster.PlaySound("bossAttackDebris", transform.position);
    }

    public void PlayAttackSound()
    {
        soundEngine.soundMaster.PlaySound("bossAttackShort", transform.position);
    }

    public void PlayImpactSound()
    {
        soundEngine.soundMaster.PlaySound("bossImpact", transform.position);
    }

    public void ClearDebris()
    {
        if(debris != null)
        if (debris.Count > 0)
        {
            for (int i = 0; i < debris.Count; i++)
                Destroy(debris[i].gameObject);
        }

        debris.Clear();
    }

  
}
