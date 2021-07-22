using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : ControllerBase
{

    [SerializeField] private Joystick joystick;
    private bool bFirstInputGiven = false;


    public override bool ControllerStatus
    {
        get => base.ControllerStatus;
        set
        {
            base.ControllerStatus = value;
            joystick.gameObject.SetActive(value);
        }
    }

    void Update()
    {

#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            controlledPlayer.InteractWithBloater(1);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            controlledPlayer.InteractWithBloater(-1);
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            controlledPlayer.Die(Vector3.zero);
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            controlledPlayer.ResetPlayer();
        }

#endif
        if (!ControllerStatus)
            return;

        Vector3 moveVector = new Vector3(joystick.Horizontal, 0.0f, joystick.Vertical);
        Move(moveVector.normalized);

    }

    public override void ResetController()
    {
        base.ResetController();
        bFirstInputGiven = false;
    }

    private void Move(Vector3 moveVector)
    {
        if (!bFirstInputGiven && moveVector.sqrMagnitude > 0)
        {
            bFirstInputGiven = true;
            controlledPlayer.FirstInputGiven();
        }

        controlledPlayer.SetMovementDirection(moveVector);

        controlledPlayer.Translate(moveVector * Time.deltaTime * MoveSpeed);
    }
}
