using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pendulum : MonoBehaviour {


    private float angle = 90; //90-270

    private float lerp = 0;

    public float Lerp { get { return lerp; } }

    private Vector3 center;
    public Vector3 Center { get { return center; } }

    
    [SerializeField]
    private float speed;

    private float length = 5;

    int direction = 1;
    public int Direction { get { return direction; } }

    float currentSpeed;

    public AnimationCurve speedCurve;


    float slowdown = 0;

    public float slowDownSpeed = 1;
    public float endSlowDown = 0.8f;

    bool initialized = false;
    public bool Initialized { get { return initialized; } set { initialized = value; } }

   public void InitializePendulum(Vector3 centerPoint, float length, int direction)
    {
        this.lerp = 0;
        this.center = centerPoint;
        this.length = length;
        this.direction = direction;
        this.initialized = true;
        angle = direction == 1 ? 90 : 270;
    }

    public Vector3 pendulumPoint()
    {
        slowdown = Mathf.MoveTowards(slowdown, 0, Time.deltaTime * slowDownSpeed);

        Vector3 point = Vector3.zero;

        if (direction == 1)
        {
            lerp = (angle - 90) / 180;
            currentSpeed = speedCurve.Evaluate(lerp) * speed;
            angle = Mathf.MoveTowards(angle, 270, Time.deltaTime * currentSpeed * (1 - slowdown));
            if (angle == 270)
            {
                slowdown = endSlowDown;
                direction = -1;
            }

        }
        else
        {
            lerp = 1 - (angle - 90) / 180;
            currentSpeed = speedCurve.Evaluate(lerp) * speed;
            angle = Mathf.MoveTowards(angle, 90, Time.deltaTime * currentSpeed * (1 - slowdown));
            if (angle == 90)
            {
                slowdown = endSlowDown;
                direction = 1;
            }

        }


        Vector3 dir = Quaternion.AngleAxis(angle, Vector3.forward) * Vector3.up;
        Debug.DrawRay(transform.position, dir.normalized * length, Color.red);

        point = center + dir.normalized * length;

        return point;
    }
}
