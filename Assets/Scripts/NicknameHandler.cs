using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NicknameHandler : MonoBehaviour
{
    [SerializeField] NicknameList nicknameListSO;

    private int nicknameIndex = 0;


    private void Start()
    {
        nicknameListSO.names.Shuffle();
    }

    public string GetNextNickName()
    {
        nicknameIndex++;
        nicknameIndex %= nicknameListSO.names.Count;
        return nicknameListSO.names[nicknameIndex];
    }

    public Sprite GetRandomFlagIcon()
    {
        return nicknameListSO.icons[Random.Range(0, nicknameListSO.icons.Count)];
    }
    public Sprite GetMainPlayerFlagIcon()
    {
        return nicknameListSO.mainPlayerIcon;
    }
}
