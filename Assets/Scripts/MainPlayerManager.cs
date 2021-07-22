using System;
using UnityEngine;

public class MainPlayerManager : MonoBehaviour
{
    [SerializeField] private Player mainPlayerRef;
    [SerializeField] private NicknameHandler nicknameHandler;

    private Player mainPlayer;

    public Player MainPlayer
    {
        get { return mainPlayer; }
        private set { mainPlayer = value; }
    }

    public void SpawnPlayer()
    {
        foreach (var player in FindObjectsOfType<Player>())
        {
            if (player.IsMainPlayer)
            {
                MainPlayer = player;
                break;
            }
        }

        if (!MainPlayer)
        {
            MainPlayer = Instantiate(mainPlayerRef);
            MainPlayer.IsMainPlayer = true;
        }

        MainPlayer.ResetPlayer();
        MainPlayer.Activate();

        MainPlayer.PlayerName = "player";
        MainPlayer.FlagIcon = nicknameHandler.GetMainPlayerFlagIcon();

    }

    public void OnTimeEnds(bool isWinner)
    {
        MainPlayer.IsAlive = false;
        MainPlayer.SetMovementDirection(Vector3.zero);

        if (isWinner)
            MainPlayer.Win();
        else
            MainPlayer.Lose();
    }
    public float GetLevel()
    {
        return MainPlayer ? MainPlayer.Level : 0;
    }

    public float GetCollectCountPercent()
    {
        return MainPlayer.CollectCountPercent;
    }

    public void BindOnPlayerSizeChanged(Action action)
    {
        MainPlayer.OnSizeChanged += action;
    }
    public void UnbindOnPlayerSizeChanged(Action action)
    {
        MainPlayer.OnSizeChanged -= action;
    }
    public void BindOnPlayerDie(Action<Player> action)
    {
        MainPlayer.OnPlayerDie += action;
    }
    public void UnbindOnPlayerDie(Action<Player> action)
    {
        MainPlayer.OnPlayerDie -= action;
    }
    public void BindOnMainPlayerLevelUp(Action action)
    {
        MainPlayer.OnPlayerLevelUp += action;
    }
    public void UnbindOnMainPlayerLevelUp(Action action)
    {
        MainPlayer.OnPlayerLevelUp -= action;
    }
}