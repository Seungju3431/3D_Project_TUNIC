using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CinemachineController : MonoBehaviour
{
    private CinemachineVirtualCamera virtualCamera;
    //public Camera main;

    private void Awake()
    {
        //if (main != null)
        //{
        //    // ī�޶��� transform�� ����
        //    main.transform.position = Vector3.zero;
        //    main.transform.rotation = Quaternion.identity;
        //    main.transform.localScale = Vector3.one;

        //    Debug.Log("���� ī�޶��� transform�� �����߽��ϴ�.");
        //}
        //else
        //{
        //    Debug.LogWarning("���� ī�޶� ã�� �� �����ϴ�!");
        //}
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
        GameObject fox = GameObject.FindGameObjectWithTag("Fox");

        if (fox != null)
        {
            virtualCamera.Follow = fox.transform;
            virtualCamera.LookAt = fox.transform;
            Debug.Log("���߾� ī�޶�");
        }
    }
    //private void Start()
    //{
    //    virtualCamera = GetComponent<CinemachineVirtualCamera>();
    //   GameObject fox = GameObject.FindGameObjectWithTag("Fox");

    //    if (fox != null)
    //    {
    //        virtualCamera.Follow = fox.transform;
    //        virtualCamera.LookAt = fox.transform;
    //    }
    //}

    public void SetTarGetFox(GameObject fox)
    {
        Initialize();
           fox = GameObject.FindGameObjectWithTag("Fox");
       
        if (fox != null)
        {
            virtualCamera.Follow = fox.transform;
            virtualCamera.LookAt = fox.transform;
        }
    }
    public void Initialize()
    {
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
    }
}
