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
            Debug.Log("인벤토리 데이터 로드 완료");
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
        Debug.Log("인벤토리 데이터 저장 완료");
    }

    public void AddItem(string itemName)
    {
        if (!inventoryItems.Contains(itemName))
        {
            inventoryItems.Add(itemName);
            SaveInventory();
            Debug.Log($"아이템 {itemName}을 인벤토리에 추가");

            UIManager.Instance.UpdateItemUI(itemName);
        }
    }
}
