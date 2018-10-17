using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pendulum : MonoBehaviour {


    public float angle = 90; //90-270

    public float lerp = 0;

    public Transform center;

    public Transform ball;

    public float speed;

    public float length = 5;

    int direction = 1;

    float currentSpeed;

    public AnimationCurve speedCurve;

    float targetAngle = 270;

    float slowdown = 0;

    public float slowDownSpeed = 1;
    public float endSlowDown = 0.8f;

    
  
    private void Update()
    {

        slowdown = Mathf.MoveTowards(slowdown, 0, Time.deltaTime * slowDownSpeed);

        if (direction == 1)
        {
            lerp = (angle - 90) / 180;
            currentSpeed = speedCurve.Evaluate(lerp) * speed;
            angle = Mathf.MoveTowards(angle, 270, Time.deltaTime * currentSpeed*(1-slowdown));
            if (angle == 270)
            {
                slowdown = endSlowDown;
                direction = -1;
            }
              
        }
        else
        {
            lerp = 1-(angle - 90) / 180;
            currentSpeed = speedCurve.Evaluate(lerp)*speed;
            angle = Mathf.MoveTowards(angle, 90, Time.deltaTime * currentSpeed * (1 - slowdown));
            if (angle == 90)
            {
                slowdown = endSlowDown;
                direction = 1;
            }
              
        }


        Vector3 dir = Quaternion.AngleAxis(angle, Vector3.forward) * Vector3.up;
        Debug.DrawRay(transform.position, dir.normalized*length,Color.red);

        ball.position = center.position + dir.normalized * length;

      

    }
}
