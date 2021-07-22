using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Character_Canvas_Follow : MonoBehaviour
{
    public Canvas PlayerCanvas;
    public Transform CharacterCanvasPosition;

    void Update()
    {
        Vector3 canvasPos = CharacterCanvasPosition.transform.position;
        PlayerCanvas.transform.position = canvasPos;
    }
}
