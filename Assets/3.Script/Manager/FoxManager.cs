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

    //체력관련
    public int maxHealth = 10;
    public int currentHealth;
    
    private bool isHurt = false;
    private bool fromSwordCave; //SwordCave씬 -> World

    //스테미너 관련
    public float maxStamina = 40f; //스테미너 최대
    public float nowStamina = 40f; //현재 스테미너
    public float dodge_Stamina = 10f; //구르기 소모 스테미너
    public float staminaRate = 20f; //스테미너 회복
    public float staminaRateDelay = 1.5f;

    //HP,Stamina변화 때, UI컴포넌트에 알리기 위해
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
            Debug.Log("새로 생성 됨");
            }
            
        }
        else
        {
            Destroy(gameObject);
            Debug.Log("FoxManager 파괴");
            return;
        }
        SceneManager.sceneLoaded += OnSceneLoaded;
        ani = GetComponent<Animator>();
        fox_Move = GetComponent<Fox_Move>();
        
    }
    //private void OnDestroy()
    //{
    //    SceneManager.sceneLoaded -= OnSceneLoaded; // 씬 로드 이벤트 구독 해제
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

    //다른 씬 들어갈 때
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "SwordCave" && !fromSwordCave)
        {
            Debug.Log("SwordCave");
            gameObject.transform.position = new Vector3(5.6f, 5.9f, -4.9f);
            fromSwordCave = true;
            //SceneManager.sceneLoaded -= OnSceneLoaded;
            Debug.Log("OnSceneLoaded 구독 해제됨"); // 구독 해제 확인 로그
        }
        else if (scene.name == "World 1" && fromSwordCave)
        {
            gameObject.transform.position = new Vector3(-10.8f, 12.0f, -128.5f);
            Debug.Log("World 1");
            fromSwordCave = false;
            fox_Move.canMoveOutNav = true;
            //SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        //구독 해지

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

        if (other.CompareTag("CheckPoint"))
        {
            Vector3 savePosition = other.transform.position;

            //JSON 객체 생성
            JsonData data = new JsonData(savePosition)
            {
                sceneName = SceneManager.GetActiveScene().name

            };

            if (SaveSystem.Instance != null)
            {
                SaveSystem.Instance.SaveData(data);
                Debug.Log("Save attempted."); // 이 로그도 추가
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
