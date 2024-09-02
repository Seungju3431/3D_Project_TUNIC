using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Monster : MonoBehaviour
{
    public int maxHealth; //�ִ�ü��
    public int curHealth; //����ü��
    public float distanceRadius;
    private bool isChase;
    

    public Transform target;
    private Rigidbody rigid;
    private CapsuleCollider capsul;
    private Material mat; //�ǰ� ��
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
        //���������� NavAgent �������� �ʰ� �ϱ� ����
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

    //�ִϸ��̼� �̺�Ʈ
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