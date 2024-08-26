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

        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        float moveright = Mathf.Abs(moveVertical * moveHorizontal);
        // 이동 방향 설정
        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);

        // 이동 처리
        rb.MovePosition(transform.position + movement * moveSpeed * Time.deltaTime);

        // 플레이어 회전 처리
        if (movement != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(movement, Vector3.up);
            rb.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
        }
        animator.SetFloat("move right", moveright);
    }
}

