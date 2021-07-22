using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttribute : MonoBehaviour
{
    [SerializeField] private float initialVal;
    [SerializeField] private float minVal;
    [SerializeField] private float maxVal;
    [SerializeField] private int numberOfLevel=1;

    private float incrementAmount;

    private float currentVal;
    public float InitialVal
    {
        get { return initialVal; }
        set { initialVal = value; }
    }

    public float CurrentVal
    {
        get { return currentVal; }
        set
        {
            currentVal = value;
            currentVal = Mathf.Clamp(currentVal, minVal, maxVal);
        }
    }

    public float MaxVal
    {
        get { return maxVal; }
        set { maxVal = value; }
    }

    public float MinVal
    {
        get { return minVal; }
        set { minVal = value; }
    }

    public float IncrementAmount
    {
        get { return incrementAmount; }
        set { incrementAmount = value; }
    }

    private void Awake()
    {
        ResetAttribute();
    }

    public void ResetAttribute()
    {
        CurrentVal = InitialVal;
        incrementAmount = (maxVal - minVal) / numberOfLevel;

    }

}

