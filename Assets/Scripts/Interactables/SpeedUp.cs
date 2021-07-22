using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedUp : InteractableBase
{
    [SerializeField] private float speedUpTime = 5.0f;

    public override void Interact(IInteractionist interactionist)
    {
        interactionist.InteractWithSpeedUp(speedUpTime);
        base.Interact(interactionist);
    }

}
