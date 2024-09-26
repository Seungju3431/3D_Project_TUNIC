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
        //    // 카메라의 transform을 리셋
        //    main.transform.position = Vector3.zero;
        //    main.transform.rotation = Quaternion.identity;
        //    main.transform.localScale = Vector3.one;

        //    Debug.Log("메인 카메라의 transform을 리셋했습니다.");
        //}
        //else
        //{
        //    Debug.LogWarning("메인 카메라를 찾을 수 없습니다!");
        //}
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
        GameObject fox = GameObject.FindGameObjectWithTag("Fox");

        if (fox != null)
        {
            virtualCamera.Follow = fox.transform;
            virtualCamera.LookAt = fox.transform;
            Debug.Log("버추얼 카메라");
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
