using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsCheck : MonoBehaviour
{

    private CapsuleCollider2D coll;

    [Header("检测参数")]
    public Vector2 bottomOffset;
    public Vector2 leftOffset;
    public Vector2 rightOffset;

    public float checkRadius;

    public LayerMask groundLayer;

    [Header("状态")]
    public bool manual;
    public bool isGround;
    public bool touchLeftWall;
    public bool touchRightWall;

    private void Awake()
    {
        coll = GetComponent<CapsuleCollider2D>();

        if (!manual) // 当不是为手动模式时
        {
            // 获取CapsuleCollider2D组件的siez参数，加上偏置Offset后除以2为右边x轴的的碰撞点位置的偏置
            rightOffset = new Vector2((coll.bounds.size.x + coll.offset.x) / 2, coll.bounds.size.y / 2);
            // y轴的碰撞点位置则是直接将size的大小除以2，左边的左边点则是将右边的x轴的偏置取负值
            leftOffset = new Vector2(-rightOffset.x, rightOffset.y);
        }
    }
    private void Update()
    {
        Check();
    }

    public void Check()
    {
        // 检测地面
        isGround = Physics2D.OverlapCircle((Vector2)transform.position, checkRadius, groundLayer);

        // 检测墙体
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
