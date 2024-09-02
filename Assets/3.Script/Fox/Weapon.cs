using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public BoxCollider hit_Sword1;
    public BoxCollider hit_Sword2;
    public SphereCollider hit_Sword3;

    public void Hit_Sword1_Start()
    {
        hit_Sword1.enabled = true;
    }
    public void Hit_Sword1_Finish()
    {
        hit_Sword1.enabled = false;
    }
    public void Hit_Sword2_Start()
    {
        hit_Sword2.enabled = true;
    }
    public void Hit_Sword2_Finish()
    {
        hit_Sword2.enabled = false;
    }
    public void Hit_Sword3_Start()
    {
        hit_Sword3.enabled = true;
    }
    public void Hit_Sword3_Finish()
    {
        hit_Sword3.enabled = false;
    }
}
