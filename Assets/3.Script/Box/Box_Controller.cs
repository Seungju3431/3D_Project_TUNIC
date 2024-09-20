using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box_Controller : MonoBehaviour
{
    private CoinPool coinpool;
    private CoinMove coinmove;
    private Animator ani;

    public bool isOpen;

    private void Start()
    {
        isOpen = false;
    }
    private void Awake()
    {
        ani = GetComponent<Animator>();
    }

    

    private void OpenBox()
    {
        if (isOpen)
        {
            
        }
    }
}
