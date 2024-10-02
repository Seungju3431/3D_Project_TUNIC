using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Fox_manage : MonoBehaviour
{
    public GameObject Spacebar;
    private Fox_Move fox_Move;
    
    //public int maxHealth;
    //public int curHealth;

    private Animator ani;
    private Rigidbody rb;
    

    //private bool isHurt = false;
    public bool isInteraction;
    
    //사다리
    private Collider ladder_Pcol;
    private Vector3 LadderPosition_Bottom;
    private Vector3 LadderPosition_Up;
    private bool isClimb_Up;
    public bool isClimb_Down;
    public bool isClimbing = false;

    //Box
    private Vector3 Box_center;
    private string boxID;
    public string itemName;
    private bool isSwordBox;
    public bool isBox;

    //Page
    private bool isPage;

    //SwitchStone
    private Vector3 Door_center;
    private bool isSwitch;
    public bool isSwitchStoneOpen;
    private string doorID;


    private void Awake()
    {
        
        Spacebar = GameObject.FindGameObjectWithTag("Spacebar");
        fox_Move = GetComponent<Fox_Move>();
        rb = GetComponent<Rigidbody>();
        ani = GetComponent<Animator>();
        
    }
    private void Update()
    {
        //if (isHurt)
        //{
        //    isHurt = false;
        //}
        if (ladder_Pcol != null)
        {
            //사다리 올라가기
            if (Input.GetKeyDown(KeyCode.Space) && isInteraction && Spacebar.activeSelf && !rb.isKinematic)
            {
                if (isClimb_Up)
                {

                    FoxToLadder_Up();
                    isClimbing = true;
                    Spacebar.SetActive(false);
                    ani.SetBool("isClimb", true);
                }
                else if (isClimb_Down)
                {
                    FoxToLadder_Down();
                    ani.SetTrigger("isClimb_On");
                    isClimbing = true;
                    ani.SetBool("isClimb", true);

                }
                else if (isSwordBox)
                {
                    string boxID = GetCurrentBoxID();
                    if (!StateManager.instance.GetBoxState(boxID))
                    {
                        Debug.Log("상자 열기 들어옴");
                        FoxToBox();
                        isBox = true;

                        //상자 상태 저장
                        StateManager.instance.UpdateBoxState(boxID, true);

                    }
                    else
                    {
                        Debug.Log("요기, 상자 이미 열려있음");
                    }


                }
                else if (isSwitch)
                {
                    string doorID = GetCurrentDoorID();
                    if (!StateManager.instance.GetDoorState(doorID))
                    {
                        ani.SetTrigger("doorOpen");
                        FoxToDoor();
                        isSwitchStoneOpen = true;
                        isSwitch = false;
                        StateManager.instance.UpdateSwitchStoneState(doorID, true);
                    }
                    
                }
                else if (isPage)
                {

                }

            }
         
        }
       

        //사다리 끝내기
        if (isClimbing)
        {
            CheckGround();
        }

    }

    //사다리 오르기
    private void FoxToLadder_Up()
    {
        fox_Move.canMoveOutNav = false;
        Vector3 targetPosition = new Vector3(LadderPosition_Bottom.x, transform.position.y + 0.5f, LadderPosition_Bottom.z) ;
        Vector3 targetRotation = -ladder_Pcol.transform.forward;
        if (rb != null)
        {
            transform.position = targetPosition;
            transform.rotation = Quaternion.LookRotation(targetRotation);

            if (!rb.isKinematic)
            {
                rb.isKinematic = true;
            }
        }
        
    }
   
    //사다리 내려가기
    private void FoxToLadder_Down()
    {
        fox_Move.canMoveOutNav = false;
        Vector3 targetPosition = new Vector3(LadderPosition_Up.x, transform.position.y + 0.5f, LadderPosition_Up.z);
        Vector3 targetRotation = ladder_Pcol.transform.forward;

        transform.position = targetPosition;
        transform.rotation = Quaternion.LookRotation(targetRotation);

    }

    //사다리 착지(아래)
    private void CheckGround()
    {
        
        Vector3 ray = transform.position + Vector3.up * 0.3f;
        float rayDistance = 0.5f;
        RaycastHit hit;

        if (Physics.Raycast(ray, Vector3.down, out hit, rayDistance))
        {
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Ground"))
            {
                Debug.Log("Ground 감지");
                ani.SetBool("isClimb", false);
                isClimbing = false;
                rb.isKinematic = false;
                fox_Move.canMoveOutNav = true;
                isClimb_Up = false;
                isClimb_Down = false;
            }
        }
        Debug.DrawRay(ray, Vector3.down * rayDistance, Color.red);
    }

    //상자
    private void FoxToBox()
    {
        Vector3 targetPosition = new Vector3(Box_center.x, transform.position.y, Box_center.z);
        Vector3 targetRotation = -ladder_Pcol.transform.forward;
        Debug.Log(targetPosition);
        if (rb != null)
        {
            transform.position = targetPosition;
            transform.rotation = Quaternion.LookRotation(targetRotation);
        }
    }

    //Door
    private void FoxToDoor()
    {
        Vector3 targetPosition = new Vector3(Door_center.x, transform.position.y, Door_center.z);
        Vector3 targetRotation = ladder_Pcol.transform.right;
        Debug.Log(targetPosition);
        if (rb != null)
        {
            transform.position = targetPosition;
            transform.rotation = Quaternion.LookRotation(targetRotation);
        }
    }

    // 현재 상자의 ID 가져오기
    private string GetCurrentBoxID()
    {
        return SceneManager.GetActiveScene().name + "_" + ladder_Pcol.gameObject.name;
    }
    // Door ID 가져오기
    private string GetCurrentDoorID()
    { 
    return SceneManager.GetActiveScene().name + "_" + ladder_Pcol.gameObject.name;
    }
    
    //애니메이션 이벤트
    public void Ladder_UpFinish()
    {
        if (!isClimbing)
        {
            fox_Move.canMoveOutNav = true;
        }
    }

    public void Ladder_DownStart()
    {
        rb.isKinematic = true;
    }

    //스폰관련
    public void SetSpacebar(GameObject spacebar)
    {
        this.Spacebar = spacebar;
    }

    private void OnTriggerEnter(Collider other)
    {
        //상호작용
        if (other.gameObject.layer == LayerMask.NameToLayer("Interaction"))
        {
            if (!isClimbing)
            {

                Animator space_ani = Spacebar.GetComponent<Animator>();
                ladder_Pcol = other;
                Spacebar.transform.position = Camera.main.WorldToScreenPoint(other.transform.position + Vector3.up * 2f);
                Spacebar.SetActive(true);
                space_ani.SetTrigger("spacebar_Start");
                isInteraction = true;
                //사다리_UP
                if (other.CompareTag("Ladder"))
                {
                    Collider ladder_Pcol = other.transform.parent.GetComponent<BoxCollider>();
                    if (ladder_Pcol != null)
                    {
                        Debug.Log(isClimb_Up);
                        LadderPosition_Bottom = ladder_Pcol.bounds.center;
                        isClimb_Up = true;
                    }
                }
                else if (other.CompareTag("Ladder_Down"))
                {
                    Collider ladder_Pcol = other.GetComponent<BoxCollider>();
                    if (ladder_Pcol != null)
                    {
                        LadderPosition_Up = ladder_Pcol.bounds.center;
                    }
                    isClimb_Down = true;
                }
                else if (other.CompareTag("SwordBox"))
                {
                    if (!StateManager.instance.GetBoxState(boxID))
                    {
                        Debug.Log("SwordBox 충돌 됐나?");
                        Collider ladder_Pcol = other.transform.GetComponent<BoxCollider>();
                        if (ladder_Pcol != null)
                        {
                            Box_center = ladder_Pcol.bounds.center;

                        }
                        isSwordBox = true;
                        Debug.Log(isSwordBox);
                    }
                }
                else if (other.CompareTag("Page"))
                {
                    isPage = true;
                }
                else if (other.CompareTag("SwitchStone"))
                {
                    Debug.Log("SwitchStone");
                    if (!StateManager.instance.GetDoorState(doorID))
                    {
                        Collider ladder_Pcol = other.transform.GetComponent<BoxCollider>();
                        if (ladder_Pcol != null)
                        {
                            Door_center = ladder_Pcol.bounds.center;
                        }
                        isSwitch = true;
                        Debug.Log("스톤 트리거 완");
                    }
                }
            }
            else
            {
                //사다리_Down
                if (other.CompareTag("Ladder_Finish"))
                {
                    if (isClimbing && rb.isKinematic)
                    {
                        Debug.Log("여기");
                        ani.SetTrigger("isClimb_Off");
                        ani.SetBool("isClimb", false);
                        rb.isKinematic = false;
                        isClimbing = false;
                        isClimb_Up = false;
                        isClimb_Down = false;
                    }

                }

            }

            
        }
       
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Interaction"))
        {
            if (isInteraction)
            {
                Spacebar.SetActive(false);
                isInteraction = false;
            }
           
            //else if (other.CompareTag("Ladder_Down"))
            //{
            //    isClimb_Down = false;
            //}
        }
        if (other.CompareTag("Ladder"))
        {
            isClimb_Up = false;
            Debug.Log(isClimb_Up);
        }
        else if (other.CompareTag("Ladder_Finish"))
        {
            Debug.Log("여기요");
        }
        else if (other.CompareTag("Ladder_Down"))
        {
            isClimb_Down = false;
        }
        else if (other.CompareTag("SwitchStone"))
        {
            Debug.Log("스위치 off");
            isSwitch = false;
        }
    }
}
