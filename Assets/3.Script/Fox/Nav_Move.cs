using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Nav_Move : MonoBehaviour
{

    [SerializeField] public float checkRadiys;

    public bool IsOnNavMesh(Vector3 position)
    {
        NavMeshHit hit;
        return NavMesh.SamplePosition(position, out hit, checkRadiys, NavMesh.AllAreas);

    }

    ////��Ʈ ��� ��ġ ������Ʈ
    //public Vector3 ToNavMesh(Vector3 position)
    //{
    //    NavMeshHit hit;
    //    if (NavMesh.SamplePosition(position, out hit, 0.5f, NavMesh.AllAreas))
    //    {
    //        return hit.position;
    //    }
    //    return position;
    //}
}
