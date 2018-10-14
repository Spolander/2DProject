using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockSpider : Enemy {

    Animator anim;

    float t;

    [SerializeField]
    private Vector2 jumpingVelocity;

    Vector3 originPoint;
    // Use this for initialization

    [SerializeField]
    private LayerMask collisionLayers;


    bool jumping = true;
    bool falling = false;
    bool walking = true;

    [SerializeField]
    private Vector3 groundCheckPosition;


    [SerializeField]
    private float movingSpeed = 10f;

    [SerializeField]
    private float timerMultiplier = 1.5f;

    float fallingTimer = 0;
    float lastGroundedY = 0;

    [SerializeField]
    private float activationDelay = 1f;

    [SerializeField]
    private GameObject particleEffect;

    bool activated = false;
	protected override void Start () {
        base.Start();

        Instantiate(particleEffect, transform.position, Quaternion.identity);
        Invoke("Activate", activationDelay);
      
	}

    void Activate()
    {
        activated = true;
        originPoint = transform.position;
        anim = GetComponent<Animator>();

        direction = startingDirection;
        transform.localScale = new Vector3(direction * -1, 1, 1);

        soundEngine.soundMaster.PlaySound("spiderScream",transform.position);
        Invoke("ActivateAnimator", 0.5f);
    }
    void ActivateAnimator()
    {
        anim.enabled = true;
    }
    private void Update()
    {

        if (activated == false)
            return;


        if (jumping)
        {
            t += Time.deltaTime*timerMultiplier;
            Vector3 newPos = new Vector3();
            newPos.x = originPoint.x + (t * jumpingVelocity.x);
            newPos.y = originPoint.y + (t * jumpingVelocity.y - (0.5f * 9.81f * t * t));

            Vector3 displacement = newPos - transform.position;

            transform.position = newPos;

            if (displacement.y <= 0)
            {
                Grounded();
            }
        }
        else if (walking)
        {

          

            if (Grounded())
            {
                transform.Translate(Vector3.right * direction * movingSpeed * Time.deltaTime);
                wallHit();
            }
               
            else
            {
              
                fallingTimer += Time.deltaTime*timerMultiplier;
                transform.Translate(Vector3.right * direction * movingSpeed * Time.deltaTime);
                Vector3 fallposition = new Vector3(transform.position.x, lastGroundedY - (0.5f * 9.81f * fallingTimer * fallingTimer));
                transform.position = fallposition;

                wallHit();
            }
        }
    }

    bool wallHit()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.TransformPoint(0,0, 0.5f),Vector2.right * direction, 0.8f, collisionLayers);

        if (hit.collider)
        {
                transform.localScale = new Vector3(direction * -1, transform.localScale.y, transform.localScale.z);
                direction *= -1;
                return true;
        }

        return false;
    }

    bool Grounded()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.TransformPoint(groundCheckPosition), Vector2.down, 0.1f, collisionLayers);

        if (hit.collider)
        {
            lastGroundedY = transform.position.y;
            fallingTimer = 0;   
            walking = true;
            anim.enabled = true;
            if (jumping)
                anim.Play("spiderWalk");

            jumping = false;

            return true;
        }

        return false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawRay(transform.TransformPoint(groundCheckPosition), Vector3.down * 0.1f);
        Gizmos.DrawRay(transform.TransformPoint(0, 0, 0.5f), Vector2.right * direction * 0.8f);
    }

    public override void OnDeath()
    {
        soundEngine.soundMaster.PlaySound("spiderDeath",transform.position);
        GetComponent<Collider2D>().enabled = false;
        walking = false;
        jumping = false;
        jumping = false;
        anim.Play("Death");
        StartCoroutine(dissolveAnimation());
    }
}
