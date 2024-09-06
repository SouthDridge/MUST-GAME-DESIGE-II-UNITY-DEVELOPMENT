using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

// 创建时没有改好名字就在class后的字段改为PlayerController
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

        
        // 读取跳跃按键
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
        // 持续读取输入
        inputDirection = inputControl.Gameplay.Move.ReadValue<Vector2>();  
    }

    private void FixedUpdate() // 以固定帧率进行更新，方式设备帧率不同产生的结果不一致
    {
        Move(); // 调用Move函数

        // Filp();
    }

    public void Move()  
    {
        // 玩家移动
        rb.velocity = new Vector2 (inputDirection.x * speed * Time.deltaTime, rb.velocity.y);
        // x 轴：按键方向 * 速度 * Time.deltaTime； y 轴：保持刚体的原始速度 -9.81（重力）

        int faceDir = (int)transform.localScale.x;
        // transform.localScale.x 是一个浮点数用于接收不同输入设备的输入
        // 在前面加上(int) 把浮点数强制转化为整数便于后续使用
        
        // 判断输入的方向 大于0,faceDir = 1(右)；小于0, faceDir = -1(左)
        if (inputDirection.x > 0)
            faceDir = 1;
        if (inputDirection.x < 0)
            faceDir = -1;

        // 通过调整scale实现 人物翻转
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
