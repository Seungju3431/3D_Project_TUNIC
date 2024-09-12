using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fox_manage : MonoBehaviour
{
    [SerializeField] private GameObject Spacebar;
    

    public int maxHealth;
    public int curHealth;

    private Animator ani;


    private bool isHurt = false;
    public bool isInteraction = false;

    private void Awake()
    {
        
        ani = GetComponent<Animator>();
        if (ani == null)
        {
            Debug.LogError("Animator 컴포넌트가 할당되지 않았습니다!");
        }
    }

    private void Start()
    {
        curHealth = maxHealth;
    }

    private void Update()
    {
        if (isHurt)
        {
            isHurt = false;
        }
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Interaction"))
        {
            Animator space_ani = Spacebar.GetComponent<Animator>();
            Spacebar.transform.position = Camera.main.WorldToScreenPoint(other.transform.position + Vector3.up * 2f);
            Spacebar.SetActive(true);
            space_ani.SetTrigger("spacebar_Start");

            if (other.CompareTag("Ladder"))
            {
                if (isInteraction && Input.GetKeyDown(KeyCode.Space))
                {
                    isInteraction = true;
                    ani.SetBool("isClimb", true);
                }
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
            //isInteraction = false;

            //if (other.CompareTag("Ladder"))
            //{ 

            //}
        }
    }


}
