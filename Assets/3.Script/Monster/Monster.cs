using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Monster : MonoBehaviour
{
    public int maxHealth; //최대체력
    public int curHealth; //현재체력
    public float distanceRadius;
    private bool isChase;
    

    public Transform target;
    private Rigidbody rigid;
    private CapsuleCollider capsul;
    private Material mat; //피격 시
    private NavMeshAgent nav;
    private Animator ani;
    private Coroutine findCo = null;
    
    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        capsul = GetComponent<CapsuleCollider>();
       
        nav = GetComponent<NavMeshAgent>();
        ani = GetComponent<Animator>();
       
        
    }
    
    private void Update()
    {
        if(findCo == null)
        findCo =StartCoroutine(Find_Fox());
        
        
    }
    
    private void FixedUpdate()
    {
        FreezeVelocity();
    }
    private void FreezeVelocity()
    {
        //물리력으로 NavAgent 방해하지 않게 하기 위해
        rigid.velocity = Vector3.zero;
        rigid.angularVelocity = Vector3.zero;
    }
    
    private IEnumerator Find_Fox()
    {
        if (target != null)
        {
            float distanceToFox = Vector3.Distance(transform.position, target.position);

            //if (distanceToFox <= distanceRadius)
            
                RaycastHit hit;
            //Vector3 FindToTarget = (target.position - transform.position).normalized;
            Vector3 ray = transform.forward * -1;
            Debug.DrawRay(transform.position, ray * distanceRadius, Color.red);
                if (Physics.Raycast(transform.position, ray, out hit, distanceRadius))
                {
                    if (hit.collider.CompareTag("Fox"))
                    {
                    if (distanceToFox <= distanceRadius)
                    { 
                    
                        nav.enabled = false;
                        ani.SetTrigger("turn");
                    }
                    }
                }
            
        }
            
        yield return null;
    }

    //애니메이션 이벤트
    public void Start_run()
    {
        ani.SetBool("isWalk", true);
    }
    public void Agent_Start()
    {
        nav.enabled = true;
        nav.SetDestination(target.position);
    }

    private void OnTriggerEnter(Collider other)
    {

        Sword sword = other.GetComponent<Sword>();

        if (sword == null) return;
        

        if (other.CompareTag("Sword"))
        {
            curHealth -= sword.damage;
            Debug.Log("Sword : " + curHealth);
            if (curHealth <= 0)
            {
                //ani.SetTrigger("die");
            }
        }
    }
}