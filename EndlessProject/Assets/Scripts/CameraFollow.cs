using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraFollow : MonoBehaviour {

    public enum FollowMode {Horizontal, Both};
    public FollowMode _followMode;

    [SerializeField]
    private bool constantlyFollowing = false;
    Transform target;

    [SerializeField]
    private Vector3 followOffset;

    [SerializeField]
    private float lookAheadTime;

    Vector3 playerLastPosition;

    Vector3 velocity;

    [SerializeField]
    private float dampTime = 0.2f;

    [SerializeField]
    private Vector2 minimumPosition;

    [SerializeField]
    private Vector2 maximumPosition;

    [SerializeField]
    private bool limitPosition;

    private float shakeDuration = 0;

    private float shakeStrength;

    private float decreaseFactor = 1f;

    Vector3 targetPosition;

    public static CameraFollow playerCamera;

    public Transform alternateFollowTarget;

    private float preShakeY;

    void Awake()
    {
        playerCamera = this;
    }
	// Use this for initialization
	void Start () {
        target = Player.player.transform;
        playerLastPosition = target.position;
        preShakeY = transform.position.y;
	}
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        if (Input.GetKeyDown(KeyCode.R))
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        if (target == null && alternateFollowTarget == null)
            return;

        if (Player.player.PhysicUpdate == false)
            return;

        if (alternateFollowTarget != null)
            target = alternateFollowTarget;
        else target = Player.player.transform;

        Vector3 playerDeltaPosition = target.position - playerLastPosition;

        targetPosition = target.position + followOffset+ playerDeltaPosition * lookAheadTime;

        if (_followMode == FollowMode.Horizontal)
            targetPosition.y = transform.position.y;

        targetPosition.z = -10;
        CameraShake();

        if (constantlyFollowing == false && targetPosition.x > transform.position.x)
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, dampTime);
        else
        {
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, dampTime);
        }
            

       playerLastPosition = target.transform.position;
        if (limitPosition)
            LimitPosition();
      
	}

    private void LateUpdate()
    {

        if (target == null && alternateFollowTarget == null)
            return;

        if (Player.player.PhysicUpdate == true)
            return;

        if (alternateFollowTarget != null)
            target = alternateFollowTarget;
        else target = Player.player.transform;

        Vector3 playerDeltaPosition = target.position - playerLastPosition;

        targetPosition = target.position + followOffset + playerDeltaPosition * lookAheadTime;

        if (_followMode == FollowMode.Horizontal)
            targetPosition.y = transform.position.y;

        targetPosition.z = -10;

        CameraShake();


        if (constantlyFollowing == false && targetPosition.x > transform.position.x)
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, dampTime);
        else
        {
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, dampTime);
        }


        playerLastPosition = target.transform.position;

        if (limitPosition)
            LimitPosition();
    }

    void CameraShake()
    {
        if (shakeDuration > 0f)
        {
            Vector2 v = Random.insideUnitCircle * shakeStrength;

            targetPosition += new Vector3(v.x, v.y, 0);

            shakeDuration -= Time.deltaTime * decreaseFactor;
        }
        else
            targetPosition.y = preShakeY;
        
      
    }
    void LimitPosition()
    {
        Vector3 pos = transform.position;
        if (pos.x < minimumPosition.x)
            pos.x = minimumPosition.x;
        else if (pos.x > maximumPosition.x)
            pos.x = maximumPosition.x;

        if (pos.y < minimumPosition.y)
            pos.y = minimumPosition.y;
        else if (pos.y > maximumPosition.y)
            pos.y = maximumPosition.y;

        transform.position = pos;
    }

    public void ActivateCameraShake(float duration, float strength)
    {
        preShakeY = transform.position.y;
        this.shakeDuration = duration;
        this.shakeStrength = strength;
    }
}
