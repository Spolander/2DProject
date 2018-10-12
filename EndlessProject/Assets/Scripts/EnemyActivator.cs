using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyActivator : MonoBehaviour {

    Vector2 localRaycastPosition;

    [SerializeField]
    private LayerMask enemyLayer;

    private void Start()
    {
        localRaycastPosition = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
        localRaycastPosition = transform.InverseTransformPoint(localRaycastPosition);
    }

    // Update is called once per frame
    void Update () {
        CheckForEnemies();
	}

    void CheckForEnemies()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.TransformPoint(localRaycastPosition), Vector2.down, 100f, enemyLayer);

        if (hit.collider)
        {
            hit.collider.GetComponent<Enemy>().enabled = true;
        }
    }
}
