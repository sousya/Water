using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckGroundTool : MonoBehaviour
{
    public Transform groundCheck;
    public LayerMask patrolLayer;
    public Rigidbody m_rigidBody;

    public virtual bool CheckGround()
    {
        return Physics.Raycast(groundCheck.position, -transform.up, 0.4f, patrolLayer);
    }

    public virtual void FixedUpdate()
    {
        CheckDown();
        if (CheckGround())
        {
            m_rigidBody.velocity = Vector3.zero;
        }
    }

    public virtual void CheckDown()
    {
        if (CheckGround())
        {
                m_rigidBody.useGravity = false;
        }
        else
        {
                m_rigidBody.useGravity = true;
        }
    }

    public virtual void OnDeath()
    {

    }
}