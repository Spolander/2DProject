using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fly : Enemy {

    // Use this for initialization
    [SerializeField]
   private float verticalFidgetSpeed = 1;

    [SerializeField]
    private float fidgetX = 1;

    [SerializeField]
    private float fidgetY = 1;

    [SerializeField]
    private float horizontalFidgetSpeed = 1;

    Camera cam;

    [SerializeField]
    private float speed = 10;

    [SerializeField]
    private float baseHeight = 5;


    [SerializeField]
    private float minimumShootInterval = 1;


    [SerializeField]
    private float maximumShootInterval = 3;

    private float interval;

    private float lastShootingTime;

    Animator anim;

    float startingOffset;

    [SerializeField]
    private GameObject projectile;

    [SerializeField]
    private float projectileSpeed = 5;

    protected override void Start () {
        base.Start();

        anim = GetComponent<Animator>();
        interval = Random.Range(minimumShootInterval, maximumShootInterval);
        cam = Camera.main;

        startingOffset = Random.Range(-5, 5);
 
	}
	
	// Update is called once per frame
	void Update () {

        if (player && !dead)
        {
            Vector3 targetpos = player.transform.position;

            targetpos.y += Mathf.Sin((startingOffset + Time.time) * verticalFidgetSpeed) * fidgetY;
            targetpos.x += Mathf.Sin((startingOffset + Time.time) * horizontalFidgetSpeed) * fidgetX;

            targetpos.y += baseHeight;
            targetpos.y = Mathf.Clamp(targetpos.y, cam.ScreenToWorldPoint(new Vector3(0, 0, 0)).y, cam.ScreenToWorldPoint(new Vector3(0, Screen.height, 0)).y);
            targetpos.x = Mathf.Clamp(targetpos.x, cam.ScreenToWorldPoint(new Vector3(0, 0, 0)).x, cam.ScreenToWorldPoint(new Vector3(Screen.width, 0, 0)).x);

            transform.position = Vector3.MoveTowards(transform.position, targetpos, speed * Time.deltaTime);

            if (player.transform.position.x >= transform.position.x)
            {
                if (direction == -1)
                {
                    direction = 1;
                    transform.localScale = new Vector3(-1, 1, 1);
                }
            }
            else if (player.transform.position.x < transform.position.x)
            {
                if (direction == 1)
                {
                    direction = -1;
                    transform.localScale = new Vector3(1, 1, 1);
                }
            }

            if (Time.time > lastShootingTime + interval)
            {
                interval = Random.Range(minimumShootInterval, maximumShootInterval);
                lastShootingTime = Time.time;
                Shoot();
            }

        }

        else if (dead)
        {
            transform.position += Vector3.down * 5 * Time.deltaTime;
            transform.Rotate(new Vector3(0, 0, Mathf.Sign(transform.localScale.x)), 180f * Time.deltaTime);
        }
	}

    void Shoot()
    {
        anim.Play("Shoot");
    }

    public void LaunchProjectile()
    {

        if (player)
        {
            soundEngine.soundMaster.PlaySound("flyShoot", transform.position);
            GameObject g = (GameObject)Instantiate(projectile, transform.TransformPoint(0.527f, -0.76f, 0), Quaternion.identity);
            g.GetComponent<EnemyProjectile>().Initialize(player.transform.TransformPoint(0,0.7f,0f)- transform.TransformPoint(0.527f, -0.76f, 0), projectileSpeed);

        }
        
    }

    public override void OnDeath()
    {
        soundEngine.soundMaster.PlaySound("flyDeath", transform.position);
        dead = true;
        soundEngine.soundMaster.PlaySound("spiderDeath", transform.position);
        GetComponent<Collider2D>().enabled = false;
        anim.Play("Death");
        StartCoroutine(dissolveAnimation());
    }
}
