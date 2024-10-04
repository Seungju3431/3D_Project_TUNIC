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
    private bool fromForestMain; //World -> ForestMain
    private bool fromForestLeft; //ForestMain -> ForestLeft
    private bool fromForestLeft_Up;

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
            gameObject.transform.position = new Vector3(-10.85f, 11.83f, -128.43f);
            Debug.Log("World 1");
            fromSwordCave = false;
            fox_Move.canMoveOutNav = true;
            //SceneManager.sceneLoaded -= OnSceneLoaded;
        }
        else if (scene.name == "Forest Main" && !fromForestMain)
        {
            gameObject.transform.position = new Vector3(76.38f, 7.98f, 80.68f);
            fox_Move.canMoveOutNav = true;
            fromForestMain = true;
        }
        else if (scene.name == "Forest Left" && !fromForestLeft && fromForestLeft_Up)
        {
            gameObject.transform.position = new Vector3(123.21f, 0f, 49.03f);
            fox_Move.canMoveOutNav = true;
            fromForestLeft = true;
        }
        else if (scene.name == "Forest Main" && fromForestLeft)
        {
            fox_Move.canMoveOutNav = true;
            gameObject.transform.position = new Vector3(124.15f, -4.76f, 46.29f);

        }
        else if (scene.name == "Forest Left" && !fromForestLeft_Up && fromForestLeft)
        {
            gameObject.transform.position = new Vector3(142.81f, 16f, 48.32f);
            fox_Move.canMoveOutNav = true;
            fromForestLeft_Up = true;
        }
        else if (scene.name == "Forest Main" && fromForestLeft_Up)
        {
            fox_Move.canMoveOutNav = true;
            gameObject.transform.position = new Vector3(139.99f, 16f, 48.81f);
        }

        //���� ����

    }

   
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Monster_Attack"))
        {
            Fox_Move fox_Move = GetComponent<Fox_Move>();
            if (!fox_Move.isShield)
            {
                MonsterController monster_Attack = other.transform.parent.GetComponent<MonsterController>();
                currentHealth -= monster_Attack.skulData.damage;
                currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
                OnHealthChanged?.Invoke(currentHealth, maxHealth);
                Debug.Log("����ü�� : " + currentHealth);

                ani.SetTrigger("hurt");
                isHurt = true;
                fox_Move.Hurt_Bool();

                if (currentHealth <= 0)
                {
                    Die();
                }
            }
            
        }

        else if (other.CompareTag("CheckPoint"))
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

        else if (other.CompareTag("SwordCave"))
        {
            SceneManager.LoadScene("SwordCave");
            fromSwordCave = false;
        }

        else if (other.CompareTag("SwordCave_World"))
        {
            SceneManager.LoadScene("World 1");
            fox_Move.canMoveOutNav = false;

        }
        else if (other.CompareTag("KeyDoorCave_In"))
        {
            fox_Move.canMoveOutNav = false;
            gameObject.transform.position = new Vector3(48.94f, 20f, -130.27f);

        }
        else if (other.CompareTag("KeyDoorCave_Out"))
        {
            fox_Move.canMoveOutNav = false;
            gameObject.transform.position = new Vector3(20.38f, 20f, -122.26f);
        }
        else if (other.CompareTag("Forest Main"))
        {
            SceneManager.LoadScene("Forest Main");
            fox_Move.canMoveOutNav = false;
            fromForestMain = false;
            //gameObject.transform.position = new Vector3(444.64f, 38f, 105.13f);
        }
        else if (other.CompareTag("Forest Left"))
        {
            SceneManager.LoadScene("Forest Left");
            fox_Move.canMoveOutNav = false;
            fromForestLeft = false;
        }
        else if (other.CompareTag("Forest Left_Up"))
        {
            SceneManager.LoadScene("Forest Left");
            fox_Move.canMoveOutNav = false;
            fromForestLeft_Up = false;
        }
        else if (other.CompareTag("Forest Left_Main"))
        {
            SceneManager.LoadScene("Forest Main");
            fox_Move.canMoveOutNav = false;
        }
        else if (other.CompareTag("Forest Left_Main_Up"))
        {
            SceneManager.LoadScene("Forest Main");
            fox_Move.canMoveOutNav = false;
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("KeyDoorCave_In"))
        {
            fox_Move.canMoveOutNav = true;
        }
        else if (other.CompareTag("KeyDoorCave_Out"))
        {
            fox_Move.canMoveOutNav = true;
        }

    }

}
