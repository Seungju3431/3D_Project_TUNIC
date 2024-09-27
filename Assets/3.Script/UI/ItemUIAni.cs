using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemUIAni : MonoBehaviour
{
    public Animator ani;
    public RawImage raw;
    

    private void Awake()
    {
        ani = GetComponent<Animator>();
        raw = GetComponent<RawImage>();
    }
}
