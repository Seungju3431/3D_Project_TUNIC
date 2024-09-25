using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FoxManager : MonoBehaviour
{
    public static FoxManager Instance { get; private set; }
    private Animator ani;

    //ü�°���
    public int maxHealth = 10;
    public int currentHealth;
    private bool isHurt = false;

    //���׹̳� ����
    public float maxStamina = 40f; //���׹̳� �ִ�
    public float nowStamina = 40f; //���� ���׹̳�
    public float dodge_Stamina = 10f; //������ �Ҹ� ���׹̳�
    public float staminaRate = 20f; //���׹̳� ȸ��
    public float staminaRateDelay = 1.5f;

    //HP,Stamina��ȭ ��, UI������Ʈ�� �˸��� ����
    public event Action<int, int> OnHealthChanged;
    public event Action<float, float> OnStaminaChanged;
    

    
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
            Debug.Log("FoxManager ���� �� ��");
        }

        ani = GetComponent<Animator>();
    }

    private void Start()
    {
        currentHealth = maxHealth;
        OnHealthChanged?.Invoke(currentHealth, maxHealth);

        nowStamina = maxStamina;
        OnStaminaChanged?.Invoke(nowStamina, maxStamina);

        Debug.Log(gameObject.transform.position);
    }

    private void Update()
    {
        if (isHurt)
        {
            isHurt = false;
        }

        OnStaminaChanged?.Invoke(nowStamina, maxStamina);
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

    public void RecoverStamina(float deltaTime)
    {
        if (nowStamina < maxStamina)
        {
            nowStamina += staminaRate * deltaTime;
            if (nowStamina > maxStamina)
            {
                nowStamina = maxStamina;
            }
            OnStaminaChanged?.Invoke(nowStamina, maxStamina);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Monster_Attack"))
        {
            MonsterController monster_Attack = other.transform.parent.GetComponent<MonsterController>();
            currentHealth -= monster_Attack.skulData.damage;
            currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
            OnHealthChanged?.Invoke(currentHealth, maxHealth);
            Debug.Log("����ü�� : " + currentHealth);
           
            ani.SetTrigger("hurt");
            isHurt = true;
            Fox_Move fox_Move = GetComponent<Fox_Move>();
            fox_Move.Hurt_Bool();
            
            if (currentHealth <= 0)
            {
                Die();
            }
        }

        if (other.CompareTag("CheckPoint"))
        {
            Vector3 savePosition = other.transform.position;

            //JSON ��ü ����
            JsonData data = new JsonData(savePosition)
            {
                sceneName = SceneManager.GetActiveScene().name

            };

            if (SaveSystem.Instance != null)
            {
                SaveSystem.Instance.SaveData(data);
                Debug.Log("Save attempted."); // �� �α׵� �߰�
                Debug.Log("Save successful.");
            }
            else
            {
                Debug.LogError("SaveSystem.Instance is null!");
            }
        }
    }
}
