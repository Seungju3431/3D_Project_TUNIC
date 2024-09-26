using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fox_Move : MonoBehaviour
{
    private Rigidbody rb;
    private Animator animator;
    private GameObject targetMonster;
    private Nav_Move navMove;
    private Fox_manage fox_Manage;
    private Vector3 position_Fox;


    [SerializeField] public float moveSpeed;
    [SerializeField] public float rotationSpeed;
    [SerializeField] public GameObject Sword;
    [SerializeField] public GameObject HPPotion;
    [SerializeField] public ParticleSystem Particle_Potion;
    [SerializeField] public ParticleSystem Particle_DodgeLess;

    
    private float lastActionTime = 0f;//���ݽð� ����

    public bool canMoveOutNav;
    public bool onNav = false;
    private bool isDodgeing = false;
    private bool isDodgeLessing = false;
    private bool isSwing = false;
    public bool isInput;
    //private bool canRecoverStamina = true;


    private void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        isInput = true;
        navMove = GetComponent<Nav_Move>();
        fox_Manage = GetComponent<Fox_manage>();
        animator.applyRootMotion = true;
    }

    //private void FixedUpdate()
    //{
    //    //rb.velocity = new Vector3(0f, rb.velocity.y, 0f);

    //}

    private void Update()
    {

        if (canMoveOutNav)
        {

            if (navMove.IsOnNavMesh(transform.position))
            {
                if (!onNav)
                    position_Fox = transform.position;

                if (onNav)
                {
                    onNav = false;

                }
            }
            else
            {
                if (!onNav)
                {
                    onNav = true;
                }
            }
        }
        
        //���׹̳� ȸ��
        //if (canRecoverStamina && !isSwing && !isDodgeing && FoxManager.Instance.nowStamina < FoxManager.Instance.maxStamina)
        if(!isSwing && !isDodgeing && Time.time >= lastActionTime + FoxManager.Instance.staminaRateDelay
            && FoxManager.Instance.nowStamina < FoxManager.Instance.maxStamina)
        {
            Debug.Log("���׹̳� ȸ��");
            FoxManager.Instance.nowStamina += FoxManager.Instance.staminaRate * Time.deltaTime;

            if (FoxManager.Instance.nowStamina > FoxManager.Instance.maxStamina)
            {
                FoxManager.Instance.nowStamina = FoxManager.Instance.maxStamina;
            }
        }
        //FoxManager.Instance.RecoverStamina(Time.deltaTime);
        
        ////���׹̳� ȸ��
        //if (!isSwing && !isDodgeing && Time.time >= lastActionTime + staminaRateDelay
        //    && nowStamina < maxStamina)
        //{
        //    nowStamina += staminaRate * Time.deltaTime;
        //    if (nowStamina > maxStamina)
        //    {
        //        nowStamina = maxStamina;
        //    }
        //    //Debug.Log("���� ���׹̳�" + nowStamina);
        //}

        Focus_Monster();


        float moveHorizontal = Input.GetAxisRaw("Horizontal");
        float moveVertical = Input.GetAxisRaw("Vertical");
        animator.SetFloat("move_right", moveHorizontal);
        animator.SetFloat("move_forward", moveVertical);

        if (!fox_Manage.isClimbing)
        {

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
            if (isInput && Input.GetKeyDown(KeyCode.J) && !fox_Manage.isInteraction && !fox_Manage.isClimbing)
            {
                isSwing = true;
                animator.SetTrigger("swing_sword");
                lastActionTime = Time.time;
            }

            if (isDodgeing || isSwing || isDodgeLessing) return;


            Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical).normalized;
            //Vector3 newPosition = /*transform.position +*/ movement * moveSpeed * Time.deltaTime;
            Vector3 newPosition = movement * moveSpeed * Time.deltaTime;
            // �̵� ó��
            //rb.MovePosition(transform.position + movement * moveSpeed * Time.deltaTime);
            rb.MovePosition(rb.position + newPosition);
            //transform.Translate(newPosition);


            // �÷��̾� ȸ�� ó��
            if (movement != Vector3.zero && !animator.GetBool("isFocus") && !fox_Manage.isClimbing)
            {
                Quaternion toRotation = Quaternion.LookRotation(movement, Vector3.up);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
            }





            //������
            if (isInput && Input.GetKeyDown(KeyCode.Space)
                && !fox_Manage.isInteraction && !fox_Manage.isClimbing)
            {
                if (FoxManager.Instance.nowStamina >= FoxManager.Instance.dodge_Stamina)
                {
                    isDodgeing = true;
                    isInput = false;
                    FoxManager.Instance.nowStamina -= FoxManager.Instance.dodge_Stamina; // ���׹̳� �Ҹ�
                    lastActionTime = Time.deltaTime;
                    animator.SetTrigger("dodge");
                    //Invoke("StaminaRecovery", FoxManager.Instance.staminaRateDelay);
                    //lastActionTime = Time.time;
                }
                else
                {
                    Debug.Log("���׹̳ʰ� �����մϴ�.");
                    isDodgeLessing = true;
                    isInput = false;
                    animator.SetTrigger("dodge_less");
                    lastActionTime = Time.time;
                }
            }

            //���׹̳� 0 �� ��, ������
            if (FoxManager.Instance.nowStamina <= 9 && isInput && Input.GetKeyDown(KeyCode.Space) && !fox_Manage.isInteraction && !fox_Manage.isClimbing)
            {
                Debug.Log("���׹̳ʰ� �����մϴ�.");
                isDodgeLessing = true;
                isInput = false;
                animator.SetTrigger("dodge_less");
                lastActionTime = Time.time;
            }

            //�޸���
            if (Input.GetKey(KeyCode.Space) && !fox_Manage.isInteraction && !fox_Manage.isClimbing)
            {
                animator.SetBool("isSprint", true);
            }

            //HP ����
            if (Input.GetKeyDown(KeyCode.P) && !fox_Manage.isInteraction && !fox_Manage.isClimbing)
            {
                //animator.SetLayerWeight(1, 0.78f);
                animator.SetBool("isPotion", true);

            }
        }
        else
        {
            animator.SetFloat("climbSpeed", moveVertical * 2f);
            transform.position += moveVertical * transform.up *Time.deltaTime * 4f;
        }

    }

    private void OnAnimatorMove()
    {
        if (animator.applyRootMotion)
        {

            Vector3 newPosition = animator.rootPosition;
            Quaternion newRotation = animator.rootRotation;
            if (rb.isKinematic)
            {
                rb.AddForce(Vector3.down * 5, ForceMode.Acceleration);
                newPosition.y = rb.position.y;
            }
            rb.position = newPosition;
            rb.rotation = newRotation;
        }
    }

    public void Initialize()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        isInput = true;
        navMove = GetComponent<Nav_Move>();
        fox_Manage = GetComponent<Fox_manage>();
        animator.applyRootMotion = true;
    }

    private void LateUpdate()
    {

        if (onNav)
        {
            transform.position = position_Fox;
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
        moveSpeed = 3.5f;
        Particle_Potion.Play();//��ƼŬ ���
    }

    public void Potioning_Finish()
    {
        HPPotion.SetActive(false);
        Sword.SetActive(true);
        moveSpeed = 7f;
        animator.SetBool("isPotion", false);

    }
    #endregion
    public void Hurt_Bool()
    {
        isSwing = false;
        isDodgeing = false;
        isDodgeLessing = false;
    }

    //���߻���
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
