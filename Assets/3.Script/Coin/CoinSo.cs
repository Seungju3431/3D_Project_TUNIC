using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Coin", menuName = "Interaction/Coin")]
public class CoinSo : ScriptableObject
{
    public int coinValue = 1;
    public int coinPoolSize = 10;
    public GameObject coinPrefab;
}
