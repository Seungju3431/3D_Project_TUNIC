using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CinemachineController : MonoBehaviour
{
    private CinemachineVirtualCamera virtualCamera;

    private void Start()
    {
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
    }

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
