using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchStoneController : MonoBehaviour
{
    [SerializeField] public Animator door;
    private Fox_manage fox_manage;
    private Animator ani;
    private string doorID;
    public bool isSwitchOn;

    private void Awake()
    {
        ani = GetComponent<Animator>();
        //door = GetComponent<Animator>();
        GameObject fox_obj = GameObject.FindGameObjectWithTag("Fox");
        if (fox_obj != null)
        {
            fox_manage = fox_obj.GetComponent<Fox_manage>();
        }
        doorID
            = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name
        + "_" + gameObject.name;
    }

    private void Start()
    {
        if (StateManager.instance.GetDoorState(doorID))
        {
            SetSwitchStoneOpend();
        }
    }
    private void Update()
    {
        if (fox_manage.isSwitchStoneOpen)
        {
            OpenSwitchStone();
        }
    }

    public void OpenSwitchStone()
    {
        ani.SetTrigger("StoneOpen");
        fox_manage.isSwitchStoneOpen = false;
        StateManager.instance.UpdateSwitchStoneState(doorID, true);
    }

    private void SetSwitchStoneOpend()
    {
        ani.SetTrigger("Result");
    }

    public void OpenDoor()
    {
        door.SetTrigger("doorOpen");
    }

    public void OpendDoor()
    {
        door.SetTrigger("doorResult");
    }
}
