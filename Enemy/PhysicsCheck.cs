using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsCheck : MonoBehaviour
{

    private CapsuleCollider2D coll;

    [Header("������")]
    public Vector2 bottomOffset;
    public Vector2 leftOffset;
    public Vector2 rightOffset;

    public float checkRadius;

    public LayerMask groundLayer;

    [Header("״̬")]
    public bool manual;
    public bool isGround;
    public bool touchLeftWall;
    public bool touchRightWall;

    private void Awake()
    {
        coll = GetComponent<CapsuleCollider2D>();

        if (!manual) // ������Ϊ�ֶ�ģʽʱ
        {
            // ��ȡCapsuleCollider2D�����siez����������ƫ��Offset�����2Ϊ�ұ�x��ĵ���ײ��λ�õ�ƫ��
            rightOffset = new Vector2((coll.bounds.size.x + coll.offset.x) / 2, coll.bounds.size.y / 2);
            // y�����ײ��λ������ֱ�ӽ�size�Ĵ�С����2����ߵ���ߵ����ǽ��ұߵ�x���ƫ��ȡ��ֵ
            leftOffset = new Vector2(-rightOffset.x, rightOffset.y);
        }
    }
    private void Update()
    {
        Check();
    }

    public void Check()
    {
        // ������
        isGround = Physics2D.OverlapCircle((Vector2)transform.position, checkRadius, groundLayer);

        // ���ǽ��
        touchLeftWall = Physics2D.OverlapCircle((Vector2)transform.position + leftOffset , checkRadius, groundLayer);
        touchRightWall = Physics2D.OverlapCircle((Vector2)transform.position + rightOffset , checkRadius, groundLayer);
    }

    private void OnDrawGizmosSelected()
    {
        // Gizmos.color = Color.red;
        Gizmos.DrawWireSphere((Vector2)transform.position + bottomOffset, checkRadius);
        Gizmos.DrawWireSphere((Vector2)transform.position + rightOffset, checkRadius);
        Gizmos.DrawWireSphere((Vector2)transform.position + leftOffset, checkRadius);
    }
}
