using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fox_Move : MonoBehaviour
{
    [SerializeField] public float moveSpeed = 5f;
    [SerializeField] public float rotationSpeed = 700f;
    
    

    private Rigidbody rb;
    private Animator animator;
    private void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
      
    }

    private void Update()
    {
        
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");


        //���� ����
        if (Input.GetKey(KeyCode.LeftShift))
        {
            animator.SetBool("isFocus", true);
        }
        else
        { 
            animator.SetBool("isFocus", false);
        }

        // �̵� ���� ����
        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);

        // �̵� ó��
        rb.MovePosition(transform.position + movement * moveSpeed * Time.deltaTime);

        // �÷��̾� ȸ�� ó��
        if (movement != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(movement, Vector3.up);
            rb.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
        }
        animator.SetFloat("move_right", moveHorizontal);
        animator.SetFloat("move_forward", moveVertical);


        //������
        if (Input.GetKey(KeyCode.Space))
        {
            animator.SetTrigger("dodge");
        }
    }

}

