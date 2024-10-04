using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawnManager : MonoBehaviour
{
    public List<Transform> spawnPoints;  // ���� ���� ���� ����Ʈ
    public GameObject monsterPrefab;     // ������ ���� ������
    private string sceneName;            // ���� �� �̸�

    void Start()
    {
        sceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        SpawnMonstersInScene();  // �� �ε� �� �ٷ� ���͸� ����
    }

    public void SpawnMonstersInScene()
    {
        for (int i = 0; i < spawnPoints.Count; i++)
        {
            string monsterID = sceneName + "_Monster_" + i; // �� �̸� + ���� ID
            if (!StateManager.instance.GetMonsterState(monsterID))
            {
                GameObject monster = Instantiate(monsterPrefab, spawnPoints[i].position, Quaternion.identity);

                // ���Ͱ� ������ �� ���� ������Ʈ
                //monster.GetComponent<Monster>().OnDefeat += () =>
                //{
                //    StateManager.instance.UpdateMonsterState(monsterID, true);
                //};
            }
        }
    }
}
