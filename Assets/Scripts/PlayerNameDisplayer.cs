using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerNameDisplayer : MonoBehaviour
{
    private Transform mainCameraTransform;
    [SerializeField] private Player followingPlayer;
    [SerializeField] private Transform playerTopPt;
    [SerializeField] private TextMeshProUGUI playerNameText;
    [SerializeField] private Image icon;

    Vector3 offset;

    // Start is called before the first frame update
    void Start()
    {
        offset = followingPlayer.GetCharacterPosition() - transform.position;
        mainCameraTransform = Camera.main.transform;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = followingPlayer.GetCharacterPosition() /*- offset*/;
        pos.y = playerTopPt.position. y + 1.5f;
        transform.position = pos;
        //transform.LookAt(mainCameraTransform, Vector3.down);

    }

    public void SetPlayerName(string name)
    {
        playerNameText.text = name;
    }
    public void SetIcon(Sprite sprite)
    {
        icon.gameObject.SetActive(sprite != null);
        icon.sprite = sprite;
    }

}
