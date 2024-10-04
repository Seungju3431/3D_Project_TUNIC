using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawnManager : MonoBehaviour
{
    public List<Transform> spawnPoints;  // 몬스터 스폰 지점 리스트
    public GameObject monsterPrefab;     // 스폰할 몬스터 프리팹
    private string sceneName;            // 현재 씬 이름

    void Start()
    {
        sceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        SpawnMonstersInScene();  // 씬 로드 후 바로 몬스터를 스폰
    }

    public void SpawnMonstersInScene()
    {
        for (int i = 0; i < spawnPoints.Count; i++)
        {
            string monsterID = sceneName + "_Monster_" + i; // 씬 이름 + 고유 ID
            if (!StateManager.instance.GetMonsterState(monsterID))
            {
                GameObject monster = Instantiate(monsterPrefab, spawnPoints[i].position, Quaternion.identity);

                // 몬스터가 잡혔을 때 상태 업데이트
                //monster.GetComponent<Monster>().OnDefeat += () =>
                //{
                //    StateManager.instance.UpdateMonsterState(monsterID, true);
                //};
            }
        }
    }
}
