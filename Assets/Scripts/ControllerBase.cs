using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ControllerBase : MonoBehaviour
{
    [SerializeField] protected Player controlledPlayer;
    [SerializeField] protected float moveSpeed = 1.0f;
    private bool controllerStatus;
    public virtual bool ControllerStatus
    {
        get { return controllerStatus; }
        set { controllerStatus = value; }
    }

    public virtual void ResetController()
    {

    }

    public virtual float MoveSpeed
    {
        get { return moveSpeed; }
        set { moveSpeed = value; }
    }
}

