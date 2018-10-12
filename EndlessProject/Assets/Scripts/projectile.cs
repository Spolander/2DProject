using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class projectile : MonoBehaviour {

    protected Vector3 direction;
    protected float speed;

    
    [SerializeField]
    protected float hitBoxSize = 0.01f;

    [SerializeField]
    protected LayerMask collisionLayers;

    protected Camera cam;

    private void Start()
    {
        cam = Camera.main;
    }

    public virtual void Initialize(Vector2 direction, float speed)
    {
        this.direction = direction.normalized;
        
        this.speed = speed;

        if(direction.y == 0)
        {
            transform.localScale = new Vector3(Mathf.Sign(direction.x), 1, 1);
        }
        else
        {
            if(direction.x > 0)
            transform.localEulerAngles = new Vector3(0, 0, Mathf.Atan(direction.y / direction.x) * Mathf.Rad2Deg);
            else
            {
                transform.localScale = new Vector3(-1, 1, 1);
                transform.localEulerAngles = new Vector3(0, 0,  (Mathf.Atan(direction.y / direction.x) * Mathf.Rad2Deg));

                if (Mathf.Abs(direction.x) < 0.5f && direction.y > 0.5f)
                {
                    transform.localScale = new Vector3(1, 1, 1);
                    transform.localEulerAngles = new Vector3(0, 0, 90);
                }
            }
              
        }
           

    }

    private void Update()
    {
        transform.position += direction * Time.deltaTime * speed;
        HitDetection();
    }

    protected virtual void HitDetection()
    {
        Collider2D[] cols = new Collider2D[1];
        Physics2D.OverlapBoxNonAlloc(transform.position, Vector2.one * hitBoxSize, 0, cols,collisionLayers);

        if (cols[0])
        {
            if (cols[0].GetComponent<EnemyHealth>())
                cols[0].GetComponent<EnemyHealth>().TakeDamage();
            Destroy(gameObject);
        }


        if (transform.position.x > cam.ScreenToWorldPoint(new Vector3(Screen.width, 0)).x)
            Destroy(gameObject);
        else if (transform.position.x < cam.ScreenToWorldPoint(new Vector3(0, 0)).x)
            Destroy(gameObject);
        else if (transform.position.y > cam.ScreenToWorldPoint(new Vector3(0, Screen.height)).y)
            Destroy(gameObject);
        else if (transform.position.y < cam.ScreenToWorldPoint(new Vector3(0, 0)).y)
            Destroy(gameObject);

    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawCube(transform.position, Vector3.one * hitBoxSize);
    }


}
