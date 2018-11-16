using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rockEnemy : Enemy {

    Animator anim;

    [SerializeField]
    private GameObject throwableRock;

    [SerializeField]
    private Vector2 throwVelocity;

    [SerializeField]
    private float throwInterval;

    private float lastThrowTime;

    private Transform boulderPoint;

    private GameObject holdingBoulder;
    public GameObject Boulder { get { return holdingBoulder; } }

    Camera cam;
	// Use this for initialization
	protected override void Start ()
    {
        base.Start();
        cam = Camera.main;
        anim = GetComponent<Animator>();
        boulderPoint = transform.Find("boulderPoint");
        lastThrowTime = Time.time-throwInterval;
	}
	
	// Update is called once per frame
	void Update () {

        CheckOutOfScreen();

        if (Time.time > lastThrowTime + throwInterval)
        {
            lastThrowTime = Time.time;
            anim.Play("throw");
        }

        if (player == null)
            return;

        if (player.transform.position.x > transform.position.x && transform.localScale.x > 0)
            Flip();
        else if (player.transform.position.x <= transform.position.x && transform.localScale.x < 0)
            Flip();
	}

    public void SpawnBoulder()
    {
        GameObject g = (GameObject)Instantiate(throwableRock, boulderPoint.position, Quaternion.identity);
        g.transform.SetParent(boulderPoint);
        holdingBoulder = g;
        holdingBoulder.GetComponent<bouncingProjectile>().enabled = false;
    }

    public void ThrowBoulder()
    {
        if (holdingBoulder == null)
            return;

        holdingBoulder.GetComponent<bouncingProjectile>().enabled = true;
        holdingBoulder.transform.SetParent(null);
        holdingBoulder.GetComponent<bouncingProjectile>().Initialize(new Vector2(throwVelocity.x * -1*Mathf.Sign(transform.localScale.x), throwVelocity.y));
        holdingBoulder = null;
    }

    public override void OnDeath()
    {
        if (holdingBoulder)
        {
            GameObject g = holdingBoulder;
            g.transform.SetParent(null);
            g.GetComponent<bouncingProjectile>().Initialize(new Vector2(0, 3));
            g.GetComponent<bouncingProjectile>().enabled = true;
        }

        if (Random.value < Enemy.healthDropChance)
            Instantiate(healthPickUp, transform.position, Quaternion.identity);

        anim.Play("Death");
        soundEngine.soundMaster.PlaySound("boulderHit", transform.position);
        StartCoroutine(dissolveAnimation());
        this.enabled = false;
    }

    void  CheckOutOfScreen()
    {
    

        if (transform.position.x < cam.ScreenToWorldPoint(new Vector3(0, 0, 0)).x - 1)
            this.enabled = false;
        else if (transform.position.x > cam.ScreenToWorldPoint(new Vector3(Screen.width, 0, 0)).x + 1)
            this.enabled = false;
    
    }


}
