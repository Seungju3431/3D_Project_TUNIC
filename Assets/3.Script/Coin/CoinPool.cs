using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinPool : MonoBehaviour
{
    public CoinSo coinData;

    private List<GameObject> coinPool;
    

    private void Start()
    {
        coinPool = new List<GameObject>();

        if (coinData == null)
        {
            Debug.LogError("coinData is not assigned in CoinPool.");
            return;
        }

        for (int i = 0; i < coinData.coinPoolSize; i++)
        {
            GameObject coin = Instantiate(coinData.coinPrefab);
            coin.SetActive(false);
            coinPool.Add(coin);
        }
        
    }

    public GameObject GetCoin()
    {
        foreach (GameObject coin in coinPool)
        {
            if (!coin.activeSelf)
            {
                coin.SetActive(true);
                return coin;
            }
        }

        //������ ���ڸ��� ���� ����
        GameObject newCoin = Instantiate(coinData.coinPrefab);
        newCoin.SetActive(true);
        coinPool.Add(newCoin);
        return newCoin;
    }

    public void ReturnCoin(GameObject coin)
    {
        coin.SetActive(false);
    }
}
