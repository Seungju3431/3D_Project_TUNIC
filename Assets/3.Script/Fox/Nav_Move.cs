using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Nav_Move : MonoBehaviour
{

    [SerializeField] public float checkRadiys;
    //[SerializeField] public float maxHeight;

    public bool IsOnNavMesh(Vector3 position)
    {
        NavMeshHit hit;
        return NavMesh.SamplePosition(position, out hit, checkRadiys, NavMesh.AllAreas);

    }

    //public bool IsOnNavMesh(Vector3 position, out float navMeshHeight)
    //{
    //    NavMeshHit hit;

    //    if (NavMesh.SamplePosition(position, out hit, checkRadiys, NavMesh.AllAreas))
    //    {
    //        navMeshHeight = hit.position.y;

    //        if (Mathf.Abs(position.y - hit.position.y) <= maxHeight)
    //        {
    //            return true;
    //        }
    //    }

    //    navMeshHeight = 0;
    //    return false;
    //}

}
