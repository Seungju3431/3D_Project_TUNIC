using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using LitJson;

public class StateManager : MonoBehaviour
{
    public static StateManager instance;
    private string filePath;
    private string filePath_door;
    public BoxDataList boxDataList;
    public DoorDataList doorDataList;

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
        filePath_door = Application.persistentDataPath + "/doorData.json";
        //LoadBoxData();
    }
    public void SaveBoxData()
    {
        string jsonDta = JsonMapper.ToJson(boxDataList);
        File.WriteAllText(filePath, jsonDta);
        Debug.Log("Box 세이브");
    }
    public void SaveDoorData()
    {
        string jsonDta = JsonMapper.ToJson(doorDataList);
        File.WriteAllText(filePath_door, jsonDta);
        Debug.Log("Door 세이브");
    }
    public void LoadBoxData()
    {
        if (File.Exists(filePath))
        {
            string jsonData = File.ReadAllText(filePath);
            boxDataList = JsonMapper.ToObject<BoxDataList>(jsonData);
            doorDataList = JsonMapper.ToObject<DoorDataList>(jsonData);
            Debug.Log("Box데이터 로드");
        }
        else
        {
            boxDataList = new BoxDataList();
        }
    }

    public void UpdateBoxState(string boxID, bool isOpen_Box)
    {
        BoxStateData box = boxDataList.boxDataList.Find(b => b.boxID == boxID);
        if (box == null)
        {
            box = new BoxStateData { boxID = boxID, isOpen_Box = isOpen_Box };
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
        BoxStateData box = boxDataList.boxDataList.Find(b => b.boxID == boxID);
        return box != null && box.isOpen_Box;
    }

    public void UpdateSwitchStoneState(string doorID, bool isOpen_Door)
    {

        DoorStateData door = doorDataList.doorDataList.Find(d => d.doorID == doorID);
        if (door == null)
        {
            door = new DoorStateData { doorID = doorID, isOpen_Door = isOpen_Door };
            doorDataList.doorDataList.Add(door);
        }
        else
        {
            door.isOpen_Door = isOpen_Door;
        }
        SaveDoorData();
    }

    public bool GetDoorState(string doorID)
    {
        DoorStateData door = doorDataList.doorDataList.Find(d => d.doorID == doorID);
        return door != null && door.isOpen_Door;
    }
}
