using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseButton : MonoBehaviour
{
    [SerializeField] private Image pauseImage;
    [SerializeField] private Image resumeImage;

    bool bIsPaused =false;

    private void Start()
    {
        resumeImage.gameObject.SetActive(false);
        pauseImage.gameObject.SetActive(true);
    }

    public void OnPauseButtonPressed()
    {
        bIsPaused = !bIsPaused;
        pauseImage.gameObject.SetActive(!bIsPaused);
        resumeImage.gameObject.SetActive(bIsPaused);
        Time.timeScale = bIsPaused ? 0 : 1;

    }
}
