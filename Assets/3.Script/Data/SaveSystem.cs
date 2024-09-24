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
            Debug.Log("게임저장 완료");
        
        
    }

    public JsonData LoadData()
    {
        //파일 존재 확인
        if (File.Exists(savePath))
        {
            string jsonData = File.ReadAllText(savePath);

            //JSON 데이터 역직렬
            JsonData data = JsonMapper.ToObject<JsonData>(jsonData);
            Debug.Log("게임 로드 완료");

            return data;
        }
        else
        {
            Debug.Log("저장 데이터 찾지 못 함");
            return null;
        }
    }

    public void LoadGame()
    {
        JsonData loadData = SaveSystem.Instance.LoadData();

        if (loadData != null)
        {
            //저장된 위치를 Vector3로 변환
            Vector3 foxPostion = loadData.ToVector3();
            //fox.transform.position = foxPostion;
        }
    }
}
