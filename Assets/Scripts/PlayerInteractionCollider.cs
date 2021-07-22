using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class PlayerInteractionCollider : MonoBehaviour
{
    public event Action<Player> OnCollision;
    [SerializeField] private Player player;
    private Collider col;
    private Rigidbody rb;

    public Player Player { get { return player; } }

    private void Awake()
    {
        col = GetComponent<Collider>();
        rb = GetComponent<Rigidbody>();
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerInteractionCollider otherRange = other.GetComponent<PlayerInteractionCollider>();
        if (otherRange)
        {
            OnCollision?.Invoke(otherRange.player);
            return;
        }

        InteractableBase otherInteractable = other.GetComponent<InteractableBase>();
        if (otherInteractable != null)
        {
            otherInteractable.Interact(player);
        }
    }

    public void SetEnabled(bool state)
    {
        col.enabled=state;
    }
}
