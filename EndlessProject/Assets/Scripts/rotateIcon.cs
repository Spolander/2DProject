using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class rotateIcon : MonoBehaviour {


    [SerializeField]
    private float interval;
    float timer;
    RectTransform r;

	// Use this for initialization
	void Start () {
        r = GetComponent<RectTransform>();		
	}
	
	// Update is called once per frame
	void Update () {
        timer += Time.fixedDeltaTime;

     
            r.Rotate(new Vector3(0, 1, 0), Time.fixedDeltaTime*interval);
       
	}
}
