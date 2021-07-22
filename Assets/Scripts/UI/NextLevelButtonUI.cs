using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NextLevelButtonUI : MonoBehaviour
{
    Button button;

    private void Start()
    {
        button = GetComponent<Button>();
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(OnNextButtonClicked);
    }

    public void OnNextButtonClicked()
    {
        button.interactable = false;
        //int currentScene= SceneManager.GetActiveScene().buildIndex;
        //int numberOfScene = SceneManager.sceneCountInBuildSettings;
        //int luckyNum = currentScene;
        //for (int i = 0; i < 100; i++)
        //{
        //    luckyNum = Random.Range(0, numberOfScene);
        //    if (luckyNum != currentScene)
        //    {
        //        break;
        //    }
        //}

        PlayerPrefs.SetInt("ChangeFloor", 1);
        PlayerPrefs.Save();

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

    }
}
