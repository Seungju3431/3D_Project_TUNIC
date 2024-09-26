using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoxSpawner : MonoBehaviour
{
    public GameObject foxPrefab;
    public Transform spawnPoint;

    private void Start()
    {
        SpawnFox();
    }

    private void SpawnFox()
    {
        Instantiate(foxPrefab, spawnPoint.position, spawnPoint.rotation);
    }
}
