using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class CoinMove : MonoBehaviour
{
    private Rigidbody rid;
    private CoinPool coinPool;

    public bool isGetCoin;

    private void Start()
    {
        isGetCoin = false;
    }

    private void Awake()
    {
        rid = GetComponent<Rigidbody>();
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Fox"))
        {
            coinPool.ReturnCoin(gameObject);
        }
    }
    
    private void UpCoin()
    {
        Vector3 randomDirection = new Vector3(UnityEngine.Random.Range(-1f, 1f), 1f,
            UnityEngine.Random.Range(-1f, 1f));
        float force = 3f;
        rid.AddForce(randomDirection * force, ForceMode.Impulse);
    }


}
