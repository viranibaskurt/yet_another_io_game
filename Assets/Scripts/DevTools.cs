#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class DevTools : MonoBehaviour
{
    [SerializeField] private TextAsset nicknameFile;
    [SerializeField] private NicknameList nicknameListSO;

    private void Start()
    {
        nicknameListSO.names = JsonUtility.FromJson<NameList>(nicknameFile.text).names;
        EditorUtility.SetDirty(nicknameListSO);

    }
}

#endif