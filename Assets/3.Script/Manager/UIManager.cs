using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [SerializeField] private Slider healthBar;

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
        //PlayerManager 체력 변경 이벤트 구독
        if (FoxManager.Instance != null)
        {
            FoxManager.Instance.OnHealthChanged += UpdateHealthUI;

            //초기 UI업데이트
            UpdateHealthUI(FoxManager.Instance.GetCurrentHealth(), FoxManager.Instance.maxHealth);
        }
    }

    private void OnDestroy()
    {
        if (FoxManager.Instance != null)
        {
            FoxManager.Instance.OnHealthChanged -= UpdateHealthUI;
        }
    }

    private void UpdateHealthUI(int currentHealth, int maxHealth)
    {
        if (healthBar != null)
        {
            healthBar.maxValue = maxHealth;
            healthBar.value = currentHealth;
        }
    }
}
