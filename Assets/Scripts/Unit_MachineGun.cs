using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit_MachineGun : Unit, ITarget, IShooter
{
    public bool tir = false;
    [Space]
    public ShooterStats gun;
    public TargetterStats targetingStats;
    public AutomaticTargeting targeting;
    public TargetStats stats;
    public float rotationSpeed;
    public bool autoTarget = false;

    public override void Start() 
    {
        base.Start();
        targeting = new AutomaticTargeting(transform);
    }

    public void Update()
    {
        if (autoTarget)
        {
            targeting.SelectTarget(Team.ENEMY, targetingStats);
        }
        else
        {
            ManualAiming();
        }

        transform.rotation = Quaternion.Lerp(transform.rotation, targeting.GetRotationToTarget(), rotationSpeed * Time.deltaTime);

        //add a check to make sure that the unit is aiming at the target before firing
        if (targeting.IsFacingTarget() && targeting.targetInRange)
        {
            if (gun.attackTimer <= 0)
            {
                StartCoroutine(Shoot());
            }
            else
            {
                gun.attackTimer -= Time.deltaTime;
            }
        }

        tir = targeting.targetInRange;
    }

    #region interface implementations
    public void RecieveAttack(float damage)
    {
        stats.currentHealth -= damage;
    }

    public void ManualAiming()
    {
        Vector3 raycastHitLocation = targeting.targetPosition;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        LayerMask mask = 1 << LayerMask.NameToLayer("Floor");

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100, mask))
        {
            raycastHitLocation = hit.point;
            targeting.targetPosition = raycastHitLocation;
            targeting.targetInRange = true;
        }
        else
        {
            targeting.targetInRange = false;
        }
    }

    public IEnumerator Shoot()
    {
        gun.attackTimer = gun.attackDelay;
        for (int i = 0; i < gun.shotsPerAttack; i++)
        {
            FireShot();
            yield return new WaitForSeconds(gun.shotDelay);
        }
    }

    public void FireShot() 
    {
        float accuracy = (Vector3.Distance(transform.position, targeting.targetPosition) / gun.accuracyOverDistance) * gun.shotSpread;
        Vector3 adjustedTargetLocation = (Vector3)(UnityEngine.Random.insideUnitCircle * accuracy);
        adjustedTargetLocation.z = adjustedTargetLocation.y;
        adjustedTargetLocation.y = 0;
        adjustedTargetLocation += targeting.targetPosition;

        Vector3 diff = adjustedTargetLocation - transform.position;
        diff.Normalize();

        Quaternion rot = transform.rotation;

        GameObject bullet = Instantiate(gun.bulletPrefab, transform.position, rot);

        Rigidbody brb = null;
        if (bullet.TryGetComponent(out brb))
        {
            brb.AddForce(diff * gun.bulletForce);
            bullet.transform.rotation.SetLookRotation(brb.velocity);
        }

        Destroy(bullet, 10);

    }

    public float TargettingValue(GameObject otherObject)
    {
        return 1;
    }

    public TargetStats GetStats()
    {
        return stats;
    }

    public override void OnSelected()
    {
        autoTarget = !autoTarget;
        Debug.Log(name + " Selected");
    }

    public override void OnDeselected()
    {
        autoTarget = false;
    }

    #endregion
}
