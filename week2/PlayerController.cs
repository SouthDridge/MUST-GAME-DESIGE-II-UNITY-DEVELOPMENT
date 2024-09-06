using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

// ����ʱû�иĺ����־���class����ֶθ�ΪPlayerController
public class PlayerController : MonoBehaviour
{
    public PlayerInputControl inputControl;
    public Vector2 inputDirection;

    [Header("basic parameter")]
    public float speed;
    public float jumpForce;

    private Rigidbody2D rb;
    private PhysicsCheck physicsCheck;

    // public Rigidbody2D rb;
    // public SpriteRenderer sr;

    private void Awake()
    {
        inputControl = new PlayerInputControl();

        rb = GetComponent<Rigidbody2D>();
        physicsCheck = GetComponent<PhysicsCheck>();
        // sr = GetComponent<SpriteRenderer>();

        
        // ��ȡ��Ծ����
        inputControl.Gameplay.Jump.started += Jump;
        
    }

    private void OnEnable()
    {
        inputControl.Enable();
    }

    private void OnDisable()
    {
        inputControl.Disable();
    }

    private void Update()
    {
        // ������ȡ����
        inputDirection = inputControl.Gameplay.Move.ReadValue<Vector2>();  
    }

    private void FixedUpdate() // �Թ̶�֡�ʽ��и��£���ʽ�豸֡�ʲ�ͬ�����Ľ����һ��
    {
        Move(); // ����Move����

        // Filp();
    }

    public void Move()  
    {
        // ����ƶ�
        rb.velocity = new Vector2 (inputDirection.x * speed * Time.deltaTime, rb.velocity.y);
        // x �᣺�������� * �ٶ� * Time.deltaTime�� y �᣺���ָ����ԭʼ�ٶ� -9.81��������

        int faceDir = (int)transform.localScale.x;
        // transform.localScale.x ��һ�����������ڽ��ղ�ͬ�����豸������
        // ��ǰ�����(int) �Ѹ�����ǿ��ת��Ϊ�������ں���ʹ��
        
        // �ж�����ķ��� ����0,faceDir = 1(��)��С��0, faceDir = -1(��)
        if (inputDirection.x > 0)
            faceDir = 1;
        if (inputDirection.x < 0)
            faceDir = -1;

        // ͨ������scaleʵ�� ���﷭ת
        transform.localScale = new Vector3(faceDir, 1, 1);
    }
    private void Jump(InputAction.CallbackContext obj)
    {
        // Debug.Log("jump");
        if (physicsCheck.isGround)
        {
            rb.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
        }
    }


    //public void Filp() 
    //{
    //    if (inputDirection.x > 0)
    //        sr.flipX = false;
    //    if (inputDirection.x < 0)
    //        sr.flipX = true;
    //}
}
