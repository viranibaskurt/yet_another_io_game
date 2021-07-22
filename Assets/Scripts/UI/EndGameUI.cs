using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGameUI : MonoBehaviour
{
    [SerializeField] private GameObject winPanel;
    [SerializeField] private GameObject failPanel;
    [SerializeField] private LeaderboardUI leaderboardUI;
    [SerializeField] private float showLeaderboardDelay=3.0f;

    private Coroutine ShowLeaderboardCoroutine;

    public void SetLeaderboard(in List<Player> players, in Player mainPlayer)
    {
        leaderboardUI.SetLeaderboard(players,mainPlayer);
    }

    public void ShowWinPanel()
    {
        leaderboardUI.gameObject.SetActive(false);
        failPanel.SetActive(false);
        winPanel.SetActive(true);
        if (ShowLeaderboardCoroutine == null)
            ShowLeaderboardCoroutine = StartCoroutine( ShowLeaderboardRoutine());
    }

    public void ShowFailPanel()
    {
        leaderboardUI.gameObject.SetActive(false);

        failPanel.SetActive(true);
        winPanel.SetActive(false);
        if (ShowLeaderboardCoroutine == null)
            ShowLeaderboardCoroutine = StartCoroutine(ShowLeaderboardRoutine());
    }

    private IEnumerator ShowLeaderboardRoutine()
    {
        yield return new WaitForSeconds(showLeaderboardDelay);
        leaderboardUI.gameObject.SetActive(true);
    }

}
