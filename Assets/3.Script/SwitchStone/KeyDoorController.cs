using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyDoorController : MonoBehaviour
{
    private Fox_manage fox_manage = null;
    private Animator ani;
    private string doorID;

    private void Awake()
    {
        
        ani = GetComponent<Animator>();
        //GameObject fox_obj = GameObject.FindGameObjectWithTag("Fox");
        //if (fox_obj != null)
        //{
        //    fox_manage = fox_obj.GetComponent<Fox_manage>();
        //}
        Invoke("KeyStart_In", 0.5f);
        doorID
            = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name
        + "_" + gameObject.name;
    }

    private void Start()
    {
        if (StateManager.instance.GetDoorState(doorID))
        {
            SetKeyDoorOpend();
        }
        
    }
    private void Update()
    {
        if (fox_manage != null)
        { 
        
        if (fox_manage.isKeyDoorOpen)
        {
            Debug.Log("여기 들어 왔나");
            OpenKeyDoor();
        }
        }
    }

    public void OpenKeyDoor()
    {
        Debug.Log("여기 들어 왔나");
        ani.SetTrigger("KeyDoorOpen");
        fox_manage.isKeyDoorOpen = false;
        StateManager.instance.UpdateSwitchStoneState(doorID, true);
    }

    private void SetKeyDoorOpend()
    {
        Debug.Log("여기 들어 왔나");
        ani.SetTrigger("Result");
    }

   

    private void KeyStart_In()
    {
        GameObject fox_obj = GameObject.FindGameObjectWithTag("Fox");
        if (fox_obj != null)
        {
            fox_manage = fox_obj.GetComponent<Fox_manage>();
        }
        Debug.Log("키 생성됨");
    }

}
