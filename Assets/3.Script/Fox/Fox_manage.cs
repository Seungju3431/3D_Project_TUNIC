using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fox_manage : MonoBehaviour
{
    public GameObject Spacebar;
    private Fox_Move fox_Move;
    public int maxHealth;
    public int curHealth;

    private Animator ani;
    private Collider ladder_Pcol;
    private Vector3 LadderPosition_Bottom;
    private Vector3 LadderPosition_Up;
    private Rigidbody rb;
    

    private bool isHurt = false;
    public bool isInteraction;
    public bool isClimbing = false;
    //��ٸ� ��/�Ʒ� ����
    private bool isClimb_Up;
    public bool isClimb_Down;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        ani = GetComponent<Animator>();
        if (ani == null)
        {
            Debug.LogError("Animator ������Ʈ�� �Ҵ���� �ʾҽ��ϴ�!");
        }
    }

    private void Start()
    {
        fox_Move = GetComponent<Fox_Move>();
        curHealth = maxHealth;
    }

    private void Update()
    {
        if (isHurt)
        {
            isHurt = false;
        }
        

        if (ladder_Pcol != null)
        {
            //��ٸ� �ö󰡱�
            if (Input.GetKeyDown(KeyCode.Space) && isInteraction && Spacebar.activeSelf && !rb.isKinematic)
            {
                if (isClimb_Up)
                {

                    FoxToLadder_Up();
                isClimbing = true;
                Spacebar.SetActive(false);
                ani.SetBool("isClimb", true);
                }

                if (isClimb_Down)
                {
                    FoxToLadder_Down();
                    ani.SetTrigger("isClimb_On");
                    isClimbing = true;
                    ani.SetBool("isClimb", true);

                }
               
            }
        }

        //��ٸ� ������
        if (isClimbing)
        {
            CheckGround();
        }

    }


    private void OnTriggerEnter(Collider other)
    {
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
                //��ٸ�_UP
                if (other.CompareTag("Ladder"))
                {
                    Collider ladder_Pcol = other.transform.parent.GetComponent<BoxCollider>();
                    if (ladder_Pcol != null)
                    {
                        LadderPosition_Bottom = ladder_Pcol.bounds.center;
                        isClimb_Up = true;
                    }
                }
                if (other.CompareTag("Ladder_Down"))
                {
                    Collider ladder_Pcol = other.GetComponent<BoxCollider>();
                    if (ladder_Pcol != null)
                    {
                        LadderPosition_Up = ladder_Pcol.bounds.center;
                    }
                    isClimb_Down = true;
                }
            }
            else
            {
                //��ٸ�_Down
                if (other.CompareTag("Ladder_Finish"))
                {
                    Debug.Log("d");
                    if (isClimbing && rb.isKinematic)
                    {
                        ani.SetTrigger("isClimb_Off");
                        ani.SetBool("isClimb", false);
                        rb.isKinematic = false;
                        isClimbing = false;
                        isClimb_Up = false;
                    }
                    
                }
                    
            }
            


        }


        if (other.CompareTag("Monster_Attack"))
        {
            MonsterController monster_Attack = other.transform.parent.GetComponent<MonsterController>();
            curHealth -= monster_Attack.damage;
            Debug.Log("����ü�� : " + curHealth);
            ani.SetTrigger("hurt");
            isHurt = true;
            Fox_Move fox_Move = GetComponent<Fox_Move>();
            fox_Move.Hurt_Bool();
            if (curHealth == 0)
            {
                ani.SetTrigger("die");
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
            
        }
    }

    //��ٸ� ������
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

    //��ٸ� ��������
    private void FoxToLadder_Down()
    {
        fox_Move.canMoveOutNav = false;
        Vector3 targetPosition = new Vector3(LadderPosition_Up.x, transform.position.y + 0.5f, LadderPosition_Up.z);
        Vector3 targetRotation = ladder_Pcol.transform.forward;

        transform.position = targetPosition;
        transform.rotation = Quaternion.LookRotation(targetRotation);

    }

    //��ٸ� ����(�Ʒ�)
    private void CheckGround()
    {
        
        Vector3 ray = transform.position + Vector3.up * 0.3f;
        float rayDistance = 0.5f;
        RaycastHit hit;

        if (Physics.Raycast(ray, Vector3.down, out hit, rayDistance))
        {
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Ground"))
            {
                Debug.Log("Ground ����");
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

    //�ִϸ��̼� �̺�Ʈ
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
}
