using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player : MonoBehaviour {

    float gravity = 0;
    [SerializeField]
    float movementSpeed = 1;
    Animator anim;

    Rigidbody2D rb;

    [SerializeField]
    float jumpForce = 6;

    private float jumpTimer = 0;

    [SerializeField]
    private float jumpWindow = 0.2f;

    private float lastJumpTime = 0;

    bool jumping = false;
    public bool Jumping { set { jumping = value; } }

    float flippingSpeed = 720f;

    bool canJump = false;

    [SerializeField]
    private Vector2[] raycastPoints;

    [SerializeField]
    private float wallCheckDistance = 0.2f;

    public LayerMask collisionLayers;

    [SerializeField]
    public GameObject bulletPrefab;

    [SerializeField]
    private Vector2 defaultBulletSpawnPoint = new Vector2(0.5f,0.5f);

    [SerializeField]
    private float rateOfFire = 0.2f;

    private float lastFireTime = 0;

    [SerializeField]
    private float projectileSpeed = 15;

    [SerializeField]
    private float fallMultiplier = 1.5f;

    private float fallTimer = 0;

    [SerializeField]
    private float fallLimit = 1f;

    public static Player player;
	// Use this for initialization
	void Awake () {
        player = this;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {

        float horizontal = Input.GetAxisRaw("Horizontal");
        anim.SetFloat("Speed", Mathf.Abs(horizontal) > 0.2f ? 1 : 0);

        if (Input.GetButton("Fire1"))
        {
            if (Time.time > lastFireTime + rateOfFire)
            {
                lastFireTime = Time.time;
                GameObject g = (GameObject)Instantiate(bulletPrefab, transform.TransformPoint(defaultBulletSpawnPoint), Quaternion.identity);
                g.GetComponent<projectile>().Initialize(transform.right*Mathf.Sign(transform.localScale.x), projectileSpeed);
            }
        }

        if (horizontal > 0 && transform.localScale.x < 0)
            Flip();
        else if (horizontal < 0 && transform.localScale.x > 0)
            Flip();

        anim.SetLayerWeight(1, Input.GetButton("Fire1") ? 1 : 0);

        if (Input.GetButton("Jump") && canJump)
        {
            Jump();
        }
        else if (Input.GetButtonUp("Jump"))
        {
            canJump = false;
            jumpTimer = 0;
        }


        if (anim.GetBool("Grounded") == false)
        {
            fallTimer += Time.deltaTime;
        }
        else
            fallTimer = 0;
      
    }

  
    private void FixedUpdate()
    {
        float horizontal = 0;
        if (canMoveHorizontally())
        {
         horizontal = Input.GetAxisRaw("Horizontal");
        }
       

        Vector2 velocity = new Vector2(horizontal * movementSpeed, rb.velocity.y);

        rb.velocity = velocity;

        if (fallTimer > fallLimit)
            rb.velocity += Vector2.down * fallMultiplier;
    
    }

    private void Jump()
    {
        jumpTimer += Time.deltaTime;

        if (jumpTimer >= jumpWindow)
        {
            canJump = false;
            return;
        }
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        anim.SetBool("Grounded", false);
        anim.Play("flip");
        lastJumpTime = Time.time;
        jumping = true;

       
    }

    void Flip()
    {
        float scale = transform.localScale.x > 0 ? -1 : 1;
        transform.localScale = new Vector3(scale, 1, 1);
    }

    bool canMoveHorizontally()
    {
        for (int i = 0; i < raycastPoints.Length; i++)
        {
            RaycastHit2D h = Physics2D.Raycast(transform.TransformPoint(raycastPoints[i]), Vector2.right * Mathf.Sign(transform.localScale.x), wallCheckDistance, collisionLayers);
            if (h.collider)
                return false;
        }
        return true;
    }
    private void OnDrawGizmos()
    {
        if (raycastPoints.Length > 0)
        {
            for (int i = 0; i < raycastPoints.Length; i++)
                Gizmos.DrawRay(transform.TransformPoint(raycastPoints[i]), Vector2.right * Mathf.Sign(transform.localScale.x) * wallCheckDistance);
        }
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (Time.time > lastJumpTime + 0.2f)
        {
            foreach (ContactPoint2D c in collision.contacts)
                if (c.normal.y > 0.7f)
                {
                    anim.SetBool("Grounded", true);
                    canJump = jumping ? false:true ;
                    jumping = false;
                }
               
        }
    }
}
