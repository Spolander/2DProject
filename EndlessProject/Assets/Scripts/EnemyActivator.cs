using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyActivator : MonoBehaviour {

    Vector2 RightLocalRaycastPosition;
    Vector2 LeftLocalRaycastPosition;

    Vector2 TopLocalRaycastPosition;

    [SerializeField]
    private LayerMask enemyLayer;

    Vector2 screenRes;

    float screenWidth;

    private void Start()
    {
        UpdateRaycastPositions();
    }
    void UpdateRaycastPositions()
    {
        RightLocalRaycastPosition = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
        RightLocalRaycastPosition = transform.InverseTransformPoint(RightLocalRaycastPosition);

        LeftLocalRaycastPosition = Camera.main.ScreenToWorldPoint(new Vector3(0f, Screen.height, 0));
        LeftLocalRaycastPosition = transform.InverseTransformPoint(LeftLocalRaycastPosition);

        screenWidth = Mathf.Abs(RightLocalRaycastPosition.x - LeftLocalRaycastPosition.x);

        screenRes = new Vector2(Screen.width, Screen.height);
    }
    // Update is called once per frame
    void Update () {
        CheckForEnemies();

        if (Screen.width != screenRes.x || Screen.height != screenRes.y)
        {
            UpdateRaycastPositions();
        }
        
	}

    void CheckForEnemies()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.TransformPoint(RightLocalRaycastPosition), Vector2.down, 100f, enemyLayer);

        Debug.DrawRay(transform.TransformPoint(RightLocalRaycastPosition), Vector2.down * 100f, Color.red);
        if (hit.collider && !hit.collider.GetComponent<RockBoss>())
        {
            Enemy e = hit.collider.GetComponent<Enemy>();
            if(e.StartingDirection == -1 || e.EnableFromAnyDirection)
            hit.collider.GetComponent<Enemy>().enabled = true;
        }

        hit = Physics2D.Raycast(transform.TransformPoint(LeftLocalRaycastPosition), Vector2.down, 100f, enemyLayer);

        if (hit.collider && !hit.collider.GetComponent<RockBoss>())
        {
            Enemy e = hit.collider.GetComponent<Enemy>();
            if (e.StartingDirection == 1 || e.EnableFromAnyDirection)
                hit.collider.GetComponent<Enemy>().enabled = true;
        }

        
        hit = Physics2D.Raycast(transform.TransformPoint(LeftLocalRaycastPosition), Vector2.right, screenWidth, enemyLayer);

        if (hit.collider && !hit.collider.GetComponent<RockBoss>())
        {
            if(hit.collider.GetComponent<Enemy>().EnableFromAnyDirection)
            hit.collider.GetComponent<Enemy>().enabled = true;
        }
    }
}
