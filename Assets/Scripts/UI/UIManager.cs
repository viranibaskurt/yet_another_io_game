using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private StartGameUI startGameUI;
    [SerializeField] private InGameUI inGameUI;
    [SerializeField] private EndGameUI endGameUI;

    private void Awake()
    {
        StartGameUIState(true);
        InGameUIState(false);
        EndGameUIState(false);
    }

    public void EndGameUIState(bool state, bool bSuccess = false)
    {
        endGameUI.gameObject.SetActive(state);

        if (!state)
            return;

        if (bSuccess)
        {
            endGameUI.ShowWinPanel();
        }
        else
        {
            endGameUI.ShowFailPanel();
        }
    }
    public void InGameUIState(bool state)
    {
        inGameUI.gameObject.SetActive(state);

    }
    public void StartGameUIState(bool state)
    {
        startGameUI.gameObject.SetActive(state);

    }
    public void SetLeaderboard(in List<Player> players, in Player mainPlayer)
    {
        endGameUI.SetLeaderboard(players, mainPlayer);
        inGameUI.SetLeaderboard(players, mainPlayer);
    }

    public void SetTimer(float gameTimer)
    {
        inGameUI.SetTimer(gameTimer);
    }

    public void SetCountDownText(string v)
    {
        startGameUI.SetCountDownText(v);
    }

    public void ShowLevelUp()
    {
        inGameUI.ShowLevelUp();
    }

    public void SetCollectCount(float count)
    {
        inGameUI.SetCollectCount(count);
    }

    public void SetSwipeInstructorState(bool state)
    {
        startGameUI.SetSwipeInstructorState(state);
    }
    public IEnumerator StartRoutine()
    {
        yield return startGameUI.CountDownRoutine();
    }
}
