using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraFollow : MonoBehaviour {

    public enum FollowMode {Horizontal, Both};
    public FollowMode _followMode;
    Transform target;

    [SerializeField]
    private Vector2 followOffset;

    [SerializeField]
    private float lookAheadTime;

    Vector3 playerLastPosition;

    Vector3 velocity;

    [SerializeField]
    private float dampTime = 0.2f;
	// Use this for initialization
	void Start () {
        target = Player.player.transform;
        playerLastPosition = target.position;
	}
	
	// Update is called once per frame
	void LateUpdate ()
    {
        if (Input.GetKeyDown(KeyCode.R))
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        if (target == null)
            return;

        Vector3 playerDeltaPosition = target.position - playerLastPosition;

        Vector3 targetPosition = target.position + playerDeltaPosition * lookAheadTime;

        if (_followMode == FollowMode.Horizontal)
            targetPosition.y = transform.position.y;

        targetPosition.z = -10;

       // if(targetPosition.x > transform.position.x)
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, dampTime);

        playerLastPosition = target.transform.position;

      
	}
}
