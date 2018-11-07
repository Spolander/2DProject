using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour {
    Camera cam;
    Vector3 lastCameraPos;
    GameObject[] parallaxObjects;
    GameObject[] parallaxObjectsVertical;
    float[] parallaxScales;
    float[] verticalParallaxScales;

    [SerializeField]
    private float smoother = 1;
    [SerializeField]
    private float baseMultiplier = 2;

    [SerializeField]
    private float verticalMultiplier = 2;
	// Use this for initialization
	void Start () {

        cam = Camera.main;
        lastCameraPos = cam.transform.position;
        parallaxObjects = GameObject.FindGameObjectsWithTag("parallax");
        parallaxObjectsVertical = GameObject.FindGameObjectsWithTag("parallaxVertical");
        parallaxScales = new float[parallaxObjects.Length];
        verticalParallaxScales = new float[parallaxObjectsVertical.Length];

        for (int i = 0; i < parallaxObjects.Length; i++)
            parallaxScales[i] = parallaxObjects[i].transform.position.z * -1*baseMultiplier;

        for (int i = 0; i < parallaxObjectsVertical.Length; i++)
            verticalParallaxScales[i] = parallaxObjectsVertical[i].transform.position.z * -1 * baseMultiplier;
    }
	
	// Update is called once per frame
	void LateUpdate () {

    
        for (int i = 0; i < parallaxObjects.Length; i++)
        {
            float parallax = (lastCameraPos.x - cam.transform.position.x) * parallaxScales[i];
            float backgroundTargetPosX = parallaxObjects[i].transform.position.x + parallax;
            Vector3 backgroundTargetPos = new Vector3(backgroundTargetPosX, parallaxObjects[i].transform.position.y, parallaxObjects[i].transform.position.z);
            parallaxObjects[i].transform.position = Vector3.Lerp(parallaxObjects[i].transform.position, backgroundTargetPos, smoother * Time.deltaTime);
        }

        for (int i = 0; i < parallaxObjectsVertical.Length; i++)
        {
            float parallaxX = (lastCameraPos.x - cam.transform.position.x) * verticalParallaxScales[i];
            float parallaxY = (lastCameraPos.y - cam.transform.position.y) * verticalParallaxScales[i];

            float backgroundTargetPosX = parallaxObjectsVertical[i].transform.position.x + parallaxX;
            float backgroundTargetPosY = parallaxObjectsVertical[i].transform.position.y + parallaxY;

            Vector3 backgroundTargetPos = new Vector3(backgroundTargetPosX, backgroundTargetPosY, parallaxObjectsVertical[i].transform.position.z);
            parallaxObjectsVertical[i].transform.position = Vector3.Lerp(parallaxObjectsVertical[i].transform.position, backgroundTargetPos, smoother * Time.deltaTime);

        }

        lastCameraPos = cam.transform.position;
	}
  

}
