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
            SaveInventory();
            Debug.Log($"������ {itemName}�� �κ��丮�� �߰�");

            UIManager.Instance.UpdateItemUI(itemName);
        }
    }
}
