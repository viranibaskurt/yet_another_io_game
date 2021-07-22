using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RestartButton : MonoBehaviour
{

    public void RestartLevel()
    {
        GetComponent<Button>().enabled = false;
        Time.timeScale = 1;

        //Animator anim = GetComponent<Animator>();
        //anim.SetBool("isRestartClicked", true);


        LevelManager levelManager= FindObjectOfType<LevelManager>() ;
        if (levelManager)
            levelManager.Restart();

    }
}
