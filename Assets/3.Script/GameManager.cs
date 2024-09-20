using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject coinPrefab;
    public CoinSo coinData;

    private void Start()
    {
        GameObject createCoin = Instantiate(coinPrefab);
        CoinPool coinPool = createCoin.GetComponent<CoinPool>();
        coinPool.coinData = coinData;
        createCoin.SetActive(false);
    }
}
