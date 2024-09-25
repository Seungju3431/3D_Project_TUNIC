using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using LitJson;


public class SaveSystem : MonoBehaviour
{
    public static SaveSystem Instance { get; private set; }
    private string savePath;
    //private GameObject fox;
    

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
        //fox = GameObject.FindGameObjectWithTag("Fox");
        savePath = Application.persistentDataPath + "/savefile.json";
        Debug.Log("savefile" + savePath);
    }

    public void SaveData(JsonData data)
    {
        Debug.Log("SaveData ȣ��");
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
            string savedScene = loadData.sceneName;
            if (!string.IsNullOrEmpty(savedScene))
            {
                StartCoroutine(LoadSceneAsync(savedScene, loadData));
            }
        
        }
    }

    private IEnumerator LoadSceneAsync(string sceneName, JsonData loadData)
    {
        //���� �񵿱������� �ε�
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);


        //���� ������ �ε�� ������
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        GameObject fox = GameObject.FindGameObjectWithTag("Fox");
        //�� �ε� �� ��ġ ����
        if (fox != null)
        {
            Vector3 foxPosition = loadData.ToVector3();
            fox.transform.position = foxPosition;
        }
        
        yield return new WaitForEndOfFrame(); // �������� ������ ���
        
    }
}
