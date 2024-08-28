using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fox_Move : MonoBehaviour
{
    private Rigidbody rb;
    private Animator animator;

    [SerializeField] public float moveSpeed = 5f;
    [SerializeField] public float rotationSpeed = 700f;
    private GameObject targetMonster;

    private bool isDodgeing = false;
    private bool isInput;
    
    private void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        
        //������ �� �ٸ� Ű �Է� x
        isInput = true;
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector3(0f, rb.velocity.y, 0f);
    }

    private void Update()
    {
        Focus_Monster();
        float moveHorizontal = Input.GetAxisRaw("Horizontal");
        float moveVertical = Input.GetAxisRaw("Vertical");
        animator.SetFloat("move_right", moveHorizontal);
        animator.SetFloat("move_forward", moveVertical);

        if (Input.GetKeyUp(KeyCode.Space)) //�޸��� ���߱�(Update �ٽ� ���ư� ��, ��ܿ��� ���� ���� Ȯ��)
        {
            animator.SetBool("isSprint", false);
        }

        if (isDodgeing) return;

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical).normalized;

        // �̵� ó��
            rb.MovePosition(transform.position + movement * moveSpeed * Time.deltaTime);

        // �÷��̾� ȸ�� ó��
        if (movement != Vector3.zero&&!animator.GetBool("isFocus"))
        {
            Quaternion toRotation = Quaternion.LookRotation(movement, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
        }

        //���� ����
        if (Input.GetKey(KeyCode.LeftShift))
        {
            animator.SetBool("isFocus", true);

            if (targetMonster != null && animator.GetBool("isFocus"))
            {
                // ���� �������� ȸ��
                Quaternion toRotation = Quaternion.LookRotation(targetMonster.transform.position - transform.position, Vector3.up);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
            }
        }
        else
        {
            animator.SetBool("isFocus", false);
        }

        //������
        if (isInput && Input.GetKeyDown(KeyCode.Space))
        {
            //animator.applyRootMotion = true;
            isDodgeing = true;
            isInput = false;
            animator.SetTrigger("dodge");
        }

        //�޸���
        if (Input.GetKey(KeyCode.Space))
        {
            animator.SetBool("isSprint", true);
        }
       

    }

    //�ִϸ��̼� �̺�ƮŬ��
    #region
    public void Dodge_Input_Finish()
    {
        isInput = true;
    }
    public void Dodge_Finish()
    {
        isDodgeing = false;
    }
    #endregion

    private void Focus_Monster()
    {
        if (animator.GetBool("isFocus") && targetMonster != null)
        {
            return;
        }

        GameObject[] monsters = GameObject.FindGameObjectsWithTag("Monster");

        float nearMonsters = Mathf.Infinity;
        foreach (GameObject monster in monsters)
        {
            float distance = Vector3.Distance(transform.position, monster.transform.position);
            if (distance < nearMonsters)
            {
                nearMonsters = distance;
                targetMonster = monster;
            }
        }
    }
}

