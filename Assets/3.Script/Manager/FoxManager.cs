using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class FoxManager : MonoBehaviour
{
    public static FoxManager Instance { get; private set; }

    public int maxHealth = 10;
    public int currentHealth;
    private bool isHurt = false;
    private Animator ani;

    //HP변화 때, UI컴포넌트에 알리기 위해
    public event Action<int, int> OnHealthChanged;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            Debug.Log("FoxManager 생성 안 됨");
        }

        ani = GetComponent<Animator>();
    }

    private void Start()
    {
        currentHealth = maxHealth;
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }

    private void Update()
    {
        if (isHurt)
        {
            isHurt = false;
        }
    }
    //public void TakeDamage(int damage)
    //{
    //    currentHealth -= damage;
    //    currentHealth = Math.Clamp(currentHealth, 0, maxHealth);
    //    OnHealthChanged?.Invoke(currentHealth, maxHealth);

    //    if (currentHealth <= 0)
    //    {
    //        Die();
    //    }
    //}

    public void Heal(int amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }

    public int GetCurrentHealth()
    {
        return currentHealth;
    }

    private void Die()
    {
        ani.SetTrigger("die");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Monster_Attack"))
        {
            MonsterController monster_Attack = other.transform.parent.GetComponent<MonsterController>();
            currentHealth -= monster_Attack.skulData.damage;
            currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
            OnHealthChanged?.Invoke(currentHealth, maxHealth);
            Debug.Log("현재체력 : " + currentHealth);
           
            ani.SetTrigger("hurt");
            isHurt = true;
            Fox_Move fox_Move = GetComponent<Fox_Move>();
            fox_Move.Hurt_Bool();
            
            if (currentHealth <= 0)
            {
                Die();
            }
        }
    }
}
