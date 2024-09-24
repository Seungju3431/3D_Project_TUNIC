using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class JsonData
{
    public Vector3 FoxPosition;
    public List<string> inventoryItem = new List<string>();
    public int savePointID;
    public string progress; //게임 진행동
}
