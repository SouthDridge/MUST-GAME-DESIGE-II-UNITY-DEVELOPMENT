using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsCheck : MonoBehaviour
{

    [Header("check parameter")]
    public Vector2 bottomOffset;
    
    public float checkRaduis;

    public LayerMask groundLayer;

    [Header("status")]
    public bool isGround;

    private void Update()
    {
        Check();
    }

    public void Check() 
    {
        // ºÏ≤‚µÿ√Ê
        isGround = Physics2D.OverlapCircle((Vector2)transform.position + bottomOffset, checkRaduis, groundLayer);
        //isGround = Physics2D.OverlapCircle(transform.position, checkRaduis, groundLayer);
    }

    private void OnDrawGizmosSelected()
    {
        // Gizmos.color = Color.red;
        Gizmos.DrawWireSphere((Vector2)transform.position + bottomOffset, checkRaduis);
    }

}


