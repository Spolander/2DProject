using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class EnemyJointSystem : MonoBehaviour {
    [SerializeField]
    [Header("This array must have 2 indices")]
    private Transform[] joints;

    [SerializeField]
    private Transform[] chains;

    private void Update()
    {
        if (this.enabled == false)
            return;

        SetChainPositions();
    }
    void SetChainPositions()
    {
        for (int i = 0; i < chains.Length; i++)
        {

            if (i > 0)
            {
                float lerp = (i * 1.0f) / chains.Length;
                chains[i].transform.position = Vector3.Lerp(joints[0].transform.position, joints[1].transform.position, lerp);
            }

            else
                chains[i].transform.position = joints[0].transform.position;
        }
    }
}
