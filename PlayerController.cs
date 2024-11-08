using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public PlayerInputControl inputControl;
    public Vector2 inputDirection;

    [Header("��������")]
    public float speed;
    public float jumpForce;
    private float runSpeed;
    public float hurtForce;
    private float walkSpeed => runSpeed / 2.5f;

    // public int combo;

    [Header("״̬")]
    public bool isHurt;
    public bool isDead;
    public bool isCrouch;
    public bool isAttack;

    [Header("�������")]
    public PhysicsMaterial2D normal;
    public PhysicsMaterial2D wall;


    // Lambda���ʽ��(�����б�) => ���ʽ�壻
    // �൱���ں���ʹ���е� walkSpeed = runSpeed / 2.5
    // �����б� ������һ�������������������Ϳ�����ȷָ�����ɱ������ƶϡ�
    // ���ʽ�壺 ������һ�����ʽ��һ�����顣

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
        if (!isCrouch) // �¶�ʱ���ƶ�
            rb.velocity = new Vector2(inputDirection.x * speed * Time.deltaTime, rb.velocity.y);

        int faceDir = (int)transform.localScale.x;

        if (inputDirection.x > 0)
        {
            faceDir = 1;
        }

        if (inputDirection.x < 0)
            faceDir = -1;

        // ���﷭ת
        transform.localScale = new Vector3(faceDir, 1, 1);

        // �����¶�
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
        rb.velocity = Vector2.zero; // �����������
        Vector2 dir = new Vector2((transform.position.x - attacker.position.x),0).normalized;
        // ��position.x�����������ķ��򣬲���normalized�����С����Ϊ1
        rb.AddForce(dir * hurtForce, ForceMode2D.Impulse); // ���������������
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
            // �޸���ײ�����ƫ��
            coll.size = new Vector2(-0.05f, 0.85f);
            coll.offset = new Vector2(0.7f, 1.7f);
        }
        else
        {
            // ��ԭ����
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
        // �޸�Player��ײ��Ĳ��ʣ�ͨ��isGround�ж�����״̬
        // ����ڵ�����Ϊ�������ʣ���֮��Ϊ�⻬����
    }

}
