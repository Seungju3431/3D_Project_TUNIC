using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FoxManager : MonoBehaviour
{
    public static FoxManager Instance = null;
 
    private Animator ani;
    private Fox_Move fox_Move;

    //ü�°���
    public int maxHealth = 10;
    public int currentHealth;
    
    private bool isHurt = false;
    private bool fromSwordCave; //SwordCave�� -> World

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
            GameObject fox = GameObject.FindGameObjectWithTag("Fox");
            if (fox != null)
            { 
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Debug.Log("���� ���� ��");
            }
            
        }
        else
        {
            Destroy(gameObject);
            Debug.Log("FoxManager �ı�");
            return;
        }
        SceneManager.sceneLoaded += OnSceneLoaded;
        ani = GetComponent<Animator>();
        fox_Move = GetComponent<Fox_Move>();
        
    }
    //private void OnDestroy()
    //{
    //    SceneManager.sceneLoaded -= OnSceneLoaded; // �� �ε� �̺�Ʈ ���� ����
    //    Debug.Log("dddd");
    //}
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

    //�ٸ� �� �� ��
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "SwordCave" && !fromSwordCave)
        {
            Debug.Log("SwordCave");
            gameObject.transform.position = new Vector3(5.6f, 5.9f, -4.9f);
            fromSwordCave = true;
            //SceneManager.sceneLoaded -= OnSceneLoaded;
            Debug.Log("OnSceneLoaded ���� ������"); // ���� ���� Ȯ�� �α�
        }
        else if (scene.name == "World 1" && fromSwordCave)
        {
            gameObject.transform.position = new Vector3(-10.8f, 12.0f, -128.5f);
            Debug.Log("World 1");
            fromSwordCave = false;
            fox_Move.canMoveOutNav = true;
            //SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        //���� ����

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

        if (other.CompareTag("SwordCave"))
        {
            SceneManager.LoadScene("SwordCave");
            fromSwordCave = false;
        }

        if (other.CompareTag("SwordCave_World"))
        {
            SceneManager.LoadScene("World 1");
            fox_Move.canMoveOutNav = false;
            
        }
    }
    
}
