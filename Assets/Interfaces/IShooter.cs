using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct ShooterStats 
{
    public float attackDelay;
    public float shotDelay;
    public int damagePerAttack;
    public int shotsPerAttack;
    public float shotSpread;
    public float accuracyOverDistance;
    public float attackTimer;
    [Space]
    public GameObject bulletPrefab;
    public Vector3 bulletSpawnOffset;
    public float bulletForce;
}

public interface IShooter
{
    public IEnumerator Shoot();
    public void FireShot();
}
