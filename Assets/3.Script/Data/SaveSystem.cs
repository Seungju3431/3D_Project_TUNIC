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
        Debug.Log("SaveData 호출");
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
            string savedScene = loadData.sceneName;
            if (!string.IsNullOrEmpty(savedScene))
            {
                StartCoroutine(LoadSceneAsync(savedScene, loadData));
            }
        
        }
    }

    private IEnumerator LoadSceneAsync(string sceneName, JsonData loadData)
    {
        //씬을 비동기적으로 로드
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);


        //씬이 완전히 로드될 때까지
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        GameObject fox = GameObject.FindGameObjectWithTag("Fox");
        //씬 로드 후 위치 설정
        if (fox != null)
        {
            Vector3 foxPosition = loadData.ToVector3();
            fox.transform.position = foxPosition;
        }
        
        yield return new WaitForEndOfFrame(); // 프레임의 끝까지 대기
        
    }
}
