using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using LitJson;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager instance;
    public List<string> inventoryItems;
    private string filePath;

    public bool hasSword = false;
    public bool hasShield = false;
    public bool hasPotion = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        filePath = Application.persistentDataPath + "/inventoryData.json";
    
    //LoadInventory();
}

    public void LoadInventory()
    {
        if (File.Exists(filePath))
        {
            string jsonDataStr = File.ReadAllText(filePath);
            JsonData jsonData = JsonMapper.ToObject<JsonData>(jsonDataStr);
            inventoryItems = jsonData.inventoryItem;
            Debug.Log("�κ��丮 ������ �ε� �Ϸ�");

            hasSword = inventoryItems.Contains("Sword");
            hasShield = inventoryItems.Contains("Shield");
            hasPotion = inventoryItems.Contains("Potion");

            UpdateInventoryUI();
        }
        else
        {
            inventoryItems = new List<string>();
        }
    }

    public void SaveInventory()
    {
        JsonData jsonData = new JsonData();
        jsonData.inventoryItem = inventoryItems;
        string jsonDataStr = JsonMapper.ToJson(jsonData);
        File.WriteAllText(filePath, jsonDataStr);
        Debug.Log("�κ��丮 ������ ���� �Ϸ�");
    }

    public void AddItem(string itemName)
    {
        if (!inventoryItems.Contains(itemName))
        {
            inventoryItems.Add(itemName);

            if (itemName == "Sword")
            {
                hasSword = true;
            }
            if (itemName == "Shield")
            {
                hasShield = true;
            }
            if (itemName == "Potion")
            {
                hasPotion = true;
                UIManager.Instance.UpdatePotionUI(hasPotion);
            }

            SaveInventory();
            Debug.Log($"������ {itemName}�� �κ��丮�� �߰�");

            UIManager.Instance.UpdateItemUI(itemName);

            Fox_Move foxMove = FindObjectOfType<Fox_Move>();
            if (foxMove != null)
            {
                if (itemName == "Sword")
                {
                    foxMove.ActiveSword();
                }
                else if (itemName == "Shield")
                {
                    foxMove.ActiveShield();
                }
            }
        }

        //if (itemName == "Sword")
        //{
        //    Fox_Move foxMove = FindObjectOfType<Fox_Move>();
        //    if (foxMove != null)
        //    {
        //        foxMove.hasSword = true;
        //        Debug.Log("���� ����");
        //    }
        //}
    }
    private void UpdateInventoryUI()
    {
        if (UIManager.Instance != null)
        {
            if (hasSword)
            {
                UIManager.Instance.UpdateItemUI("Sword");
            }
            if (hasShield)
            {
                UIManager.Instance.UpdateItemUI("Shield");
            }
            if (hasPotion)
            {
                UIManager.Instance.UpdatePotionUI(hasPotion);
            }
        }
    }
}
