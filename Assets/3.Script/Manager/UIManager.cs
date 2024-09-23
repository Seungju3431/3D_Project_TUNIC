using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [SerializeField] private Slider healthBar;
    [SerializeField] private Slider staminaBar;

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
            Debug.Log("UIManager 생성 안 됨");
        }
    }

    private void Start()
    {
        if (FoxManager.Instance != null)
        {
            //PlayerManager 체력 변경 이벤트 구독
            FoxManager.Instance.OnHealthChanged += UpdateHealthUI;
            //스테미너 이벤트 구독
            FoxManager.Instance.OnStaminaChanged += UpdateStaminaUI;

            //초기 UI업데이트
            UpdateHealthUI(FoxManager.Instance.GetCurrentHealth(), FoxManager.Instance.maxHealth);
            UpdateStaminaUI(FoxManager.Instance.nowStamina, FoxManager.Instance.maxStamina);


        }
    }
    
    private void OnDestroy()
    {
        if (FoxManager.Instance != null)
        {
            FoxManager.Instance.OnHealthChanged -= UpdateHealthUI;
            FoxManager.Instance.OnStaminaChanged -= UpdateStaminaUI;
        }
    }

    //체력
    private void UpdateHealthUI(int currentHealth, int maxHealth)
    {
        if (healthBar != null)
        {
            healthBar.maxValue = maxHealth;
            healthBar.value = currentHealth;
        }
    }

    //스테미나
    public void UpdateStaminaUI(float nowStamina, float maxStamina)
    {
        if (staminaBar != null)
        {
            staminaBar.maxValue = maxStamina;
            staminaBar.value = nowStamina;
        }
    }
}
