using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player : MonoBehaviour {

  
    [SerializeField]
    float movementSpeed = 1;
    Animator anim;
    [SerializeField]
    Vector2 groundCheckStart;

    [SerializeField]
    Vector2 groundCheckEnd;

    Vector2 groundNormal;
    Rigidbody2D rb;

    [SerializeField]
    float jumpForce = 6;

    private float jumpTimer = 0;

    [SerializeField]
    private float jumpWindow = 0.2f;

    private float lastJumpTime = 0;

    bool jumping = false;
    public bool Jumping { set { jumping = value; } }


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

    private float aboutToFallTimer = 0;

    private float fallTimer = 0;

    [SerializeField]
    private float fallLimit = 1f;

    public static Player player;
    public bool PhysicUpdate { get { return !grapplingHook.Initialized; } }


    CapsuleCollider2D cc2d;

    private Vector2 defaultColliderOffset = new Vector2(-0.03237343f, 0.5870146f);
    private Vector2 defaultColliderSize = new Vector2(0.3038788f, 1.174029f);

    private Vector2 jumpColliderOffset = new Vector2(-0.03237343f, 0.3637235f);
    private Vector2 jumpColliderSize = new Vector2(0.3038788f, 0.727447f);

    private Vector2 crouchColliderOffset = new Vector2(-0.03237343f, 0.4222663f);
    private Vector2 crouchColliderSize = new Vector2(0.3038788f, 0.8445324f);

    Vector2 aimVector;

    Pendulum grapplingHook;
    [SerializeField]
    private float grapplingHookDistance = 4;

    public GameObject[] chains;



    [SerializeField]
    private LayerMask grapplingHookLayers;

    [SerializeField]
    private float grapplingHookShootDuration = 0.75f;

    bool canGrappleHook = true;


    Transform grapplingPoint;

    int lastSwingDirection;

    private float grapplingHookStartingTime;

    private float grapplingHookCollisionRadius = 0.4f;

    private Vector2 grapplingHookCollisionPoint = new Vector2(-0.1f,0.25f);

    [SerializeField]
    private bool hookAbilityFound = false;

    Camera cam;

    // Use this for initialization
    void Awake () {
        grapplingPoint = transform.Find("grapplingPoint");
        cc2d = GetComponent<CapsuleCollider2D>();
        player = this;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        grapplingHook = GetComponent<Pendulum>();
        currentBulletSpawnPoint = defaultBulletSpawnPoint;

        if(hookAbilityFound)
        ShowGrapplingHookChains(false);

        cam = Camera.main;
	}
	
	// Update is called once per frame
	void Update () {

        float horizontal = Mathf.Abs(Input.GetAxisRaw("Horizontal")) > 0.2f ? Mathf.Sign(Input.GetAxisRaw("Horizontal")): 0;
        anim.SetFloat("Speed", Mathf.Abs(horizontal) > 0.2f ? 1 : 0);

        SetAimVector();

        if (Input.GetButton("Fire1") && grapplingHook.Initialized == false)
        {
            if (Time.time > lastFireTime + rateOfFire)
            {
                soundEngine.soundMaster.PlaySound("gun1",transform.position);
                lastFireTime = Time.time;
                GameObject g = (GameObject)Instantiate(bulletPrefab, transform.TransformPoint(currentBulletSpawnPoint), Quaternion.identity);
                g.GetComponent<projectile>().Initialize(aimVector, projectileSpeed);
            }
        }

      

        anim.SetLayerWeight(1, Input.GetButton("Fire1") ? 1 : 0);

        if (Input.GetButton("Jump") && canJump)
        {
            if (grapplingHook.Initialized)
            {
                jumpTimer = jumpWindow * 0.5f;
                grapplingHook.Initialized = false;
                canGrappleHook = true;
                rb.isKinematic = false;
                ShowGrapplingHookChains(false);
            }
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
        if (Input.GetButtonDown("Secondary") && canGrappleHook && hookAbilityFound)
        {

            StartCoroutine(ShootGrapplingHook(horizontal));
        }


        if (grapplingHook.Initialized)
        {
            canJump = true;
            Vector3 grapplingHookPoint = grapplingHook.pendulumPoint();

            Vector3 translation = Vector3.zero;
            transform.position = grapplingHookPoint + translation;

            for (int i = 0; i < chains.Length; i++)
            {


                if (i > 0)
                {
                    float lerp = (i * 1.0f) / chains.Length;
                    chains[i].transform.position = Vector3.Lerp(grapplingPoint.position, grapplingHook.Center, lerp);
                }

                else
                    chains[i].transform.position = grapplingPoint.position;
            }
            fallTimer = 0;
            rb.velocity = Vector2.zero;
            transform.localScale = new Vector3(grapplingHook.Direction, 1, 1);
            if (grapplingHook.Direction != lastSwingDirection)
            {
                lastSwingDirection = grapplingHook.Direction;
                anim.Play("Swing", 0, 0);
            }


            if (Time.time > grapplingHookStartingTime + 0.5f)
            {
                GrapplingHookCollision();
            }
        }
        else
        {
            if (horizontal > 0 && transform.localScale.x < 0)
                Flip();
            else if (horizontal < 0 && transform.localScale.x > 0)
                Flip();
        }

        groundCheck();
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
                horizontal = Mathf.Abs(Input.GetAxisRaw("Horizontal")) > 0.2f ? Mathf.Sign(Input.GetAxisRaw("Horizontal")) : 0;
            }


        if (grapplingHook.Initialized == false)
        {

            Vector2 velocity = new Vector2(horizontal * movementSpeed, rb.velocity.y);

            //if(jumpedFromGrapplingHook)
            //velocity += new Vector2(launchDirection.x, 0f) * grapplingHookForce;



            if (anim.GetBool("Grounded"))
            {
                rb.gravityScale = 0;
                velocity = Vector3.ProjectOnPlane(velocity, groundNormal);
                if (horizontal == 0)
                    velocity = Vector2.zero;
            }
            else
                rb.gravityScale = 2;


            rb.velocity = velocity;

            if (fallTimer > fallLimit && anim.GetBool("Grounded") == false)
                rb.velocity += Vector2.down * fallMultiplier;

        }

        Vector3 clampPosition = transform.position;
        if (clampPosition.x < cam.ScreenToWorldPoint(new Vector3(0, 0, 0)).x+0.5f)
            clampPosition.x = cam.ScreenToWorldPoint(new Vector3(0, 0, 0)).x+0.5f;
        else if (clampPosition.x > cam.ScreenToWorldPoint(new Vector3(Screen.width, 0, 0)).x-0.5f)
            clampPosition.x = cam.ScreenToWorldPoint(new Vector3(Screen.width, 0, 0)).x-0.5f;
        transform.position = clampPosition;

    }

    private void GrapplingHookCollision()
    {
        if (Physics2D.OverlapCircle(transform.TransformPoint(grapplingHookCollisionPoint), grapplingHookCollisionRadius, collisionLayers))
        {
            anim.Play("Move");
            ShowGrapplingHookChains(false);
                grapplingHook.Initialized = false;
                rb.isKinematic = false;
        }
    }
    private void Jump()
    {
        groundNormal = new Vector2(0, 1);
        jumpTimer += Time.deltaTime;

        if (jumpTimer >= jumpWindow)
        {
            canJump = false;
            return;
        }

        if (grapplingHook.Initialized)
        {
            grapplingHook.Initialized = false;
            rb.isKinematic = false;
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

    IEnumerator ShootGrapplingHook(float horizontal)
    {
        grapplingHookStartingTime = Time.time;
        soundEngine.soundMaster.PlaySound("hookShoot", transform.position);
        canGrappleHook = false;
        float distance = 0;
        int direction = Mathf.Abs(horizontal) > 0 ? (int)Mathf.Sign(horizontal) : (int)Mathf.Sign(transform.localScale.x);

       

        ShowGrapplingHookChains(true);

        if (grapplingHook.Initialized)
        {
            canJump = false;
            grapplingHook.Initialized = false;
            rb.isKinematic = false;
            canGrappleHook = true;
        }
        while (distance < grapplingHookDistance)
        {
            Vector3 centerPoint = jumping ? transform.TransformPoint(0,0.25f,0) : transform.TransformPoint(0, 1, 0);
            distance += (Time.deltaTime / grapplingHookShootDuration) * grapplingHookDistance;
       
            RaycastHit2D hit = Physics2D.CircleCast(centerPoint, 0.3f, Vector2.right * direction, distance, grapplingHookLayers);
            for (int i = 0; i < chains.Length; i++)
            {
                Vector3 dir = Vector2.right * direction;


                if (i > 0)
                {
                    float lerp = (i * 1.0f) / chains.Length;
                    chains[i].transform.position = Vector3.Lerp(centerPoint, centerPoint + (dir * distance), lerp);
                }

                else
                    chains[i].transform.position = centerPoint;
            }

            if (hit.collider)
            {
                if (hit.collider.tag == "hook")
                {
                    soundEngine.soundMaster.PlaySound("hookLatch", hit.collider.transform.position);
                    anim.Play("Swing", 0, 0);
                    canJump = true;
                    canGrappleHook = true;
                    grapplingHook.InitializePendulum(hit.collider.transform.position, Mathf.Clamp(hit.distance,2, hit.distance), direction);
                    rb.isKinematic = true;
                    lastSwingDirection = (int)Mathf.Sign(transform.localScale.x);
                    yield break;
                }
                canGrappleHook = true;
                ShowGrapplingHookChains(false);
                yield break;
            }

            yield return null;
        }

        ShowGrapplingHookChains(false);

    }

    void ShowGrapplingHookChains(bool show)
    {
        for (int i = 0; i < chains.Length; i++)
            chains[i].SetActive(show);
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
                if(h.collider.isTrigger == false)
                if(h.normal.y < 0.7f)
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

       // Gizmos.DrawWireSphere(transform.TransformPoint(grapplingHookCollisionPoint), grapplingHookCollisionRadius);
        Gizmos.DrawLine(transform.TransformPoint(groundCheckStart), transform.TransformPoint(groundCheckEnd));
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (Time.time > lastJumpTime + 0.2f)
        {
        
            foreach (ContactPoint2D c in collision.contacts)
                if (c.normal.y > 0.7f)
                {
                    groundNormal = c.normal;
                    canGrappleHook = true;
                    anim.SetBool("Grounded", true);
                    canJump = jumping ? false : true;
                    jumping = false;
                }
                   
         }
               
        
    }

    void groundCheck()
    {
        bool grounded = Physics2D.Linecast(transform.TransformPoint(groundCheckStart), transform.TransformPoint(groundCheckEnd), collisionLayers);

        if (grounded)
            aboutToFallTimer = 0;
        else
            aboutToFallTimer += Time.deltaTime;

        if (aboutToFallTimer > 0.05f && anim.GetBool("Grounded"))
        {
            fallTimer = 0.25f;
            groundNormal = Vector3.down;
            anim.SetBool("Grounded", false);
            rb.gravityScale = 2;
        }
            
    }
}
