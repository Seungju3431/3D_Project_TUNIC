using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class JsonData
{
    // ��ġ ����
    public double[] position;
    // ���� ���൵
    public string progress;
    // ���� �� �̸�
    public string sceneName;
    // �κ��丮
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

    //����� ��ġ�� Vector3�� ��ȯ
    public Vector3 ToVector3()
    {
        return new Vector3((float)position[0], (float)position[1], (float)position[2]);
    }
}

[System.Serializable]
public class StateData
{
    public string boxID; //���� ���� ID(�� �̸� + ���� �̸�)
    public bool isOpen_Box; //���� ���ȴ��� ����

    public string doorID;
    public bool isOpen_Door; //�� ���ȴ��� ����
}

[System.Serializable]
public class BoxDataList
{
    public List<StateData> boxDataList = new List<StateData>();
}

[System.Serializable]
public class DoorDataList
{
    public List<StateData> doorDataList = new List<StateData>();
}