using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    public GameObject monsterPrefab;
    public Transform spawnPoint;

    public void SpawnMonster()
    {
        GameObject obj = Instantiate(monsterPrefab, spawnPoint.position, spawnPoint.rotation);
        obj.GetComponent<MonsterController>().Initialize();
    }
}
