using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Skul", menuName = "Monster/Skul")]
public class SkulSo : ScriptableObject
{
    public float targetDistance = 15;
    public float hitDistance = 4;
    public int maxHealth = 10; //최대체력
    public int curHealth; //현재체력
    public int damage = 4;
   

    // 체력 초기화 할 메서드
    public void ResetHP_SkulSO()
    {
        curHealth = maxHealth;
    }
}
