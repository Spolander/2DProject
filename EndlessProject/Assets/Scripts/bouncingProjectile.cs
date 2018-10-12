using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bouncingProjectile : MonoBehaviour {

    int direction = -1;

    Vector2 originPoint;

    float t;

    [SerializeField]
    private Vector2 velocity;

    float angle;

    [SerializeField]
    Vector2 bottomHitDetectionPoint;

    [SerializeField]
    Vector2 sideHitDetectionPoint;

    [SerializeField]
    private float hitDetectionDistance = 0.1f;

    [SerializeField]
    private LayerMask collisionLayers;

    [SerializeField]
    private float rotateSpeed = 180f;

    [Range(0,1)]
    [SerializeField]
    float forceLossPercentage = 0.25f;

    [SerializeField]
    private GameObject explosionPrefab;

    [SerializeField]
    private GameObject[] shrapnel;

    [SerializeField]
    private float shrapnelSpeed = 5;

    [SerializeField]
    private int shrapnelCount = 4;

    public void Initialize(Vector2 velocity)
    {
        this.velocity = velocity;
        originPoint = transform.position;
        angle = Mathf.Atan(velocity.y / velocity.x) * Mathf.Rad2Deg;
    }
    // Use this for initialization

    // Update is called once per frame
    void Update ()
    {
        t += Time.deltaTime;
        float xPosition = originPoint.x + velocity.x * t;
        float yPosition =originPoint.y + ((velocity.y * t) - (0.5f * 9.81f * t*t));

        float displacementX = xPosition - transform.position.x;
        float displacementY = yPosition - transform.position.y;
        transform.position = new Vector3(xPosition, yPosition, transform.position.z);
        transform.Rotate(new Vector3(0, 0, rotateSpeed * Time.deltaTime*-Mathf.Sign(displacementX)));

        HitDetection(displacementX, displacementY);
	}

    void HitDetection(float xDisplacement, float yDisplacement)
    {
        if (yDisplacement < 0)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position + new Vector3(bottomHitDetectionPoint.x, bottomHitDetectionPoint.y, 0), Vector3.down, hitDetectionDistance, collisionLayers);

            if (hit.collider)
            {
                t = 0;
                originPoint = transform.position;
            }
        }

        RaycastHit2D hHit = Physics2D.Raycast(transform.position + new Vector3(sideHitDetectionPoint.x*Mathf.Sign(xDisplacement), sideHitDetectionPoint.y, 0), Vector3.down, hitDetectionDistance, collisionLayers);

        if (hHit.collider)
        {
            t = 0;
            originPoint = transform.position;
            velocity.x *= -1;
            velocity.Scale(Vector2.one * (1-forceLossPercentage));
        }


    }

    public void Explode()
    {
        if (explosionPrefab)
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);

        soundEngine.soundMaster.PlaySound("rockExplosion");
        for (int i = 0; i < shrapnelCount; i++)
        {
            Vector3 direction =  Quaternion.AngleAxis((360 / shrapnelCount) * i, Vector3.forward) * Vector3.up;
            GameObject g = (GameObject)Instantiate(shrapnel[0], transform.position, Quaternion.identity);
            g.GetComponent<EnemyProjectile>().Initialize(direction, shrapnelSpeed);
        }

        Destroy(gameObject);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawRay(transform.position + new Vector3(bottomHitDetectionPoint.x, bottomHitDetectionPoint.y,0), Vector3.down * hitDetectionDistance);
        Gizmos.DrawRay(transform.position + new Vector3(sideHitDetectionPoint.x, sideHitDetectionPoint.y,0), Vector2.right * hitDetectionDistance);
    }
}
