using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fox_manage : MonoBehaviour
{
    public int maxHealth;
    public int curHealth;

    private Animator ani;

    private bool isHurt = false;

    private void Awake()
    {
        ani = GetComponent<Animator>();
        if (ani == null)
        {
            Debug.LogError("Animator ������Ʈ�� �Ҵ���� �ʾҽ��ϴ�!");
        }
    }

    private void Start()
    {
        curHealth = maxHealth;
    }

    private void Update()
    {
        if (isHurt)
        {
            isHurt = false;
        }
        
    }
    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Monster_Attack"))
        {
            MonsterController monster_Attack = other.transform.parent.GetComponent<MonsterController>();
            curHealth -= monster_Attack.damage;
            Debug.Log("����ü�� : " + curHealth);
            ani.SetTrigger("hurt");
            isHurt = true;
            Fox_Move fox_Move = GetComponent<Fox_Move>();
            fox_Move.Hurt_Bool();
            if (curHealth == 0)
            {
                ani.SetTrigger("die");
            }
        }
    }

    
}
