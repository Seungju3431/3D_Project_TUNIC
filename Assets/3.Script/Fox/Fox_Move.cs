using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fox_Move : MonoBehaviour
{
    private Rigidbody rb;
    private Animator animator;
    private GameObject targetMonster;

    [SerializeField] public float moveSpeed;
    [SerializeField] public float rotationSpeed;
    [SerializeField] public GameObject Sword;
    [SerializeField] public GameObject HPPotion;
    [SerializeField] public ParticleSystem Particle_Potion;
    [SerializeField] public ParticleSystem Particle_DodgeLess;

    private float maxStamina = 40f; //���׹̳� �ִ�
    private float nowStamina = 40f; //���� ���׹̳�
    private float dodge_Stamina = 10f; //������ �Ҹ� ���׹̳�
    private float staminaRate = 20f; //���׹̳� ȸ��
    private float staminaRateDelay = 1.5f;
    private float lastActionTime = 0f;//���ݽð� ����

    private bool isDodgeing = false;
    private bool isDodgeLessing = false;
    private bool isSwing = false;
    private bool isInput;


    private void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        isInput = true;
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector3(0f, rb.velocity.y, 0f);
    }

    private void Update()
    {
        //���׹̳� ȸ��
        if (!isSwing && !isDodgeing && Time.time >= lastActionTime + staminaRateDelay
            && nowStamina < maxStamina)
        {
            nowStamina += staminaRate * Time.deltaTime;
            if (nowStamina > maxStamina)
            {
                nowStamina = maxStamina;
            }
            //Debug.Log("���� ���׹̳�" + nowStamina);
        }



        Focus_Monster();

        float moveHorizontal = Input.GetAxisRaw("Horizontal");
        float moveVertical = Input.GetAxisRaw("Vertical");
        animator.SetFloat("move_right", moveHorizontal);
        animator.SetFloat("move_forward", moveVertical);

        if (Input.GetKeyUp(KeyCode.Space)) //�޸��� ���߱�(Update �ٽ� ���ư� ��, ��ܿ��� ���� ���� Ȯ��)
        {
            animator.SetBool("isSprint", false);
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

        //����
        if (isInput && Input.GetKeyDown(KeyCode.J))
        {
            isSwing = true;
            animator.SetTrigger("swing_sword");
            lastActionTime = Time.time;
        }

        if (isDodgeing || isSwing || isDodgeLessing) return;


        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical).normalized;

        // �̵� ó��
        rb.MovePosition(transform.position + movement * moveSpeed * Time.deltaTime);

        // �÷��̾� ȸ�� ó��
        if (movement != Vector3.zero && !animator.GetBool("isFocus"))
        {
            Quaternion toRotation = Quaternion.LookRotation(movement, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
        }



        //������
        if (isInput && Input.GetKeyDown(KeyCode.Space))
        {
            if (nowStamina >= dodge_Stamina)
            {
                //animator.applyRootMotion = true;
                isDodgeing = true;
                isInput = false;
                nowStamina -= dodge_Stamina; //���׹̳� �Ҹ�
                animator.SetTrigger("dodge");
                lastActionTime = Time.time;
            }

        }

        //���׹̳� 0 �� ��, ������
        if (nowStamina <= 9 && isInput && Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("���׹̳ʰ� �����մϴ�.");
            isDodgeLessing = true;
            isInput = false;
            animator.SetTrigger("dodge_less");
            lastActionTime = Time.time;
        }

        //�޸���
        if (Input.GetKey(KeyCode.Space))
        {
            animator.SetBool("isSprint", true);
        }

        //HP ����
        if (Input.GetKeyDown(KeyCode.P))
        {
            //animator.SetLayerWeight(1, 0.78f);
            animator.SetBool("isPotion", true);

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
    public void DodgeLess_Start()
    {
        Particle_DodgeLess.Play();
    }
    public void DodegeLess_Finish()
    {
        Particle_DodgeLess.Stop();
        isDodgeLessing = false;
        isInput = true;
    }
    public void Swing_Finish()
    {
        isSwing = false;
    }

    public void SwingCombo_Finish()
    {
        animator.ResetTrigger("swing_sword");
    }
    public void Potioning_Start()
    {
        HPPotion.SetActive(true);
        Sword.SetActive(false);
        moveSpeed = 15;
        Particle_Potion.Play();//��ƼŬ ���
    }

    public void Potioning_Finish()
    {
        HPPotion.SetActive(false);
        Sword.SetActive(true);
        moveSpeed = 30f;
        animator.SetBool("isPotion", false);

    }
    #endregion
    public void Hurt_Bool()
    {
        isSwing = false;
        isDodgeing = false;
        isDodgeLessing = false;
    }
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

