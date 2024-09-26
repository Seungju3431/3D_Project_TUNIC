using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoxSpawner : MonoBehaviour
{
    public GameObject foxPrefab;
    public Transform spawnPoint;
    public MonsterSpawner monsterSpawner;
    public CinemachineController virtualCamera;

    private void Start()
    {
        if (!GameObject.FindGameObjectWithTag("Fox"))
        {
            SpawnFox();
        }
        Debug.Log("여우 생성");
    }

    private void SpawnFox()
    {
        GameObject obj = Instantiate(foxPrefab, spawnPoint.position, spawnPoint.rotation);
        obj.GetComponent<Fox_Move>().Initialize();
        if (virtualCamera != null)
        {
            
            virtualCamera.SetTarGetFox(obj);
        }
        OnFoxSpawned();
    }

    private void OnFoxSpawned()
    {
        if (monsterSpawner != null)
        {
            monsterSpawner.SpawnMonster();
        }
        else
        {
            Debug.LogWarning("MonsterSpawner가 설정되지 않았습니다.");
        }
    }
}
