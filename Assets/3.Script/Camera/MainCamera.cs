using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    private void Awake()
    {
        // 메인 카메라 가져오기
        Camera mainCamera = Camera.main;

        if (mainCamera != null)
        {
            // 카메라의 transform을 리셋
            mainCamera.transform.position = Vector3.zero;
            mainCamera.transform.rotation = Quaternion.identity;
            mainCamera.transform.localScale = Vector3.one;

            Debug.Log("메인 카메라의 transform을 리셋했습니다.");
        }
        else
        {
            Debug.LogWarning("메인 카메라를 찾을 수 없습니다!");
        }
    }
}
