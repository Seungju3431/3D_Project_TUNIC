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
            Debug.Log("UIManager ���� �� ��");
        }
    }

    private void Start()
    {
        if (FoxManager.Instance != null)
        {
            //PlayerManager ü�� ���� �̺�Ʈ ����
            FoxManager.Instance.OnHealthChanged += UpdateHealthUI;
            //���׹̳� �̺�Ʈ ����
            FoxManager.Instance.OnStaminaChanged += UpdateStaminaUI;

            //�ʱ� UI������Ʈ
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

    //ü��
    private void UpdateHealthUI(int currentHealth, int maxHealth)
    {
        if (healthBar != null)
        {
            healthBar.maxValue = maxHealth;
            healthBar.value = currentHealth;
        }
    }

    //���׹̳�
    public void UpdateStaminaUI(float nowStamina, float maxStamina)
    {
        if (staminaBar != null)
        {
            staminaBar.maxValue = maxStamina;
            staminaBar.value = nowStamina;
        }
    }
}
