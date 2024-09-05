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

    private bool isHurting;
    private bool isAttacking;

    private Coroutine controll_co_I = null; //Idle
    private Coroutine controll_co_W = null; //Walk
    private Coroutine controll_co_F = null; //Find
    private Coroutine controll_co_A = null; //Attack
    private Animator ani;
    private NavMeshAgent nav;
    [SerializeField]public Material mat_Hurt;
    [SerializeField] public Material mat_Base;

    

    private void Awake()
    {
        ani = GetComponent<Animator>();
        nav = GetComponent<NavMeshAgent>();
    
    }
    private void Start()
    {
        isHurting = false;
        isAttacking = false;
        controll_co_F = StartCoroutine(Find_co());
        
    }

    //�����ȿ��� ��� Foxã��
    private IEnumerator Find_co()
    {
        if (isHurting)
        {

            yield return new WaitForSeconds(1f);
            Debug.Log(isHurting);
            isHurting = false;
        }
        while (true)
        {

            float distanceToFox = Vector3.Distance(transform.position, target.position);
         
            if (distanceToFox <= hitDistance && controll_co_A == null)
            {
                if (controll_co_I != null)
                {

                    StopCoroutine(controll_co_I);
                    controll_co_I = null;
                }
                if (controll_co_W != null)
                {

                    StopCoroutine(controll_co_W);
                    controll_co_W = null;
                }
                isAttacking = true;
                controll_co_A = StartCoroutine(Attack_co());
                //Debug.Log("���Դ�?");

            }
            else if (distanceToFox <= targetDistance && controll_co_W == null && controll_co_A == null)
            {
                if (controll_co_I != null)
                {
                    StopCoroutine(controll_co_I);
                    controll_co_I = null;
                }
                controll_co_W = StartCoroutine(Walk_co());
               // Debug.Log("���� ����");
            }
            else if (distanceToFox > targetDistance && controll_co_I == null)
            {
                if (controll_co_W != null)
                {
                    StopCoroutine(controll_co_W);
                    controll_co_W = null;
                }

                controll_co_I = StartCoroutine(Idle_co());
               // Debug.Log("���� ����");
            }
            yield return null;
        }

    }

    //�⺻����
    private IEnumerator Idle_co()
    {
        //�⺻ ������ (�������� �Դ� ����)
        while (true)
        {
            Vector3 randomDirection = Random.insideUnitSphere * 5f; //5f����
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

    //����
    private IEnumerator Walk_co()
    {
        ani.SetBool("isWalk", true);
        while (true)
        {
            nav.SetDestination(target.position);
            yield return null;
        }
    }

    //����
    private IEnumerator Attack_co()
    {
        nav.SetDestination(transform.position);
        ani.SetBool("isWalk", false);
        bool turn = true;
        ani.SetTrigger("attack_1");
        while (turn)
        {
          
                Vector3 movement = target.position - transform.position;
            Debug.Log(Vector3.Angle(transform.forward, movement));
            if (Vector3.Angle(transform.forward, movement) > 10)
            {
                Quaternion toRotation = Quaternion.LookRotation(movement, Vector3.up);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, nav.angularSpeed * 0.8f * Time.deltaTime);
                yield return null;
            }
            else
            {
                //ani.SetTrigger("attack_1");
                turn = false;
            }

            
        }
        
        yield return new WaitForSeconds(2f);
        isAttacking = false;
        yield return new WaitForSeconds(1f);
        controll_co_A = null;
        
    }

    ////Ÿ�� ������ ��
    //private IEnumerator ReturnPosition_co()
    //{ 

    //}

    //�ǰ�
    private void OnTriggerEnter(Collider other)
    {
        Sword sword = other.GetComponent<Sword>();

        if (sword == null) return;

        if (other.CompareTag("Sword"))
        {
            //�̹̼�
            curHealth -= sword.damage;

            if (!isAttacking)
            { 
            
            StopCoroutine(controll_co_F);
            controll_co_F = null;
            isHurting = true;
            if (controll_co_I != null)
            {

                StopCoroutine(controll_co_I);
                controll_co_I = null;
            }
            if (controll_co_W != null)
            {

                StopCoroutine(controll_co_W);
                controll_co_W = null;
            }
            ani.SetBool("isWalk", false);
            ani.SetTrigger("hurt");



            controll_co_F = StartCoroutine(Find_co());
            Debug.Log("Sword : " + curHealth);
            }
        }
    }
}

