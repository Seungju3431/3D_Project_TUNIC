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
    private Rigidbody rb;
    

    private bool isHurt = false;
    public bool isInteraction;
    public bool isClimbing = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        ani = GetComponent<Animator>();
        if (ani == null)
        {
            Debug.LogError("Animator 컴포넌트가 할당되지 않았습니다!");
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
            //사다리 올라가기
            if (Input.GetKeyDown(KeyCode.Space) && isInteraction && Spacebar.activeSelf && !rb.isKinematic)
            {
                FoxToLadder();
                isClimbing = true;
                Spacebar.SetActive(false);
                ani.SetBool("isClimb", true);
                
            }
        }
        else
        {
            ani.SetBool("isClimb", false);
            isClimbing = false;
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Interaction"))
        {
            Animator space_ani = Spacebar.GetComponent<Animator>();
            ladder_Pcol = other;
            Spacebar.transform.position = Camera.main.WorldToScreenPoint(other.transform.position + Vector3.up * 2f);
            Spacebar.SetActive(true);
            space_ani.SetTrigger("spacebar_Start");
            isInteraction = true;

            //사다리
            if (other.CompareTag("Ladder"))
            {
                Collider ladder_Pcol = other.transform.parent.GetComponent<BoxCollider>();
                if (ladder_Pcol != null)
                {
                    LadderPosition_Bottom = ladder_Pcol.bounds.center;
                }
                Debug.Log("Ladder Position Bottom: " + LadderPosition_Bottom);
                

            }
        }

        if (other.CompareTag("Monster_Attack"))
        {
            MonsterController monster_Attack = other.transform.parent.GetComponent<MonsterController>();
            curHealth -= monster_Attack.damage;
            Debug.Log("현재체력 : " + curHealth);
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
            Spacebar.SetActive(false);
            isInteraction = false;

            if (other.CompareTag("Ladder"))
            {
                if (!isClimbing)
                { 
                
                ladder_Pcol = null;
                fox_Move.canMoveOutNav = true;
                }
                //isClimbing = false;
            }
        }
    }

    private void FoxToLadder()
    {
        fox_Move.canMoveOutNav = false;
        Vector3 targetPosition = new Vector3(LadderPosition_Bottom.x, transform.position.y + 0.5f, LadderPosition_Bottom.z) ;
        
        if (rb != null)
        {
            transform.position = targetPosition;
            
            if (!rb.isKinematic)
            {
                rb.isKinematic = true;
            }
        }
        //Debug.Log("위치이동" + targetPosition);
    }

}
