using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Skul", menuName = "Monster/Skul")]
public class SkulSo : ScriptableObject
{
    public float targetDistance = 15;
    public float hitDistance = 4;
    public int maxHealth = 10; //�ִ�ü��
    public int curHealth; //����ü��
    public int damage = 4;
   

    // ü�� �ʱ�ȭ �� �޼���
    public void ResetHP_SkulSO()
    {
        curHealth = maxHealth;
    }
}
