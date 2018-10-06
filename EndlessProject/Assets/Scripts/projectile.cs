using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class projectile : MonoBehaviour {

    Vector3 direction;
    float speed;

    
    private float hitBoxSize = 0.02f;

    [SerializeField]
    private LayerMask collisionLayers;

    public void Initialize(Vector2 direction, float speed)
    {
        this.direction = direction.normalized;
        
        this.speed = speed;

        if(direction.y == 0)
        {
            transform.localScale = new Vector3(Mathf.Sign(direction.x), 1, 1);
        }
        else
             transform.localEulerAngles = new Vector3(0, 0, Mathf.Atan(direction.y / direction.x) * Mathf.Rad2Deg);

    }

    private void Update()
    {
        transform.position += direction * Time.deltaTime * speed;
        HitDetection();
    }

    void HitDetection()
    {
        Collider2D[] cols = new Collider2D[1];
        Physics2D.OverlapBoxNonAlloc(transform.position, Vector2.one * hitBoxSize, 0, cols,collisionLayers);

        if (cols[0])
            Destroy(gameObject);
    }

   
}
