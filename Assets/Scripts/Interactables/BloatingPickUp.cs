using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloatingPickUp : InteractableBase
{
    [SerializeField] private GameObject pickUpParticle;
    [SerializeField] private GameObject pickUpMesh;
    [SerializeField] private Collider interactionCollider;
    [SerializeField] private float particlePlayTime = 1.5f;

    private Coroutine InteractAfterParticlePlayCoroutine;
    public override void Activate()
    {
        base.Activate();
        pickUpParticle.SetActive(false);
        pickUpMesh.SetActive(true);
        interactionCollider.enabled = true;
        if (InteractAfterParticlePlayCoroutine != null)
        {
            StopCoroutine(InteractAfterParticlePlayCoroutine);
            InteractAfterParticlePlayCoroutine = null;
        }
    }

    public override void Interact(IInteractionist interactionist)
    {
        pickUpParticle.SetActive(true);
        interactionist.InteractWithBloater(1);
        pickUpParticle.SetActive(true);
        pickUpMesh.SetActive(false);
        interactionCollider.enabled = false;
        InteractAfterParticlePlayCoroutine = StartCoroutine(InteractAfterParticlePlayRoutine());
    }

    private IEnumerator InteractAfterParticlePlayRoutine()
    {
        yield return new WaitForSeconds(particlePlayTime);
        Deactivate();
        InteractAfterParticlePlayCoroutine = null;
    }


}