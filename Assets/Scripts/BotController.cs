using CleverCrow.Fluid.BTs.Tasks;
using CleverCrow.Fluid.BTs.Trees;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class BotController : ControllerBase
{

    [SerializeField] private BehaviorTree behaviorTree;
    [SerializeField] NavMeshAgent navMeshAgent;
    [SerializeField] LayerMask interactionColliderLayerMask;
    [SerializeField] LayerMask pickUpLayerMask;

    private Blackboard blackboard;
    float chaseTimer;
    public override float MoveSpeed
    {
        get
        {
            return navMeshAgent.speed;
        }
        set
        {
            navMeshAgent.speed = value;
        }
    }

    public override bool ControllerStatus
    {
        get => base.ControllerStatus;
        set
        {
            base.ControllerStatus = value;
            navMeshAgent.isStopped = !value;
        }
    }

    private void Awake()
    {
        blackboard = GetComponent<Blackboard>();

        behaviorTree = new BehaviorTreeBuilder(gameObject)
            .RepeatForever()
                .Selector()
                    .Sequence("In Combat Sequence")
                        .Condition("Is In Danger?", () => blackboard.bIsInDanger)
                        .Selector("Combat Decision Selector")
                            .Sequence("Attac Seq")
                                .Condition("Can I Win?", CanIWin)
                                .Do("Init Chase Timer", InitChaseTimer)
                                .Do("Chase Enemy", ChaseEnemy)
                            .End()
                            .Sequence("Escape Seq")
                                .Do("Escape", Escape)
                                .Do("Go to Escape Pos", GoToEscapePos)
                            .End()
                        .End()
                    .End()
                    .Sequence("Wander Sequence")
                        .Selector("Wander Selector")
                            .Sequence("Pick up Sequence")
                                .Condition("Any pick up around", LookPickUpAround)
                                .Do("Go to pick up", GoToPickUp)
                            .End()
                        .Sequence("Random Wander Selector")
                            .Do("Find Random Position", FindRandomPosition)
                            .Do("Wander Randomly", Wander)
                        .End()
                        .End()
                    .End()
                .End()
            .End()
            .Build();
    }

    private void Update()
    {

#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            //controlledPlayer.InteractWithSizeChanger(1);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            //controlledPlayer.InteractWithSizeChanger(-1);
        }
#endif

        if (!ControllerStatus)
        {
            return;
        }

        behaviorTree.Tick();

        bool tempIsInDanger = IsInDanger();
        if (blackboard.bIsInDanger != tempIsInDanger)
        {

            blackboard.bIsInDanger = tempIsInDanger;
            behaviorTree.Reset();
        }

    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        if (Application.isPlaying)
        {
            Debug.Log("Controller Status for " + controlledPlayer.name + " : " + ControllerStatus);

            if (blackboard == null || blackboard.TargetPos == null)
                return;

            Color originalColor = Gizmos.color;
            Gizmos.color = Color.cyan;
            Gizmos.DrawSphere(blackboard.TargetPos, 1.0f);
            Gizmos.color = originalColor;
            Handles.Label(blackboard.TargetPos, $"{controlledPlayer.gameObject.name}- Target");
        }
    }

