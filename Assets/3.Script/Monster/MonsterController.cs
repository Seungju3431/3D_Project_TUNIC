using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterController : MonoBehaviour
{
    [SerializeField] public Transform target;
    public float targetDistance;
    public float hitDistance;

    public int damage;
    public int maxHealth;
    public int curHealth;



    private Coroutine controll_co_I = null;
    private Coroutine controll_co_W = null;
    private Coroutine controll_co_F = null;
    private Animator ani;
    private NavMeshAgent nav;
    private Rigidbody rigid;

    private void Awake()
    {
        ani = GetComponent<Animator>();
        nav = GetComponent<NavMeshAgent>();
        rigid = GetComponent<Rigidbody>();
    }
    private void Start()
    {
        controll_co_F = StartCoroutine(Find_co());
        controll_co_I = StartCoroutine(Idle_co());

    }
    private void Update()
    {


    }

    //범위안에서 계속 Fox찾기
    private IEnumerator Find_co()
    {
        while (true)
        {
            float distanceToFox = Vector3.Distance(transform.position, target.position);
            if (distanceToFox <= targetDistance && controll_co_W == null)
            {
                if (controll_co_I != null)
                {
                    StopCoroutine(controll_co_I);
                    controll_co_I = null;
                }
                controll_co_W = StartCoroutine(Walk_co());
                Debug.Log("추적 시작");
            }
            else if (distanceToFox > targetDistance && controll_co_I == null)
            {
                if (controll_co_W != null)
                {
                    StopCoroutine(controll_co_W);
                    controll_co_W = null;
                }

                controll_co_I = StartCoroutine(Idle_co());
                Debug.Log("추적 멈춤");

            }
            yield return null;

        }

    }

    //기본상태
    private IEnumerator Idle_co()
    {
        //기본 움직임 (랜덤으로 왔다 갔다)
        while (true)
        {
            Vector3 randomDirection = Random.insideUnitSphere * 5f; //5f범위
            randomDirection += transform.position;
            NavMeshHit hit;
            NavMesh.SamplePosition(randomDirection, out hit, 5f, 1);
            Vector3 finalPosition = hit.position;

            nav.SetDestination(finalPosition);
            ani.SetBool("isWalk", true);

            while (nav.pathPending || nav.remainingDistance > 0.5f)
            {

                yield return null;
            }

            ani.SetBool("isWalk", false);
            yield return new WaitForSeconds(Random.Range(2f, 5f));

        }
    }

    //추적
    private IEnumerator Walk_co()
    {
        ani.SetBool("isWalk", true);
        while (true)
        {
            nav.SetDestination(target.position);
            yield return null;
        }
    }

    //공격
    private IEnumerator Attack_co()
    {
        float distanceToFox = Vector3.Distance(transform.position, target.position);
        while (true)
        { 
        if (distanceToFox >= hitDistance)
        {
                ani.SetTrigger("attack_1");
        }
        
        }
    }

    ////타겟 놓쳤을 때
    //private IEnumerator ReturnPosition_co()
    //{ 

    //}

    //피격
    private void OnTriggerEnter(Collider other)
    {
        Sword sword = other.GetComponent<Sword>();

        if (sword == null) return;

        if (other.CompareTag("Sword"))
        {
            curHealth -= sword.damage;
            ani.SetTrigger("hurt");
            if (controll_co_I == null && controll_co_W != null)
            {
                StopCoroutine(controll_co_I);
                StopCoroutine(controll_co_W);
            }
            Debug.Log("Sword : " + curHealth);
        }
    }




}
