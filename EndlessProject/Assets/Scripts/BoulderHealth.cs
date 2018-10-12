using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoulderHealth : EnemyHealth {

    public override void Death()
    {
        GetComponent<bouncingProjectile>().Explode();
    }

}
