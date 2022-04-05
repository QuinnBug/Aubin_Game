using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour, ITarget
{
    public TargetStats stats;
    public Vector3 offsetDistance;
    public NavigationStats navStats;
    public TargetterStats targetingStats;
    public Navigation nav;
    public AutomaticTargeting targeting;
    public Enemy_Squad squad;

    public virtual void Start()
    {
        nav = new Navigation(transform);
        targeting = new AutomaticTargeting(transform);
        targeting.SelectTarget(Team.PLAYER, targetingStats);
    }

    public void Move()
    {
        Debug.Log("move");
        transform.position = Vector3.MoveTowards(transform.position, nav.targetPosition,
            navStats.moveSpeed * Time.deltaTime);
    }

    public virtual TargetStats GetStats()
    {
        return stats;
    }

    public virtual void RecieveAttack(float damage)
    {
        stats.currentHealth -= damage;
    }

    public virtual float TargettingValue(GameObject otherObject)
    {
        return -Vector3.Distance(transform.position, otherObject.transform.position);
    }
}
