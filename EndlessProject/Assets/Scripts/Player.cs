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

   
    private Vector2 defaultBulletSpawnPoint = new Vector2(1.1f, 0.9f);
    private Vector2 jumpBulletSpawnPoint = new Vector2(0.108f, 0.376f);
    private Vector2 upwardsBulletSpawnPoint = new Vector2(0.75f, 1.45f);
    private Vector2 straightUpBulletSpawnPoint = new Vector2(0.1f, 1.65f);
    private Vector2 crouchBulletSpawnPoint = new Vector2(1.1f, 0.65f);
    private Vector2 shootDownBulletSpawnPoint = new Vector2(0.85f, 0.45f);

    private Vector2 currentBulletSpawnPoint;

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


    CapsuleCollider2D cc2d;

    private Vector2 defaultColliderOffset = new Vector2(-0.03237343f, 0.5870146f);
    private Vector2 defaultColliderSize = new Vector2(0.3038788f, 1.174029f);

    private Vector2 jumpColliderOffset = new Vector2(-0.03237343f, 0.3637235f);
    private Vector2 jumpColliderSize = new Vector2(0.3038788f, 0.727447f);

    private Vector2 crouchColliderOffset = new Vector2(-0.03237343f, 0.4222663f);
    private Vector2 crouchColliderSize = new Vector2(0.3038788f, 0.8445324f);

    Vector2 aimVector;
    // Use this for initialization
    void Awake () {
        cc2d = GetComponent<CapsuleCollider2D>();
        player = this;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        currentBulletSpawnPoint = defaultBulletSpawnPoint;
	}
	
	// Update is called once per frame
	void Update () {

        float horizontal = Input.GetAxisRaw("Horizontal");
        anim.SetFloat("Speed", Mathf.Abs(horizontal) > 0.2f ? 1 : 0);

        SetAimVector();

        if (Input.GetButton("Fire1"))
        {
            if (Time.time > lastFireTime + rateOfFire)
            {
                soundEngine.soundMaster.PlaySound("gun1");
                lastFireTime = Time.time;
                GameObject g = (GameObject)Instantiate(bulletPrefab, transform.TransformPoint(currentBulletSpawnPoint), Quaternion.identity);
                g.GetComponent<projectile>().Initialize(aimVector, projectileSpeed);
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
        {
            fallTimer = 0;

            if (Input.GetAxisRaw("Vertical") >= 0)
            {


                cc2d.size = defaultColliderSize;
                cc2d.offset = defaultColliderOffset;
            }
            else if(Mathf.Abs(Input.GetAxisRaw("Horizontal")) < 0.2f)
            {
                cc2d.size = crouchColliderSize;
                cc2d.offset = crouchColliderOffset;
            }
        }
     }

        void SetAimVector()
    {
        aimVector = Vector2.zero;
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        if(anim.GetBool("Grounded"))
            currentBulletSpawnPoint = defaultBulletSpawnPoint;

        if (Mathf.Abs(horizontal) > 0.2f)
        {
            aimVector.x = Mathf.Sign(horizontal);

            if (vertical > 0.2f)
                aimVector.y = 1;
            else if (vertical < -0.2f)
            {
                aimVector.y = -1;
                currentBulletSpawnPoint = shootDownBulletSpawnPoint;
            }
                
        }
        else
        {
            aimVector.x = Mathf.Sign(transform.localScale.x);

            if (vertical > 0.2f)
            {
                aimVector.y = 1;
                aimVector.x = 0;
                currentBulletSpawnPoint = straightUpBulletSpawnPoint;
            }
            else if (vertical < -0.2f)
            {
                if (anim.GetBool("Grounded"))
                    currentBulletSpawnPoint = crouchBulletSpawnPoint;
                else
                {
                    aimVector.x = 0;
                    aimVector.y = -1;
                }
                anim.SetFloat("aimVectorY", -1);
                return;
            }


        }
        anim.SetFloat("aimVectorY", aimVector.y);

        if (aimVector.y > 0.2f)
        {
            if (Mathf.Abs(aimVector.x) > 0.2f)
                currentBulletSpawnPoint = upwardsBulletSpawnPoint;
        }

        if (anim.GetBool("Grounded") == false)
            currentBulletSpawnPoint = jumpBulletSpawnPoint;

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

        currentBulletSpawnPoint = jumpBulletSpawnPoint;
        cc2d.size = jumpColliderSize;
        cc2d.offset = jumpColliderOffset;
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
}
                    anim.SetBool("Grounded", true);
                    canJump = jumping ? false:true ;
                    jumping = false;
                }
               
        
    }
}
