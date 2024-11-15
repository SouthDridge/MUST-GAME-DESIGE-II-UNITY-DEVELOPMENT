using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    protected Rigidbody2D rb;
    protected Animator anim;
    protected PhysicsCheck physicsCheck;

    [Header("��������")]
    public float normalSpeed;
    public float chaseSpeed;
    public float currentSpeed;
    public float hurtForce;
    public Vector3 faceDir;

    public Transform attacker;

    [Header("��ʱ��")]
    public float waitTime;
    public float waitTimeCounter;

    [Header("״̬")]
    public bool wait;
    public bool isHurt;
    public bool isDead;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        physicsCheck = GetComponent<PhysicsCheck>();

        currentSpeed = normalSpeed;
        waitTimeCounter = waitTime;
    }
    public void Update()
    {
        faceDir = new Vector3(-transform.localScale.x, 0 , 0);
        // Ĭ��ͼƬ����, ������transformǰ�Ӹ���

        if ((physicsCheck.touchLeftWall && faceDir.x < 0)  || 
            (physicsCheck.touchRightWall && faceDir.x > 0))
        // ������Ƿ��нӴ������泯����һ��ʱ��waitֵ��Ϊtrue,
        // ����֮��ִ��TimeCounter����
        {
            wait = true;
            anim.SetBool("walk", false); // �л��������� walk to idle
            //transform.localScale = new Vector3(faceDir.x, 1, 1); 
        }
        TimeCounter();

    }

    public void FixedUpdate()
    {
        if(!isHurt)
        Move();
    }

    public virtual void Move()
    {
        rb.velocity = new Vector2(currentSpeed * faceDir.x * Time.deltaTime, rb.velocity.y);
    }

    public void TimeCounter() // ��ʱ��
    {
        if (wait) // ײǽ���һ��ʱ���ת
        {
            waitTimeCounter -= Time.deltaTime;
            if (waitTimeCounter <= 0) 
            {
                wait = false;
                waitTimeCounter = waitTime;
                transform.localScale = new Vector3(faceDir.x, 1, 1);
            }
        }
    }

    public void OnTakeDamage(Transform attackTrans) 
    { 
        attacker = attackTrans; // attack from player

        // �ܵ��˺����жϹ�������֮��Ұ���ƶ�����ת
        if(attackTrans.position.x - transform.position.x > 0)
            transform.localScale = new Vector3(-1, 1, 1);
        if (attackTrans.position.x - transform.position.x < 0)
            transform.localScale = new Vector3(-1, 1, 1);

        // ���˻���
        isHurt = true;
        anim.SetBool("hurt", true);
        Vector2 dir = new Vector2(transform.position.x - attackTrans.position.x, 0).normalized;

        StartCoroutine(Onhurt(dir));
    }

    private IEnumerator Onhurt(Vector2 dir) 
    {
        rb.AddForce(dir * hurtForce, ForceMode2D.Impulse);
        yield return new WaitForSeconds(0.45f);
        isHurt=false;
    }

    public void Ondie() 
    {
        gameObject.layer = 2;
        anim.SetBool("dead", true);
        isDead = true;
    }

    public void DestroyAfterAnimation()
    {
        Destroy(this.gameObject);
    }

}
