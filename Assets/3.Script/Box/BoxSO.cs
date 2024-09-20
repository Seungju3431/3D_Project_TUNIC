using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Box", menuName = "Interaction/Box")]
public class BoxSO : ScriptableObject
{
    public int coinAmount = 5;
    public GameObject coinPrefab;

}
