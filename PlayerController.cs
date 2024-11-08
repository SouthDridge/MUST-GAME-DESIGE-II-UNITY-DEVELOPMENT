using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public PlayerInputControl inputControl;
    public Vector2 inputDirection;

    [Header("基础属性")]
    public float speed;
    public float jumpForce;
    private float runSpeed;
    public float hurtForce;
    private float walkSpeed => runSpeed / 2.5f;

    // public int combo;

    [Header("状态")]
    public bool isHurt;
    public bool isDead;
    public bool isCrouch;
    public bool isAttack;

    [Header("物理材质")]
    public PhysicsMaterial2D normal;
    public PhysicsMaterial2D wall;


    // Lambda表达式：(参数列表) => 表达式体；
    // 相当于在后续使用中的 walkSpeed = runSpeed / 2.5
    // 参数列表： 可以是一个或多个参数，参数类型可以明确指定或由编译器推断。
    // 表达式体： 可以是一个表达式或一个语句块。

    private Rigidbody2D rb;
    private PhysicsCheck physicsCheck;
    private CapsuleCollider2D coll;
    private PlayerAnimation playerAnimation;

    private Vector2 originalSize;
    private Vector2 originalOffset;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        physicsCheck = GetComponent<PhysicsCheck>();
        coll = GetComponent<CapsuleCollider2D>();
        playerAnimation = GetComponent<PlayerAnimation>();

        originalSize = coll.size;
        originalOffset = coll.offset;

        inputControl = new PlayerInputControl();
        inputControl.Gameplay.Jump.started += Jump;

        #region walk
        runSpeed = speed;
        inputControl.Gameplay.WalkButton.performed += ctx =>
        {
            if (physicsCheck.isGround)
                speed = walkSpeed;
        };
        inputControl.Gameplay.WalkButton.canceled += ctx =>
        {
            if (physicsCheck.isGround)
                speed = runSpeed;
        };
        #endregion

        // attack
        inputControl.Gameplay.Attack.started += PlayerAttack;
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
        inputDirection = inputControl.Gameplay.Move.ReadValue<Vector2>();
        CheckState();
    }

    private void FixedUpdate()
    {
        if(!isHurt && !isAttack)
            Move();
            // ChangeColl();
    }

    public void Move()
    {
        if (!isCrouch) // 下蹲时不移动
            rb.velocity = new Vector2(inputDirection.x * speed * Time.deltaTime, rb.velocity.y);

        int faceDir = (int)transform.localScale.x;

        if (inputDirection.x > 0)
        {
            faceDir = 1;
        }

        if (inputDirection.x < 0)
            faceDir = -1;

        // 人物翻转
        transform.localScale = new Vector3(faceDir, 1, 1);

        // 人物下蹲
        isCrouch = inputDirection.y < -0.5f && physicsCheck.isGround;
    }

    public void Jump(InputAction.CallbackContext obj)
    {
        // Debug.Log("jump");
        // 
        if (physicsCheck.isGround)
        {
            rb.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
        }          
    }

    #region UnityEvent
    public void GetHurt(Transform attacker) 
    { 
        isHurt = true;
        rb.velocity = Vector2.zero; // 消除人物惯性
        Vector2 dir = new Vector2((transform.position.x - attacker.position.x),0).normalized;
        // 将position.x相减获得推力的方向，并用normalized将其大小设置为1
        rb.AddForce(dir * hurtForce, ForceMode2D.Impulse); // 添加推力反弹人物
    }
    public void PlayerDead() 
    {
        isDead = true;
        inputControl.Gameplay.Disable();
    }
    #endregion

    public void ChangeColl()
    {
        if (isCrouch)
        {
            // 修改碰撞体积和偏置
            coll.size = new Vector2(-0.05f, 0.85f);
            coll.offset = new Vector2(0.7f, 1.7f);
        }
        else
        {
            // 还原参数
            coll.size = originalSize;
            coll.offset = originalOffset;
        }
    }

    private void PlayerAttack(InputAction.CallbackContext obj) 
    {
        playerAnimation.PlayerAttack();
        isAttack = true;
        //combo++;
        //if(combo >= 3)
        //    combo = 0;
    }
    private void CheckState() 
    {
        coll.sharedMaterial = physicsCheck.isGround ? normal : wall;
        // 修改Player碰撞体的材质，通过isGround判定人物状态
        // 如果在地面则为正常材质，反之则为光滑材质
    }

}
