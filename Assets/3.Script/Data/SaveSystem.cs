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
        //������ JSON �������� ��ȯ
        string jsonData = JsonMapper.ToJson(data);

        //JSON ������ ���Ϸ� ����
        File.WriteAllText(savePath, jsonData);
        Debug.Log("�������� �Ϸ�");
    }

    public JsonData LoadGame()
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
}
