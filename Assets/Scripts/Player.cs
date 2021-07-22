using MoreMountains.NiceVibrations;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class Player : MonoBehaviour, IInteractionist
{
    public event Action<Player> OnPlayerDie;
    public event Action OnSizeChanged;
    public event Action OnFirstInputGiven;
    public event Action OnPlayerLevelUp;

    [SerializeField] private PlayerInteractionCollider interactionCollider;
    [SerializeField] private Character character;
    [SerializeField] private ControllerBase controller;
    [SerializeField] private PlayerNameDisplayer playerNameDisplayer;
    [SerializeField] private GameObject crownGO;

    [Space(3)]
    [Header("PARTICLES")]
    [SerializeField] private GameObject dieParticle;
    [SerializeField] private GameObject attackParticle;
    [SerializeField] private GameObject confettiParticle;
    [SerializeField] private GameObject speedUpParticle;


    [Space(3)]
    [Header("ATTRIBUTES")]
    [SerializeField] private PlayerAttribute weightAttribute;
    [SerializeField] private PlayerAttribute scaleAttribute;
    [SerializeField] private PlayerAttribute speedAttribute;
    [SerializeField] private PlayerAttribute animSpeedAttribute;
    [SerializeField] private PlayerAttribute idleAnimBlendAttribute;

    private string playerName;
    private Sprite flagIcon;
    private bool bIsAlive;
    private Coroutine ChangeSpeedCoroutine;
    private int level;
    private int numberOfCollectOnCurrentLevel;
    private bool bInSpeedUpEffect;
    private int numberOfCollectToLevelUp = 3;

    public bool IsAlive
    {
        get { return bIsAlive; }
        set
        {
            bIsAlive = value;
            controller.ControllerStatus = bIsAlive;
            interactionCollider.SetEnabled(bIsAlive);
        }
    }

    public int Level
    {
        get { return level; }
    }
    public float HealthMax
    {
        get { return weightAttribute.MaxVal; }
        set { weightAttribute.MaxVal = value; }
    }

    public bool IsLeader
    {
        set
        {
            crownGO.SetActive(value);
        }
    }

    public void FirstInputGiven()
    {
        OnFirstInputGiven?.Invoke();
    }

    public string PlayerName
    {
        get { return playerName; }
        set
        {
            playerName = value;
            playerNameDisplayer.SetPlayerName(playerName);
        }
    }

    public Sprite FlagIcon
    {
        get { return flagIcon; }
        set
        {
            flagIcon = value;
            playerNameDisplayer.SetIcon(flagIcon);
        }
    }

    public Character Character
    {
        get { return character; }
    }

    public bool IsMainPlayer { get; set; }

    public float CollectCountPercent
    {
        get { return (float)numberOfCollectOnCurrentLevel / (float)GetNumberOfCollectToLevelUp(level); }
    }

    private void Start()
    {
        interactionCollider.OnCollision += RangeCollider_OnCollision;
    }

    public void SetMovementDirection(Vector3 moveVector)
    {
        Character.SetMovementDirection(moveVector);
    }

    public void ResetPlayer()
    {
        bInSpeedUpEffect = false;
        level = 0;
        numberOfCollectOnCurrentLevel = 0;
        IsLeader = false;
        playerNameDisplayer.gameObject.SetActive(true);
        numberOfCollectToLevelUp = IsMainPlayer ? 3 : Random.Range(3, 5);
        ResetAttributes();

        ChangeWeight(0);
        ChangeSpeed(0);
        ChangeScale(0);
        ChangeAnimSpeed(0);
        ChangeIdleAnimBlend(0);

        Character.ResetCharacter();
        controller.ResetController();
    }

    public void Activate()
    {
        gameObject.SetActive(true);
        dieParticle.SetActive(false);
        attackParticle.SetActive(false);
        confettiParticle.SetActive(false);
    }

    public void Deactivate()
    {
        gameObject.SetActive(false);
    }

    public void Translate(Vector3 vector3)
    {
        Character.Translate(vector3);
    }

    private int GetNumberOfCollectToLevelUp(int level)
    {
        return numberOfCollectToLevelUp;
    }

    public PlayerInteractionCollider GetInteractionCollider()
    {
        return interactionCollider;
    }

    public Vector3 GetCharacterPosition()
    {
        return Character.transform.position;
    }

    private void RangeCollider_OnCollision(Player collidedPlayer)
    {
        int comparedLevel = IsMainPlayer ? Level : Level - 1;

        if (collidedPlayer.Level < comparedLevel)
        {
            Attack(collidedPlayer);
        }
    }

    private void Attack(Player collidedPlayer)
    {
        Vector3 dir = (collidedPlayer.GetCharacterPosition() - GetCharacterPosition()).normalized;

        collidedPlayer.Die(dir);
        int collidedPlayerSize = collidedPlayer.level;
        StartCoroutine(AttackRoutine(collidedPlayerSize));
    }

    private IEnumerator AttackRoutine(int collidedPlayerSize)
    {
        attackParticle.SetActive(false);
        attackParticle.SetActive(true);
        InteractWithBloater(3);
        yield return null;
    }

    public void Die(Vector3 force)
    {
        StartCoroutine(DieRoutine(force));
    }

    private IEnumerator DieRoutine(Vector3 force)
    {
        IsAlive = false;

        playerNameDisplayer.gameObject.SetActive(false);
        if (IsMainPlayer)
        {
            MMVibrationManager.Haptic(HapticTypes.Failure, false, true, this);
        }

        while (scaleAttribute.CurrentVal > scaleAttribute.MinVal)
        {
            ChangeScale(-1);
            yield return new WaitForSeconds(0.05f);
        }
        dieParticle.SetActive(true);

        ResetAttributes();
        Character.Die(force);
        OnPlayerDie?.Invoke(this);
    }

    public void Win()
    {
        Character.DanceAnim();
        confettiParticle.SetActive(true);
    }

    public void Lose()
    {
        Character.SadAnim();
    }

    #region Attributes
    private void ResetAttributes()
    {
        weightAttribute.ResetAttribute();
        scaleAttribute.ResetAttribute();
        speedAttribute.ResetAttribute();
        animSpeedAttribute.ResetAttribute();
        idleAnimBlendAttribute.ResetAttribute();
    }
    public void ChangeWeight(int changeTimes)
    {
        weightAttribute.CurrentVal += weightAttribute.IncrementAmount * changeTimes;
        Character.ChangeSize(weightAttribute.CurrentVal, IsMainPlayer);
    }

    public void ChangeSpeed(int changeTimes)
    {
        speedAttribute.CurrentVal += speedAttribute.IncrementAmount * changeTimes;
        if (!bInSpeedUpEffect)
            controller.MoveSpeed = speedAttribute.CurrentVal;
    }

    public void ChangeScale(int changeTimes)
    {
        scaleAttribute.CurrentVal += scaleAttribute.IncrementAmount * changeTimes;
        Character.ChangeScale(scaleAttribute.CurrentVal);
    }

    public void ChangeAnimSpeed(int changeTimes)
    {
        animSpeedAttribute.CurrentVal += animSpeedAttribute.IncrementAmount * changeTimes;
        if (!bInSpeedUpEffect)
            Character.ChangeAnimSpeed(animSpeedAttribute.CurrentVal);
    }

    public void ChangeIdleAnimBlend(int changeTimes)
    {
        idleAnimBlendAttribute.CurrentVal += idleAnimBlendAttribute.IncrementAmount * changeTimes;
        character.ChangeIdleAnimBlendVal(idleAnimBlendAttribute.CurrentVal);
    }
    #endregion

    #region IInteractionist Implementation
    public void InteractWithBloater(int changeTimes)
    {
        if (IsMainPlayer)
        {
            MMVibrationManager.Haptic(HapticTypes.RigidImpact, false, true, this);
        }

        numberOfCollectOnCurrentLevel += changeTimes;

        while (numberOfCollectOnCurrentLevel > GetNumberOfCollectToLevelUp(level))
        {
            numberOfCollectOnCurrentLevel -= GetNumberOfCollectToLevelUp(level);
            level++;
            ChangeWeight(1);
            ChangeSpeed(-1);
            ChangeAnimSpeed(-1);
            ChangeScale(1);
            ChangeIdleAnimBlend(1);
            OnPlayerLevelUp?.Invoke();
        }
        OnSizeChanged?.Invoke();
    }

    public void InteractWithSpeedUp(float time)
    {
        if (ChangeSpeedCoroutine == null)
        {
            ChangeSpeedCoroutine = StartCoroutine(RedoSpeedChangeForSec(time));
        }
    }
    #endregion

    private IEnumerator RedoSpeedChangeForSec(float time)
    {
        bInSpeedUpEffect = true;
        speedUpParticle.SetActive(true);

        controller.MoveSpeed = speedAttribute.MaxVal;
        Character.ChangeAnimSpeed(animSpeedAttribute.MaxVal);

        yield return new WaitForSeconds(time);

        controller.MoveSpeed = speedAttribute.CurrentVal;
        Character.ChangeAnimSpeed(animSpeedAttribute.CurrentVal);

        speedUpParticle.SetActive(false);
        bInSpeedUpEffect = false;

        ChangeSpeedCoroutine = null;
    }
}

public enum PlayerState
{
    None,
    Dead,
    Moving,
    Idle
}


