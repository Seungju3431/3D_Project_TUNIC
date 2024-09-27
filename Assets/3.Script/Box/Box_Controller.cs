using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box_Controller : MonoBehaviour
{
    [SerializeField] public Animator itemUI;
    
    private Fox_manage fox_manage;
    private Animator ani;
    private string boxID;
    public string itemName;
    

    private void Awake()
    {
        
        //itemUI = GetComponent<Animator>();
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
        Debug.Log("���¹ڽ� �޼���");
        ani.SetTrigger("isOpen");
        fox_manage.isBox = false;
        StateManager.instance.UpdateBoxState(boxID, true);
        //�κ��丮�� ������ �߰�
        InventoryManager.instance.AddItem(itemName);
    }

    private void SetBoxOpened()
    {
        ani.SetTrigger("Result");
    }

    //�ִϸ��̼� �̺�Ʈ
    public void UIAni_T()
    {
        
  
        itemUI.SetBool("show", true);
    }
}
