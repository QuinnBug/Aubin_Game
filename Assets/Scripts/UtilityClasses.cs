using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Team 
{
    NONE,
    BOTH,
    PLAYER,
    ENEMY
}

[Serializable]
public struct TargetterStats
{
    public float detectionRange;
}

[Serializable]
public struct NavigationStats
{
    public float moveSpeed;
    public float minimumDistance;
    public Vector3 target;
}

public class AutomaticTargeting
{
    public Transform transform;
    public bool targetInRange;
    public Vector3 targetPosition;
    private Vector3 flatYTargetPos;

    public AutomaticTargeting(Transform _tf) 
    {
        transform = _tf;
    }

    public Quaternion GetRotationToTarget() 
    {
        flatYTargetPos = targetPosition;
        flatYTargetPos.y = transform.position.y;

        return Quaternion.LookRotation(flatYTargetPos - transform.position, transform.up);
    }

    public bool IsFacingTarget() 
    {
        Vector3 dirFromAtoB = (flatYTargetPos - transform.position).normalized;
        float dotProd = Vector3.Dot(dirFromAtoB, transform.forward);

        return dotProd > 0.9;
    }

    public void SelectTarget(Team _teamToTarget, TargetterStats _stats) 
    {
        LayerMask mask = -1;
        switch (_teamToTarget)
        {
            case Team.NONE:
                return;

            case Team.BOTH:
                mask = 1 << (LayerMask.NameToLayer("Enemy") | LayerMask.NameToLayer("Player"));
                break;

            case Team.PLAYER:
                mask = 1 << LayerMask.NameToLayer("Player");
                break;

            case Team.ENEMY:
                mask = 1 << LayerMask.NameToLayer("Enemy");
                break;

            default:
                Debug.Log("Error In Selecting Target - Invalid Team");
                break;
        }

        Collider[] hits = Physics.OverlapSphere(transform.position, _stats.detectionRange, mask);

        ITarget tgt = null;
        ITarget bestTgt = null;
        Vector3 bestTgtLocation = targetPosition;
        float bestTgtScore = -999;
        float tgtScore = -999;

        foreach (Collider hit in hits)
        {
            if (hit.TryGetComponent(out tgt))
            {
                tgtScore = tgt.TargettingValue(transform.gameObject);

                if (bestTgt == null || bestTgtScore < tgtScore)
                {
                    bestTgt = tgt;
                    bestTgtScore = tgtScore;
                    bestTgtLocation = hit.transform.position;
                }
            }
        }

        targetInRange = bestTgt != null;
        if (targetInRange)
        {
            targetPosition = bestTgtLocation;
            targetPosition.y = 0;
        }
    }
}

public class Navigation 
{
    public Transform transform;
    public Vector3 targetPosition;
    public List<Vector3> path;

    public Navigation(Transform _tf)
    {
        transform = _tf;
    }

    public Vector3 GetNextPathPosition() 
    {
        return path[0];
    }
    public List<Vector3> GetPath() 
    {
        return path;
    }
    public void SetTarget(Vector3 _pos) 
    {
        targetPosition = _pos;
    }
}
