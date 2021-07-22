using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blackboard : MonoBehaviour
{
    [HideInInspector] public Player closestPlayer;
    public float chaseTime = 4.0f;

    public float stoppingDistance = 1.0f;
    public float lookAroundEnemyRadius = 3.0f;
    public float lookAroundPickupRadius = 5.0f;

    public bool bIsInDanger { get; set; }
    public Vector3 TargetPos { get; set; }
}
