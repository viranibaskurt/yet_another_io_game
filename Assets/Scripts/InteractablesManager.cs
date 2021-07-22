using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractablesManager : MonoBehaviour
{
    [SerializeField] private List<InteractableBase> interactablesRef = new List<InteractableBase>();
    [SerializeField] private int interactablePoolSize = 20;
    [SerializeField] private int initialNumberOfPickUp = 5;
    [SerializeField] private float maxTimeBetweenSpawnsInSec = 6.0f;
    [SerializeField] private float minTimeBetweenSpawnsInSec = 3.0f;

    private Queue<InteractableBase> interactablePool = new Queue<InteractableBase>();
    private Coroutine spawnCoroutine;

    public void Initialize()
    {
        for (int i = 0; i < interactablePoolSize; i++)
        {
            foreach (InteractableBase item in interactablesRef)
            {
                InteractableBase temp = Instantiate(item, transform);
                interactablePool.Enqueue(temp);
                temp.Deactivate();
            }
        }
    }

    public void StartSpawning()
    {
        for (int i = 0; i < initialNumberOfPickUp; i++)
            SpawnPickUpItem();

        spawnCoroutine = StartCoroutine(SpawnRoutine());
    }

    public void StopSpawning()
    {
        StopCoroutine(spawnCoroutine);
        spawnCoroutine = null;
    }

    private void SpawnPickUpItem()
    {
        if (interactablePool.Count == 0)
            return;

        InteractableBase temp = interactablePool.Dequeue();
        if (!temp)
            return;

        temp.SetPosition(Utils.GetRandomPosition());
        temp.Activate();
        interactablePool.Enqueue(temp);
    }
    private IEnumerator SpawnRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(minTimeBetweenSpawnsInSec, maxTimeBetweenSpawnsInSec));
            for (int i = 0; i < initialNumberOfPickUp; i++)
            {
                SpawnPickUpItem();
            }
        }
    }
}
