using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InGameUI : MonoBehaviour
{
    [SerializeField] private LeaderboardUI leaderboardUI;
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private Slider collectCountSlider;
    [SerializeField] private SizeUpImgUI sizeUpImg;

    public void SetTimer(float gameTimer)
    {
        TimeSpan t = TimeSpan.FromSeconds(gameTimer);
        string minStr = (t.Minutes < 10) ? $"0{t.Minutes}" : $"{t.Minutes}";
        string secStr = (t.Seconds < 10) ? $"0{t.Seconds}" : $"{t.Seconds}";

        timerText.text = $"{minStr}:{secStr}";
    }

    public void SetLeaderboard(List<Player> players, Player mainPlayer)
    {
        leaderboardUI.SetLeaderboard(players, mainPlayer);
    }

    public void SetCollectCount(float count)
    {
        collectCountSlider.value = count;
    }

    public void ShowLevelUp()
    {
        sizeUpImg.Show();
    }
}
