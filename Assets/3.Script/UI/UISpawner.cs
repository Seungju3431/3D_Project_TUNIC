using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISpawner : MonoBehaviour
{
    public GameObject UIPrefab;

    private void Awake()
    {
        if (!GameObject.FindGameObjectWithTag("Fox"))
        { 
        
        UISpawn();
        }
    }

    private void UISpawn()
    {
        Instantiate(UIPrefab);
    }
}
