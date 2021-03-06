﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bouncingProjectile : MonoBehaviour {


    Vector2 originPoint;

    float t;

    [SerializeField]
    private Vector2 velocity;


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

    [SerializeField]
    private bool shrapnelOn = true;

    [SerializeField]
    private bool destroyOnImpact = false;

    [SerializeField]
    private bool physicsEnabled = true;

    Camera cam;

    public void Initialize(Vector2 velocity)
    {
        cam = Camera.main;
        this.velocity = velocity;
        originPoint = transform.position;

    }
    // Use this for initialization

    // Update is called once per frame
    void Update ()
    {

        CheckOutOfScreen();
        t += Time.deltaTime;
        float xPosition = originPoint.x + velocity.x * t;

      
            float yPosition;
        if (physicsEnabled)
            yPosition = originPoint.y + ((velocity.y * t) - (0.5f * 9.81f * t * t));
        else
            yPosition = originPoint.y + velocity.y * t;

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
                if (destroyOnImpact)
                {
                    Explode();
                    return;
                }
            }
        }

        RaycastHit2D hHit = Physics2D.Raycast(transform.position + new Vector3(sideHitDetectionPoint.x*Mathf.Sign(xDisplacement), sideHitDetectionPoint.y, 0), Vector3.down, hitDetectionDistance, collisionLayers);

        if (hHit.collider)
        {
            t = 0;
            originPoint = transform.position;
            velocity.x *= -1;
            velocity.Scale(Vector2.one * (1-forceLossPercentage));

            if (destroyOnImpact)
            {
                Explode();
                return;
            }
        }


    }

    public void Explode()
    {
        if (explosionPrefab)
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);

        soundEngine.soundMaster.PlaySound("rockExplosion",transform.position);

        if (shrapnelOn)
        {
            for (int i = 0; i < shrapnelCount; i++)
            {
                Vector3 direction = Quaternion.AngleAxis((360 / shrapnelCount) * i, Vector3.forward) * Vector3.up;
                GameObject g = (GameObject)Instantiate(shrapnel[Random.Range(0, shrapnel.Length)], transform.position, Quaternion.identity);
                g.GetComponent<EnemyProjectile>().Initialize(direction, shrapnelSpeed);
                Destroy(g, 10);
            }
        }

        Destroy(gameObject);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawRay(transform.position + new Vector3(bottomHitDetectionPoint.x, bottomHitDetectionPoint.y,0), Vector3.down * hitDetectionDistance);
        Gizmos.DrawRay(transform.position + new Vector3(sideHitDetectionPoint.x, sideHitDetectionPoint.y,0), Vector2.right * hitDetectionDistance);
    }

    void CheckOutOfScreen()
    {


        if (transform.position.x < cam.ScreenToWorldPoint(new Vector3(0, 0, 0)).x - 1)
            Destroy(gameObject);
        else if (transform.position.x > cam.ScreenToWorldPoint(new Vector3(Screen.width, 0, 0)).x + 1)
            Destroy(gameObject);
        else if (transform.position.y < cam.ScreenToWorldPoint(new Vector3(0, 0, 0)).y - 1)
            Destroy(gameObject);

    }
}
