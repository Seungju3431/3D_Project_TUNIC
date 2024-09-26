using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box_Controller : MonoBehaviour
{
    
    private Animator ani;
    private string boxID;
    private Fox_manage fox_manage;
    private void Awake()
    {
        ani = GetComponent<Animator>();
        GameObject fox_obj = GameObject.FindGameObjectWithTag("Fox");
        if (fox_obj != null)
        {
            fox_manage = fox_obj.GetComponent<Fox_manage>();
        }
        boxID
        = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name
        + "_" + gameObject.name;
    }

    private void Start()
    {

        if (StateManager.instance.GetBoxState(boxID))
        {
            SetBoxOpened();
        }
    }
    private void Update()
    {
        if (fox_manage.isBox)
        {
            OpenBox();
            
        }
    }
    public void OpenBox()
    {
        ani.SetTrigger("isOpen");
        fox_manage.isBox = false;
        //StateManager.instance.UpdateBoxState(boxID, true);
    }

    private void SetBoxOpened()
    {
        ani.SetTrigger("Result");
    }
    
}
