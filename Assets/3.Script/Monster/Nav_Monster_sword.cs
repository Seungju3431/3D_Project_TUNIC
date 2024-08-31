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
        //현재 animator 상태정보
        var curAniStateInfo = ani.GetCurrentAnimatorStateInfo(0);

        //애니메이션 이름에 따른
        if (curAniStateInfo.IsName("Idle") == false)
        {
            ani.Play("Idle", 0, 0);
        }

        //몬스터 Idle 상태에서 두리번 두리번 거리게 (50% 확률)
        int dir = Random.Range(0f, 1f) > 0.5f ? 1 : -1;

        //회전 속도
        float lookSpeed = Random.Range(25f, 40f);

        //Idle 재생 시간 동안 돌아보기
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

            //한 프레임 넘기기 위해
            yield return null;
        }

        //목표까지의 남은 거리가 멈추는 지점보다 작거나 같으면
        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            //StateMachine을 공격으로 변경
            ChangeState(State.ATTACK);
        }
        //목표와의 거리가 멀어진 경우
        else if (agent.remainingDistance > lostDistance)
        {
            target = null;
            agent.SetDestination(transform.position);
            yield return null;

            //StateMachine을 대기로 변경
            ChangeState(State.IDLE);
        }
        else
        {
            //Walk 애니메이션의 한 사이클 동안 대기
            yield return new WaitForSeconds(curAniStateInfo.length);
        }
    }

    private IEnumerator Attack()
    {
        var curAniStateInfo = ani.GetCurrentAnimatorStateInfo(0);

        ani.Play("Attack", 0, 0);

        //거리 멀어지면
        if (agent.remainingDistance > agent.stoppingDistance)
        {
            //StateMachine을 추적으로 변경
            ChangeState(State.CHASE);
        }
        else
            //공격 animation의 두 배만큼 대기
            //나중에 간격 조절하기
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

        //Fox를 감지하면
        target = other.transform;

        //NavMeshAgent의 목표를 Fox로 설정
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
