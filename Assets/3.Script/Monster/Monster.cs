using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Monster : MonoBehaviour
{
    [SerializeField] public GameObject Monster_Sword;

    public int maxHealth; //최대체력
    public int curHealth; //현재체력
    public int damage  = 4;
    public float distanceRadius;
    public float hitDistance; //공격 범위
    private bool isAttacking = false;
    private bool isHurting;
    

    public Transform target;
    private Rigidbody rigid;
    private CapsuleCollider capsul;
    private Material mat; //피격 시
    private NavMeshAgent nav;
    private Animator ani;
    
    
    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        capsul = GetComponent<CapsuleCollider>();
        nav = GetComponent<NavMeshAgent>();
        ani = GetComponent<Animator>();
       
        
    }
    
    private void Update()
    {
        float distanceToFox = Vector3.Distance(transform.position, target.position);
        if (distanceToFox <= distanceRadius && !ani.GetBool("isWalk"))
        { 
        Find_Fox();
        }
        if (isAttacking) return;
        Run_Start();
        
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
    
   

    private void Find_Fox()
    {
            float distanceToFox = Vector3.Distance(transform.position, target.position);
            //if (distanceToFox <= distanceRadius)
            RaycastHit hit;
            //Vector3 FindToTarget = (target.position - transform.position).normalized;
            Vector3 back = transform.forward * -1;
            Vector3 right = transform.right;
            Vector3 left = transform.right * -1;
            Debug.DrawRay(transform.position, back * distanceRadius, Color.red);
            Debug.DrawRay(transform.position, right * distanceRadius, Color.red);
            Debug.DrawRay(transform.position, left * distanceRadius, Color.red);
            
        if (Physics.Raycast(transform.position, back, out hit, distanceRadius) ||
            Physics.Raycast(transform.position, right, out hit, distanceRadius) ||
            Physics.Raycast(transform.position, left, out hit, distanceRadius))
            {
                if (hit.collider.CompareTag("Fox") && distanceToFox <= distanceRadius)
                {
                    ani.SetTrigger("turn");
                    ani.SetBool("isWalk", true);
                }
            }   
    }

    private void Run_Start()
    {
        
        if (ani.GetBool("isWalk"))
        {
            nav.speed = 6;
            ani.SetBool("isWalk", true);
            nav.enabled = true;
            nav.SetDestination(target.position);
        }
        float distanceToFox = Vector3.Distance(transform.position, target.position);
        RaycastHit hit;
        Vector3 front = transform.forward;
        Debug.DrawRay(transform.position, front * hitDistance, Color.blue);

        if (Physics.Raycast(transform.position, front, out hit, hitDistance))
        {
            ani.SetBool("isWalk", false);
            ani.SetTrigger("attack_1");
            isAttacking = true;
            nav.speed = 0;
        }
    }
  
    //애니메이션 이벤트
    public void Start_run()
    {
        ani.SetBool("isWalk", true);
        Run_Start();
    }
    public void Agent_Start()
    {
        nav.enabled = true;
        nav.SetDestination(target.position);
    }
    
    public void Attack_Finish()
    {
        float distanceToFox = Vector3.Distance(transform.position, target.position);
        if (distanceToFox <= distanceRadius)
        {
            isAttacking = false;
            ani.SetBool("isWalk", true);
            //nav.SetDestination(target.position);
        }
    }



    //피격
    private void OnTriggerEnter(Collider other)
    {

        Sword sword = other.GetComponent<Sword>();

        if (sword == null) return;
        

        if (other.CompareTag("Sword"))
        {
            curHealth -= sword.damage;
            ani.SetTrigger("hurt");
            nav.enabled = false;
            isAttacking = false;
            Debug.Log("Sword : " + curHealth);
            if (curHealth == -100)
            {
                //ani.SetTrigger("die");
            }
            else
            {

                StartCoroutine(RecoverFromHurt());
            }
            
        }

    }
        private IEnumerator RecoverFromHurt()
        {
            // 피격 후 약간의 대기 시간
            yield return new WaitForSeconds(1.0f);
            // 다시 타겟 추적 시작
            //ani.SetBool("isWalk", true);
        
        Run_Start();
            
        }
}