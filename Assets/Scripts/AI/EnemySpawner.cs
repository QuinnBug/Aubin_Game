using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject[] prefabs;
    public GameObject squadPrefab;
    public float spawnTimer;
    public float spawnDelay;
    public float squadDelay;
    public int unitsPerSquad;
    public int squadsPerSpawn;
    public float spawnRange;

    public void Update()
    {
        Spawn();
    }

    private void Spawn()
    {
        spawnTimer -= Time.deltaTime;
        if (spawnTimer <= 0)
        {
            StartCoroutine(SpawnUnits());
            //squadsPerSpawn++;
        }
    }

    IEnumerator SpawnUnits() 
    {
        spawnTimer = spawnDelay;

        Enemy_Squad _squad;

        for (int i = 0; i < squadsPerSpawn; i++)
        {
            Vector3 rndPos = Random.insideUnitCircle.normalized * spawnRange;
            rndPos.z = rndPos.y;
            rndPos.y = 0;
            rndPos += transform.position;

            int rndUnits = Random.Range(4, unitsPerSquad);
            unitsPerSquad++;

            _squad = Instantiate(squadPrefab, rndPos, Quaternion.identity).GetComponent<Enemy_Squad>();

            for (int j = 0; j < rndUnits; j++)
            {
                int rndNum = Random.Range(0, prefabs.Length);
                _squad.AddUnit(Instantiate(prefabs[rndNum], rndPos, Quaternion.identity).GetComponent<Enemy>());
            }
            yield return new WaitForSeconds(squadDelay);
        }
    }
}
