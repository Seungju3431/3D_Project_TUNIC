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
            Debug.Log("UIManager ���� �� ��");
        }
    }

    private void Start()
    {
        //PlayerManager ü�� ���� �̺�Ʈ ����
        if (FoxManager.Instance != null)
        {
            FoxManager.Instance.OnHealthChanged += UpdateHealthUI;

            //�ʱ� UI������Ʈ
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
