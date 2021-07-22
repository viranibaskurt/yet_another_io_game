using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public abstract class InteractableBase : MonoBehaviour
{
    public event Action<InteractableBase> OnDeactivated;
    [SerializeField] private int maxLifeTime = 15;
    [SerializeField] private int minLifeTime = 10;


    private float deactivationTime = 15.0f;
    private Coroutine deactivationCorotuine;

    public virtual void Interact(IInteractionist interactionist)
    {
        Deactivate();
    }

    public void SetPosition(Vector3 position)
    {
        transform.position = position;
    }

    public virtual void Activate()
    {
        gameObject.SetActive(true);
        if (deactivationCorotuine != null)
        {
            StopCoroutine(deactivationCorotuine);
        }
        deactivationTime = Random.Range(minLifeTime, maxLifeTime);
        deactivationCorotuine = StartCoroutine(DeactivationRoutine());

    }

    public virtual void Deactivate()
    {
        if (deactivationCorotuine != null)
        {
            StopCoroutine(deactivationCorotuine);
            deactivationCorotuine = null;
        }
        OnDeactivated?.Invoke(this);
        gameObject.SetActive(false);
    }

    private IEnumerator DeactivationRoutine()
    {
        yield return new WaitForSeconds(deactivationTime);
        Deactivate();
    }
}

public interface IInteractionist
{
    void InteractWithBloater(int change);
    void InteractWithSpeedUp(float time);
}
