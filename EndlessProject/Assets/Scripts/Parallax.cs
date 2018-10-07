using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour {
    Camera cam;
    Vector3 lastCameraPos;
    GameObject[] parallaxObjects;
    float[] parallaxScales;

    [SerializeField]
    private float smoother = 1;
    [SerializeField]
    private float baseMultiplier = 2;
	// Use this for initialization
	void Start () {

        cam = Camera.main;
        lastCameraPos = cam.transform.position;
        parallaxObjects = GameObject.FindGameObjectsWithTag("parallax");
        parallaxScales = new float[parallaxObjects.Length];

        for (int i = 0; i < parallaxObjects.Length; i++)
            parallaxScales[i] = parallaxObjects[i].transform.position.z * -1*baseMultiplier;
	}
	
	// Update is called once per frame
	void Update () {

        for (int i = 0; i < parallaxObjects.Length; i++)
        {
            float parallax = (lastCameraPos.x - cam.transform.position.x) * parallaxScales[i];
            float backgroundTargetPosX = parallaxObjects[i].transform.position.x + parallax;
            Vector3 backgroundTargetPos = new Vector3(backgroundTargetPosX, parallaxObjects[i].transform.position.y, parallaxObjects[i].transform.position.z);
            parallaxObjects[i].transform.position = Vector3.Lerp(parallaxObjects[i].transform.position, backgroundTargetPos, smoother * Time.deltaTime);
        }

        lastCameraPos = cam.transform.position;
	}
}
