using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class JsonData
{
    // 위치 저장
    public double[] position;
    // 게임 진행도
    public string progress;
    // 현재 씬 이름
    public string sceneName;
    // 인벤토리
    public List<string> inventoryItem = new List<string>();


    public JsonData()
    {
        position = new double[3];
        progress = "";
        sceneName = "";
        inventoryItem = new List<string>();
    }
    public JsonData(Vector3 vector3)
    {
        position = new double[] { vector3.x, vector3.y, vector3.z };
    }

    //저장된 위치를 Vector3로 변환
    public Vector3 ToVector3()
    {
        return new Vector3((float)position[0], (float)position[1], (float)position[2]);
    }
}


[System.Serializable]
public class BoxStateData
{
    public string boxID; // 상자 고유 ID(씬 이름 + 상자 이름)
    public bool isOpen_Box; // 상자가 열렸는지 여부
}
[System.Serializable]
public class DoorStateData
{
    public string doorID; // 문 고유 ID(씬 이름 + 문 이름)
    public bool isOpen_Door; // 문이 열렸는지 여부
}

[System.Serializable]
public class BoxDataList
{
    public List<BoxStateData> boxDataList = new List<BoxStateData>();
}

[System.Serializable]
public class DoorDataList
{
    public List<DoorStateData> doorDataList = new List<DoorStateData>();
}