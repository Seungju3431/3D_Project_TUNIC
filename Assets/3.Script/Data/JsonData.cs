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
