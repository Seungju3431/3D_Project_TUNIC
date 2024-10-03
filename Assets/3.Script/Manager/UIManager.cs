using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [SerializeField] public Image potionIcon;
    [SerializeField] Sprite fullPotionSprite;
    [SerializeField] Sprite emptyPotionSprite;
    [SerializeField] private Slider healthBar;
    [SerializeField] private Slider staminaBar;
    [SerializeField] private GameObject shieldImage;
    [SerializeField] private GameObject swordImage;
    [SerializeField] private GameObject keyImage;




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
    //����
    public void UpdatePotionUI(bool hasPotion)
    {
        if (potionIcon != null)
        {
            potionIcon.sprite = hasPotion ? fullPotionSprite : emptyPotionSprite;
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

    //������
    public void UpdateItemUI(string itemName)
    {
        Debug.Log("UpdateItemUI");
        switch (itemName)
        {
            case "Sword":
                swordImage.SetActive(true);
                break;
            case "Shield":
                shieldImage.SetActive(true);
                break;
            case "Key":
                shieldImage.SetActive(true);
                break;

            default:
                Debug.LogWarning("�� �� ���� ������: " + itemName);
                break;
        }
    }

    
}
