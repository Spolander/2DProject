using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : projectile {

    
    public override void Initialize(Vector2 direction, float speed)
    {
        cam = Camera.main;
        this.direction = direction.normalized;

        this.speed = speed;

        if (direction.y == 0)
        {
            transform.localScale = new Vector3(Mathf.Sign(direction.x), 1, 1);
        }
        else
            transform.localEulerAngles = new Vector3(0, 0, 90 * -Mathf.Sign(direction.x) + Mathf.Atan(direction.y / direction.x) * Mathf.Rad2Deg);

    }
    protected override void HitDetection()
    {
        Collider2D[] cols = new Collider2D[1];
        Physics2D.OverlapBoxNonAlloc(transform.position, Vector2.one * hitBoxSize, 0, cols, collisionLayers);

        if (cols[0])
        {
            if (cols[0].GetComponent<PlayerHealth>())
                cols[0].GetComponent<PlayerHealth>().TakeDamage();
            Destroy(gameObject);
          
        }

        if (transform.position.x > cam.ScreenToWorldPoint(new Vector3(Screen.width, 0)).x+1)
            Destroy(gameObject);
        else if (transform.position.x < cam.ScreenToWorldPoint(new Vector3(0, 0)).x-1)
                Destroy(gameObject);
        else if (transform.position.y > cam.ScreenToWorldPoint(new Vector3(0, Screen.height)).y+1)
            Destroy(gameObject);
        else if (transform.position.y < cam.ScreenToWorldPoint(new Vector3(0, 0)).y-1)
            Destroy(gameObject);

    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawCube(transform.position, Vector3.one * hitBoxSize);
    }

}
