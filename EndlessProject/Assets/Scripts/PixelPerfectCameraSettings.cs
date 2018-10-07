using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
public class PixelPerfectCameraSettings : MonoBehaviour {

	
	void Start () {

        PixelPerfectCamera ppc = GetComponent<PixelPerfectCamera>();

        ppc.refResolutionX = Screen.width;
        ppc.refResolutionY = Screen.height;
	}
	
	
}
