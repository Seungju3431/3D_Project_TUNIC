using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

    enum State
    { 
    IDLE,
    CHASE,
    ATTACK,
    KILLED
    }
public class Nav_Monster_sword : MonoBehaviour
{
    State state;
    public Transform target;
    private NavMeshAgent agent;
    private Animator ani;

    private float HP = 0;
    public float lostDistance;

    private void Start()
    {
        ani = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();

        HP = 10;
        state = State.IDLE;
        StartCoroutine(StateMachine_co());
    }

    private IEnumerator StateMachine_co()
    {
        while (HP > 0)
        {
            yield return StartCoroutine(state.ToString());
        }
    }

    private IEnumerator IDLE()
    {
        //���� animator ��������
        var curAniStateInfo = ani.GetCurrentAnimatorStateInfo(0);

        //�ִϸ��̼� �̸��� ����
        if (curAniStateInfo.IsName("Idle") == false)
        {
            ani.Play("Idle", 0, 0);
        }

        //���� Idle ���¿��� �θ��� �θ��� �Ÿ��� (50% Ȯ��)
        int dir = Random.Range(0f, 1f) > 0.5f ? 1 : -1;

        //ȸ�� �ӵ�
        float lookSpeed = Random.Range(25f, 40f);

        //Idle ��� �ð� ���� ���ƺ���
        for (float i = 0; i < curAniStateInfo.length; i += Time.deltaTime)
        {
            transform.localEulerAngles =
                    new Vector3(0f, transform.localEulerAngles.y + (dir) * Time.deltaTime * lookSpeed, 0f);

            yield return null;
        }
    }

    private IEnumerator CHASE()
    {
        var curAniStateInfo = ani.GetCurrentAnimatorStateInfo(0);

        if (curAniStateInfo.IsName("Walk") == false)
        {
            ani.Play("Walk", 0, 0);

            //�� ������ �ѱ�� ����
            yield return null;
        }

        //��ǥ������ ���� �Ÿ��� ���ߴ� �������� �۰ų� ������
        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            //StateMachine�� �������� ����
            ChangeState(State.ATTACK);
        }
        //��ǥ���� �Ÿ��� �־��� ���
        else if (agent.remainingDistance > lostDistance)
        {
            target = null;
            agent.SetDestination(transform.position);
            yield return null;

            //StateMachine�� ���� ����
            ChangeState(State.IDLE);
        }
        else
        {
            //Walk �ִϸ��̼��� �� ����Ŭ ���� ���
            yield return new WaitForSeconds(curAniStateInfo.length);
        }
    }

    private IEnumerator Attack()
    {
        var curAniStateInfo = ani.GetCurrentAnimatorStateInfo(0);

        ani.Play("Attack", 0, 0);

        //�Ÿ� �־�����
        if (agent.remainingDistance > agent.stoppingDistance)
        {
            //StateMachine�� �������� ����
            ChangeState(State.CHASE);
        }
        else
            //���� animation�� �� �踸ŭ ���
            //���߿� ���� �����ϱ�
            yield return new WaitForSeconds(curAniStateInfo.length * 2f);
    }

    private IEnumerator KILLED()
    {
        yield return null;
    }

    private void ChangeState(State newState)
    {
        state = newState;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name != "_Fox") return;

        //Fox�� �����ϸ�
        target = other.transform;

        //NavMeshAgent�� ��ǥ�� Fox�� ����
        agent.SetDestination(target.position);
        ChangeState(State.CHASE);
    }

    private void Update()
    {
        if (target == null)
            return;
        agent.SetDestination(target.position);
    }
}
