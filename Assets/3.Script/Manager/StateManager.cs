using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using LitJson;

public class StateManager : MonoBehaviour
{
    public static StateManager instance;
    private string filePath;
    public BoxDataList boxDataList;

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
        filePath = Application.persistentDataPath + "/boxData.json";
        //LoadBoxData();
    }
    public void SaveBoxData()
    {
        string jsonDta = JsonMapper.ToJson(boxDataList);
        File.WriteAllText(filePath, jsonDta);
        Debug.Log("Box 세이브");
    }
    public void LoadBoxData()
    {
        if (File.Exists(filePath))
        {
            string jsonData = File.ReadAllText(filePath);
            boxDataList = JsonMapper.ToObject<BoxDataList>(jsonData);
            Debug.Log("Box데이터 로드");
        }
        else
        {
            boxDataList = new BoxDataList();
        }
    }

    public void UpdateBoxState(string boxID, bool isOpen_Box)
    {
        StateData box = boxDataList.boxDataList.Find(b => b.boxID == boxID);
        if (box == null)
        {
            box = new StateData { boxID = boxID, isOpen_Box = isOpen_Box };
            boxDataList.boxDataList.Add(box);
        }
        else
        {
            box.isOpen_Box = isOpen_Box;
        }
        SaveBoxData();
    }

    public bool GetBoxState(string boxID)
    {
        StateData box = boxDataList.boxDataList.Find(b => b.boxID == boxID);
        return box != null && box.isOpen_Box;
    }
}
