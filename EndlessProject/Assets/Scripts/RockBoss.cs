using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockBoss : MonoBehaviour {

    [SerializeField]
    private GameObject throwableRock;

    public enum AttackPhase {Idle, RockStorm, GroundStomp, Punch};

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

    GameObject player;

    Animator anim;
	// Use this for initialization
	void Start () {
        anim = GetComponent<Animator>();
        player = Player.player.gameObject;

        ChangePhase(AttackPhase.RockStorm);
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
            if (Time.time > lastRockThrowTime + rockThrowInterval)
            {
                SpawnRock();
            }
        }
        else if (phase == AttackPhase.Punch)
        {

        }
	}
    void ChangePhase(AttackPhase phase)
    {
        this.phase = phase;

        if (this.phase == AttackPhase.Idle)
        {
            anim.CrossFadeInFixedTime("Idle", 0.5f);
        }
        else if (this.phase == AttackPhase.RockStorm)
        {
            anim.CrossFadeInFixedTime("spawnBoulders", 0.5f);
        }
    }


    void SpawnRock()
    {
        lastRockThrowTime = Time.time;

        Vector3 dir = new Vector3(0.2f, -1);

        float lerp = (float)currentRockThrowCount / (float)rockThrowCount;
        Vector3 spawnPos = Camera.main.ScreenToWorldPoint(new Vector3(Mathf.Lerp(-Screen.width*0.25f, Screen.width,lerp), Screen.height));
        spawnPos.z = 0;

        GameObject g = (GameObject)Instantiate(throwableRock, spawnPos, Quaternion.identity);
        g.GetComponent<bouncingProjectile>().Initialize(dir.normalized * rockSpeed);

        currentRockThrowCount++;

        if (currentRockThrowCount >= rockThrowCount)
        {
            currentRockThrowCount = 0;
            ChangePhase(AttackPhase.Idle);
        }
    }

    public void ShakeCamera()
    {
        CameraFollow.playerCamera.ActivateCameraShake(shakeDuration, shakeStrength);
    }
}
