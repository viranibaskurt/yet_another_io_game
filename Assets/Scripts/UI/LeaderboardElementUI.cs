using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class LeaderboardElementUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI playerNameText;
    [SerializeField] private TextMeshProUGUI playerIndexText;


    [SerializeField] private Color normalTextColor;
    [SerializeField] private Color mainPlayerTextColor;

    public void SetElement(string playerName, int i,bool bIsMainPlayer)
    {
        playerNameText.text = playerName;
        playerIndexText.text = (i+1).ToString();
        if(bIsMainPlayer)
        {
            SetElementForMainPlayer();
        }
        else
        {
            SetElementForNormalPlayer();
        }
    }

    public void SetElementForNormalPlayer()
    {
        playerNameText.color = normalTextColor;
    }
    public void SetElementForMainPlayer()
    {
        playerNameText.color = mainPlayerTextColor;
    }

}

