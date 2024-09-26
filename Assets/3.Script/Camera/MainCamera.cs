using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    private void Awake()
    {
        // ���� ī�޶� ��������
        Camera mainCamera = Camera.main;

        if (mainCamera != null)
        {
            // ī�޶��� transform�� ����
            mainCamera.transform.position = Vector3.zero;
            mainCamera.transform.rotation = Quaternion.identity;
            mainCamera.transform.localScale = Vector3.one;

            Debug.Log("���� ī�޶��� transform�� �����߽��ϴ�.");
        }
        else
        {
            Debug.LogWarning("���� ī�޶� ã�� �� �����ϴ�!");
        }
    }
}
