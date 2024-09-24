using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using LitJson;


public class SaveSystem : MonoBehaviour
{
    private string savePath;

    private void Awake()
    {
        savePath = Application.persistentDataPath + "/savefile.json";
    }

    public void SaveGame(JsonData data)
    {
        //데이터 JSON 형식으로 변환
        string jsonData = JsonMapper.ToJson(data);

        //JSON 데이터 파일로 저장
        File.WriteAllText(savePath, jsonData);
        Debug.Log("게임저장 완료");
    }

    public JsonData LoadGame()
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
}
