using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using LitJson;


public class SaveSystem : MonoBehaviour
{
    public static SaveSystem Instance { get; private set; }
    private string savePath;
    //public GameObject fox;

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
        }
        
        savePath = Application.persistentDataPath + "/savefile.json";
    }

    public void SaveData(JsonData data)
    {
        
            string jsonData = JsonMapper.ToJson(data);
            Debug.Log("Saving Data: " + jsonData);
            File.WriteAllText(savePath, jsonData);
            Debug.Log("�������� �Ϸ�");
        
        
    }

    public JsonData LoadData()
    {
        //���� ���� Ȯ��
        if (File.Exists(savePath))
        {
            string jsonData = File.ReadAllText(savePath);

            //JSON ������ ������
            JsonData data = JsonMapper.ToObject<JsonData>(jsonData);
            Debug.Log("���� �ε� �Ϸ�");

            return data;
        }
        else
        {
            Debug.Log("���� ������ ã�� �� ��");
            return null;
        }
    }

    public void LoadGame()
    {
        JsonData loadData = SaveSystem.Instance.LoadData();

        if (loadData != null)
        {
            //����� ��ġ�� Vector3�� ��ȯ
            Vector3 foxPostion = loadData.ToVector3();
            //fox.transform.position = foxPostion;
        }
    }
}