#endif

    public override void ResetController()
    {
        base.ResetController();
        behaviorTree.Reset();
    }

    private TaskStatus GoToPickUp()
    {
        return GoToTarget(blackboard.TargetPos);
    }

    private bool LookPickUpAround()
    {
        Collider closestPickup = FindClosest(controlledPlayer.GetCharacterPosition(), blackboard.lookAroundPickupRadius, pickUpLayerMask);
        if (closestPickup)
        {
            blackboard.TargetPos = closestPickup.transform.position;
            return true;
        }
        else
        {
            return false;
        }
    }

    private TaskStatus FindRandomPosition()
    {

        blackboard.TargetPos = GetRandomPosition();

        return TaskStatus.Success;
    }

    private Vector3 GetRandomPosition()
    {
        float radius = 50.0f;
        Vector2 randomDirection2D = Random.insideUnitCircle * radius;
        Vector3 randomDirection3D = new Vector3(randomDirection2D.x, 0.0f, randomDirection2D.y);

        randomDirection3D += controlledPlayer.GetCharacterPosition();
        Vector3 result = Vector3.zero;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomDirection3D, out hit, radius, NavMesh.AllAreas))
        {
            result = hit.position;
            result.y = 0.0f;
        }
        return result;
    }

    private TaskStatus Wander()
    {
        return GoToTarget(blackboard.TargetPos);
    }

    private TaskStatus InitChaseTimer()
    {
        chaseTimer = blackboard.chaseTime;
        return TaskStatus.Success;
    }

    private TaskStatus Escape()
    {
        if (!blackboard.closestPlayer)
        {
            Debug.Log("Escape failed. ClosestPlayer is null!");
            return TaskStatus.Failure;
        }

        //TODO find pt to escape
        Vector3 controlledCharPos = controlledPlayer.GetCharacterPosition();
        Vector3 enemyPos = blackboard.closestPlayer.GetCharacterPosition();
        Vector3 randomPosition, toEnemy, toRandPos;

        randomPosition = GetRandomPosition();
        toEnemy = enemyPos - controlledCharPos;
        toRandPos = randomPosition - controlledCharPos;

        for (int i = 0; i < 1000; i++)
        {
            randomPosition = GetRandomPosition();
            toEnemy = enemyPos - controlledCharPos;
            toRandPos = randomPosition - controlledCharPos;
            float dot = Vector3.Dot(toEnemy, toRandPos);
            if (dot < 0)
            {
                blackboard.TargetPos = randomPosition;
                break;
            }
        }

        return TaskStatus.Success;
    }

    private TaskStatus GoToEscapePos()
    {
        return GoToTarget(blackboard.TargetPos);
    }

    private TaskStatus ChaseEnemy()
    {
        if (blackboard.closestPlayer == null || !blackboard.closestPlayer.IsAlive)
            return TaskStatus.Success;

        chaseTimer -= Time.deltaTime;
        if (chaseTimer <= 0)
            return TaskStatus.Success;

        return GoToTarget(blackboard.closestPlayer.GetCharacterPosition());
    }

    private TaskStatus Attack()
    {
        return TaskStatus.Success;
    }

    private TaskStatus GoToTarget(Vector3 target)
    {
        if (Vector3.SqrMagnitude(controlledPlayer.GetCharacterPosition() - target) < blackboard.stoppingDistance)
        {
            controlledPlayer.SetMovementDirection(Vector3.zero);
            return TaskStatus.Success;
        }

        navMeshAgent.SetDestination(target);
        controlledPlayer.SetMovementDirection(navMeshAgent.velocity.normalized);
        return TaskStatus.Continue;
    }

    private bool CanIWin()
    {
        if (blackboard.closestPlayer == null)
            return false;

        return controlledPlayer.Level > blackboard.closestPlayer.Level;
    }

    private bool IsInDanger()
    {
        RefreshClosestPlayer();
        // Debug.Log(blackboard.closestPlayer != null);
        return blackboard.closestPlayer != null;
    }

    private void RefreshClosestPlayer()
    {
        Vector3 originPos = controlledPlayer.GetCharacterPosition();

        Collider closestPlayerCharCollider = FindClosest(originPos
                                                            , blackboard.lookAroundEnemyRadius
                                                            , interactionColliderLayerMask
                                                            , new List<Collider> { controlledPlayer.GetInteractionCollider().GetComponent<Collider>() }
                                                        );

        if (closestPlayerCharCollider)
        {
            PlayerInteractionCollider otherInteractionCol = closestPlayerCharCollider.GetComponent<PlayerInteractionCollider>();
            blackboard.closestPlayer = (otherInteractionCol.Player.IsAlive) ? otherInteractionCol.Player : null;
        }
        else
        {
            blackboard.closestPlayer = null;
        }
    }

    private Collider FindClosest(Vector3 originPos, float radius, LayerMask mask, List<Collider> ignored = null)
    {
        Collider[] colliders = Physics.OverlapSphere(originPos, radius, mask, QueryTriggerInteraction.Collide);
        Collider closest = null;
        float minDistance = float.MaxValue;
        foreach (Collider item in colliders)
        {
            if (ignored != null && ignored.Contains(item))
                continue;

            if (closest == null)
            {
                closest = item;
                minDistance = Vector3.SqrMagnitude(originPos - item.transform.position);
                continue;
            }

            float tempDistance = Vector3.SqrMagnitude(originPos - item.transform.position);
            if (tempDistance < minDistance)
            {
                minDistance = tempDistance;
                closest = item;
            }
        }
        return closest;
    }

}
