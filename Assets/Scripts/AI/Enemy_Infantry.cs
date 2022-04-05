using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Infantry : Enemy
{
    public override void Start()
    {
        base.Start();
    }

    public void Update()
    {
        Action();
    }

    public void Action()
    {
        if (stats.currentHealth > 0)
        {
            Move();
        }
        else
        {
            squad.RemoveUnit(this);
            Destroy(gameObject);
        }
    }

    #region interface implementations

    public float TargetEvaluation(GameObject targetObj, ITarget targetStats)
    {
        return 1;
    }

    #endregion
}
