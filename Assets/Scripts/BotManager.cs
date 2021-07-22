using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotManager : MonoBehaviour
{
    [SerializeField] private int numberOfBotInGame = 10;
    [SerializeField] private float botRespawnDelay = 2.5f;
    [SerializeField] private Player botPlayerRef;
    [SerializeField] private NicknameHandler nicknameHandler;

    private List<Player> bots = new List<Player>();
    private Action<Player> OnBotSpawned;
    private Action<Player> OnBotDied;

    public void InitializeBotPool()
    {
        for (int i = 0; i < numberOfBotInGame; i++)
        {
            bots.Add(Instantiate(botPlayerRef,transform));
            bots[i].gameObject.name = "bot" + i.ToString();
            bots[i].Deactivate();
            bots[i].OnPlayerDie += BotManager_OnBotPlayerDie;
            bots[i].IsMainPlayer = false;
        }
    }
    public void SpawnBots()
    {
        for (int i = 0; i < numberOfBotInGame; i++)
            SpawnBot(bots[i]);
    }

    public void BindOnBotSizeChanged(Action action)
    {
        bots.ForEach(bot => bot.OnSizeChanged += action);
    }
    public void UnbindOnBotSizeChanged(Action action)
    {
        bots.ForEach(bot => bot.OnSizeChanged -= action);
    }
    public void BindOnBotSpawned(Action<Player> action)
    {
        OnBotSpawned += action;
    }
    public void UnbindOnBotSpawned(Action<Player> action)
    {
        OnBotSpawned -= action;
    }
    public void BindOnBotDie(Action<Player> action)
    {
        OnBotDied += action;
    }
    public void UnbindOnBotDie(Action<Player> action)
    {
        OnBotDied -= action;
    }

    private void SpawnBot(Player bot)
    {
        bot.ResetPlayer();

        Vector3 pos = Vector3.zero;
        for (int i = 0; i < 100; i++)
        {
            pos = Utils.GetRandomPosition();
            float sqrMag = Vector3.SqrMagnitude(pos);
            if (Vector3.SqrMagnitude(pos) > 20.0f)
            {
                break;
            }
        }

        bot.transform.position = new Vector3(pos.x, bot.transform.position.y, pos.z);
        bot.PlayerName = nicknameHandler.GetNextNickName();
        bot.FlagIcon = nicknameHandler.GetRandomFlagIcon();
        bot.Activate();
        bot.InteractWithBloater(UnityEngine.Random.Range(0, 2));
        bot.IsAlive = true;

        OnBotSpawned?.Invoke(bot);
    }

    private void BotManager_OnBotPlayerDie(Player bot)
    {
        OnBotDied?.Invoke(bot);
        StartCoroutine(SpawnBotAfterDieRoutine(bot));
    }

    private IEnumerator SpawnBotAfterDieRoutine(Player bot)
    {
        yield return new WaitForSeconds(botRespawnDelay);
        SpawnBot(bot);
    }

}
