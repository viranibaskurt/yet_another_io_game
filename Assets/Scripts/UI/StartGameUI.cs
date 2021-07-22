using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StartGameUI : MonoBehaviour
{
    [SerializeField] private GameObject ready;
    [SerializeField] private GameObject set;
    [SerializeField] private GameObject go;
    [SerializeField] private GameObject swipeInstructor;
    [SerializeField] private GameObject hand;
    [SerializeField] private float timeBetweenCountdowns=0.7f;

    public void SetCountDownText(string v)
    {
        ready.SetActive(v == "ready");
        set.SetActive(v == "set");
        go.SetActive(v == "go");
    }

    public void SetSwipeInstructorState(bool state)
    {
        swipeInstructor.SetActive(state);
        hand.SetActive(state);
    }

    public IEnumerator CountDownRoutine()
    {
        yield return new WaitForSeconds(timeBetweenCountdowns);
        SetCountDownText("ready");

        yield return new WaitForSeconds(timeBetweenCountdowns);
        SetCountDownText("set");

        yield return new WaitForSeconds(timeBetweenCountdowns);
        SetCountDownText("go");
        SetSwipeInstructorState(true);
    }
}
