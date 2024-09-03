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
            Debug.LogError("Animator 컴포넌트가 할당되지 않았습니다!");
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
            Monster monster_Attack = other.transform.parent.GetComponent<Monster>();
            curHealth -= monster_Attack.damage;
            Debug.Log("현재체력 : " + curHealth);
            ani.SetTrigger("hurt");
            isHurt = true;
            if (curHealth == 0)
            {
                ani.SetTrigger("die");
            }
        }
    }

    
}
