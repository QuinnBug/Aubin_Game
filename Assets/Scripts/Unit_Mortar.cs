using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit_Mortar : Unit, ITarget
{
    public TargetStats stats;
    public Vector3 targetPosition;

    private bool aiming;

    public TargetStats GetStats()
    {
        return stats;
    }

    public override void OnDeselected()
    {
        aiming = false;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        LayerMask mask = 1 << LayerMask.NameToLayer("Floor");

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100, mask))
        {
            targetPosition = hit.point;
            Debug.DrawLine(transform.position, targetPosition, Color.red, 10);
        }
         
    }

    public override void OnSelected()
    {
        aiming = true;
    }

    public void RecieveAttack(float damage)
    {
        stats.currentHealth -= damage;
    }

    public float TargettingValue(GameObject otherObject)
    {
        return 1;
    }
}
