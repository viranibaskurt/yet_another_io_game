using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SizeUpImgUI : MonoBehaviour
{
    public void Show()
    {
        GetComponent<Animator>().SetTrigger("Show");    
    }

}
