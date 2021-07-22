using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
[CreateAssetMenu(fileName = "NicknameList", menuName = "Create new nickname list")]
public class NicknameList : ScriptableObject
{
    public List<string> names = new List<string>();
    public List<Sprite> icons = new List<Sprite>();
    public Sprite mainPlayerIcon;
}

[Serializable]
public class NameList
{
    public List<string> names = new List<string>();
}



