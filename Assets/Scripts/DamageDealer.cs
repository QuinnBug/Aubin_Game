using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageDealer : MonoBehaviour
{
    public float damage;
    public bool playersTeam;

    private void OnTriggerEnter(Collider other)
    {
        if (playersTeam && other.tag == "Player") return;

        ITarget tgt;
        if (other.TryGetComponent(out tgt))
        {
            tgt.RecieveAttack(damage);
            Destroy(gameObject);
        }
    }
}
