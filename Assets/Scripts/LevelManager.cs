using MoreMountains.NiceVibrations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private float gameTimeInSec = 60.0f;
    [SerializeField] private UIManager uIManagerRef;
    [SerializeField] private BotManager botManager;
    [SerializeField] private MainPlayerManager mainPlayerManager;
    [SerializeField] private InteractablesManager interactablesManager;

    private Floor floor;
    private UIManager uIManager;
    private List<Player> playersInGame = new List<Player>();
    private LeaderboardHandler leaderboardHandler;
    private Coroutine finishGameCoroutine;

    private float gameTimer;
    private bool bIsGameRunning;
    private float mainPlayerDieShowUIDelay = 1.5f;

    void Awake()
    {
        leaderboardHandler = new LeaderboardHandler();

        interactablesManager.Initialize();
        InitializeBotManager();
        InitializeMainPlayerManager();
        InitializeUI();
        InitializeFloor();
    }

    private IEnumerator Start()
    {
        RefreshCollectCountDisplay();
        AddPlayerToGame(mainPlayerManager.MainPlayer);

        bIsGameRunning = false;
        gameTimer = gameTimeInSec;

        uIManager.StartGameUIState(true);
        uIManager.InGameUIState(false);
        uIManager.SetSwipeInstructorState(false);

        interactablesManager.StartSpawning();
        playersInGame.ForEach(player => player.IsAlive = false);

        yield return StartCoroutine(uIManager.StartRoutine());
        playersInGame.ForEach(player => player.IsAlive = true);

        yield return new WaitForSeconds(1);
        uIManager.InGameUIState(true);
        bIsGameRunning = true;
        EventsManager.LogLevelStartedEvent(SceneManager.GetActiveScene().buildIndex, null/*, floor.GetMaterialName()*/);

        yield return new WaitForSeconds(1);
        uIManager.StartGameUIState(false);
    }



    private void Update()
    {
        if (!bIsGameRunning && gameTimer < 0)
        {
            gameTimer = 0;
            if (finishGameCoroutine == null)
                finishGameCoroutine = StartCoroutine(TimeEndRoutine());
        }

        bIsGameRunning &= gameTimer > 0;
        if (bIsGameRunning)
        {
            gameTimer -= Time.deltaTime;
        }

        uIManager.SetTimer(gameTimer);
    }

    private void OnDestroy()
    {
        if (botManager)
            TerminateBotManager();
        if (mainPlayerManager && mainPlayerManager.MainPlayer)
            TerminateMainPlayerManager();
    }

    public void Restart()
    {
        EventParams eventParams = new EventParams();
        eventParams.score = mainPlayerManager.GetLevel();
        EventsManager.LogLevelRestartEvent(SceneManager.GetActiveScene().buildIndex, eventParams);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    private void InitializeUI()
    {
        uIManager = FindObjectOfType<UIManager>();
        if (!uIManager)
        {
            uIManager = Instantiate(uIManagerRef);
        }
    }

    private void RefreshCollectCountDisplay()
    {
        uIManager.SetCollectCount(mainPlayerManager.GetCollectCountPercent());
    }

    private void RefreshLeaderboard()
    {
        if (!bIsGameRunning)
            return;

        leaderboardHandler.RefreshLeaderboard(playersInGame);
        uIManager.SetLeaderboard(leaderboardHandler.Leaderboard, mainPlayerManager.MainPlayer);
    }

    private void AddPlayerToGame(Player player)
    {
        playersInGame.Add(player);
        RefreshLeaderboard();
    }

    private void RemovePlayerFromGame(Player player)
    {
        playersInGame.Remove(player);
        RefreshLeaderboard();
    }

    private void MainPlayerDied(Player mainPlayer)
    {
        StartCoroutine(MainPlayerDieRoutine());

        EventParams eventParams = new EventParams();
        eventParams.score = mainPlayerManager.GetLevel();
        EventsManager.LogLevelFailedEvent(SceneManager.GetActiveScene().buildIndex, eventParams);
    }

    private IEnumerator MainPlayerDieRoutine()
    {
        RefreshLeaderboard();
        bIsGameRunning = false;

        yield return new WaitForSeconds(mainPlayerDieShowUIDelay);
        uIManager.StartGameUIState(false);
        uIManager.InGameUIState(false);
        uIManager.EndGameUIState(true, false);
    }

    private void InitializeFloor()
    {
        floor = FindObjectOfType<Floor>();
        //TODO fix it
        if (PlayerPrefs.GetInt("ChangeFloor", 0) == 1)
        {
            PlayerPrefs.SetInt("ChangeFloor", 0);
            floor.ChangeMaterial();
        }
        else
        {
            floor.LoadLastPlayedMaterial();
        }
    }

    private void InitializeMainPlayerManager()
    {
        mainPlayerManager.SpawnPlayer();
        mainPlayerManager.BindOnPlayerDie(MainPlayerDied);
        mainPlayerManager.BindOnPlayerSizeChanged(RefreshLeaderboard);
        mainPlayerManager.BindOnMainPlayerLevelUp(() => uIManager.ShowLevelUp());
        mainPlayerManager.BindOnPlayerSizeChanged(RefreshCollectCountDisplay);

    }

    private void InitializeBotManager()
    {
        botManager.InitializeBotPool();
        botManager.BindOnBotSizeChanged(RefreshLeaderboard);
        botManager.BindOnBotSpawned(bot => AddPlayerToGame(bot));
        botManager.BindOnBotDie(bot => RemovePlayerFromGame(bot));
        botManager.SpawnBots();
    }

    private void TerminateBotManager()
    {
        botManager.UnbindOnBotSizeChanged(RefreshLeaderboard);
        botManager.UnbindOnBotSpawned(bot => AddPlayerToGame(bot));
        botManager.UnbindOnBotDie(bot => RemovePlayerFromGame(bot));
    }
    private void TerminateMainPlayerManager()
    {
        mainPlayerManager.UnbindOnPlayerDie(MainPlayerDied);
        mainPlayerManager.UnbindOnPlayerSizeChanged(RefreshLeaderboard);
        mainPlayerManager.UnbindOnPlayerSizeChanged(RefreshCollectCountDisplay);
        mainPlayerManager.UnbindOnMainPlayerLevelUp(() => uIManager.ShowLevelUp());
    }

    private IEnumerator TimeEndRoutine()
    {
        EventParams eventParams = new EventParams();
        eventParams.score = mainPlayerManager.GetLevel();
        EventsManager.LogLevelCompleteEvent(SceneManager.GetActiveScene().buildIndex, eventParams);

        uIManager.StartGameUIState(false);
        uIManager.InGameUIState(false);
        uIManager.EndGameUIState(true, leaderboardHandler.CurrentLeader == mainPlayerManager.MainPlayer);
        uIManager.SetLeaderboard(leaderboardHandler.Leaderboard, mainPlayerManager.MainPlayer);

        mainPlayerManager.OnTimeEnds(leaderboardHandler.CurrentLeader == mainPlayerManager.MainPlayer);

        yield return null;
    }


}
