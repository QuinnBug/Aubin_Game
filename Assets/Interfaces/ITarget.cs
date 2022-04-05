using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct TargetStats 
{
    public float currentHealth;
    public float maxHealth;
}

public interface ITarget
{
    public float TargettingValue(GameObject otherObject);
    public void RecieveAttack(float damage);
    public TargetStats GetStats();
}
