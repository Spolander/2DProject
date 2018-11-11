using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bossAreaActivator : MonoBehaviour {
    [SerializeField]
    private Vector3 cameraPosition;

    [SerializeField]
    private float moveTime = 2;

 

    [SerializeField]
    private AudioSource levelMusic;

    [SerializeField]
    private AudioSource bossMusic;

    [SerializeField]
    RockBoss boss;

    [SerializeField]
    private Transform mustSeePoint;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            GetComponent<Collider2D>().enabled = false;
            StartCoroutine(movingAnimation());
    
        }
    }

    IEnumerator movingAnimation()
    {
        Vector3 startPos = CameraFollow.playerCamera.transform.position;
        float lerp = 0;
        CameraFollow.playerCamera.enabled = false;

        float height = 2f * Camera.main.orthographicSize;
        float width = height * Camera.main.aspect;
        cameraPosition.x = mustSeePoint.position.x - (width / 2);


        while (lerp < 1)
        {
            levelMusic.volume = Mathf.Lerp(1, 0, lerp);
            CameraFollow.playerCamera.transform.position = Vector3.Slerp(startPos, cameraPosition, lerp);
            lerp += Time.deltaTime / moveTime;
            yield return null;
        }

        GameObject g = new GameObject("cameraTarget");
        g.transform.position = cameraPosition;
        CameraFollow.playerCamera.alternateFollowTarget = g.transform;
        CameraFollow.playerCamera.enabled = true;

        boss.enabled = true;
        bossMusic.Play();

    }
}
