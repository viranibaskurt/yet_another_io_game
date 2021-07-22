using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Character : MonoBehaviour
{
    private const string bIsMovingKey = "bIsMoving";

    [SerializeField] private GameObject characterMeshGO;
    [SerializeField] private Animator anim;

    [SerializeField] private Transform bodiesRoot;
    [SerializeField] private Transform hairsRoot;
    [SerializeField] private Transform slippersRoot;
    private float forceMultiplier = 900.0f;

    private List<SkinnedMeshRenderer> skinnedMeshes = new List<SkinnedMeshRenderer>();
    private Collider rootCollider;
    private Rigidbody rootRigidbody;
    private float turnSpeed = 20.0f;
    private List<RagdollComponents> ragdollComponents = new List<RagdollComponents>();

    private Quaternion rotation = Quaternion.identity;
    private TweenerCore<float, float, FloatOptions> changeSizeTween;

    public float TurnSpeed
    {
        get { return turnSpeed; }
        set { turnSpeed = value; }
    }

    private void Awake()
    {
        rootCollider = GetComponent<Collider>();
        rootRigidbody = GetComponent<Rigidbody>();

        skinnedMeshes = new List<SkinnedMeshRenderer>();
        foreach (Transform item in bodiesRoot)
        {
            SkinnedMeshRenderer temp = item.GetComponent<SkinnedMeshRenderer>();
            if (temp)
            {
                skinnedMeshes.Add(temp);
            }
        }
    }


    private void Start()
    {
        InitializeRigidbodyComponents();
        ResetCharacter();
    }

    private void OnDestroy()
    {
        if (changeSizeTween != null)
        {
            changeSizeTween.Kill();
        }
    }

    public void ResetCharacter()
    {
        SetRagdollState(false);
        anim.SetTrigger(AnimationKeys.Reset);
        ChooseRandomSkinConfiguration();

        rootCollider.enabled = true;
        rootRigidbody.isKinematic = false;
    }

    public void Activate()
    {
        gameObject.SetActive(true);
        ResetCharacter();
    }

    public void Deactivate()
    {
        gameObject.SetActive(false);
    }

    public void SetMovementDirection(Vector3 moveVector)
    {
        bool bIsMoving = moveVector.sqrMagnitude > 0;
        anim.SetBool(bIsMovingKey, bIsMoving);
        if (bIsMoving)
        {
            Vector3 desiredForward = Vector3.RotateTowards(transform.forward, moveVector, TurnSpeed * Time.deltaTime, 0f);
            rotation = Quaternion.LookRotation(desiredForward);
            transform.rotation = rotation;
        }
    }
    public void Translate(Vector3 v)
    {
        transform.Translate(v, Space.World);
    }

    public void ChangeSize(float val, bool bIsAnimating)
    {
        if (bIsAnimating)
        {
            float currentVal = skinnedMeshes[0].GetBlendShapeWeight(0);
            changeSizeTween = DOTween.To(() => currentVal, x => currentVal = x, val, 0.5f)
                .OnUpdate(
                    () => skinnedMeshes.ForEach(item => item.SetBlendShapeWeight(0, currentVal))
                )
                .SetEase(Ease.OutElastic);
        }
        else
        {
            skinnedMeshes.ForEach(item => item.SetBlendShapeWeight(0, val));
        }

        //skinnedMeshes.ForEach(item => item.SetBlendShapeWeight(0, val));
    }

    public void ChangeScale(float val)
    {
        transform.localScale = Vector3.one * val;

    }

    public void ChangeAnimSpeed(float val)
    {
        anim.speed = val;
    }

    public void SetRagdollState(bool state)
    {
        anim.enabled = !state;
        rootCollider.enabled = !state;
        rootRigidbody.isKinematic = state;
        foreach (RagdollComponents item in ragdollComponents)
        {
            item.SetState(state);
            if(!state)
            {
                item.rigidbody.velocity = Vector3.zero;
            }
        }
    }

    public void SadAnim()
    {
        //anim.SetTrigger(AnimationKeys.Sad);
    }

    public void DanceAnim()
    {
        //anim.SetTrigger(AnimationKeys.Dance);
    }

    public void Die(Vector3 forceDirection)
    {
        //anim.SetTrigger(AnimationKeys.Die);
        SetRagdollState(true);

        if (ragdollComponents.Count > 0)
        {
            forceDirection += Vector3.up/5.0f;
            ragdollComponents[0].rigidbody.AddForce(forceDirection * forceMultiplier, ForceMode.Impulse);

        }
    }

    private void ChooseRandomSkinConfiguration()
    {

        foreach (Transform item in bodiesRoot)
        {
            item.gameObject.SetActive(false);
        }

        foreach (Transform item in slippersRoot)
        {
            item.gameObject.SetActive(false);
        }

        foreach (Transform item in hairsRoot)
        {
            item.gameObject.SetActive(false);
        }

        bodiesRoot.GetChild(Random.Range(0, bodiesRoot.childCount)).gameObject.SetActive(true);
        slippersRoot.GetChild(Random.Range(0, slippersRoot.childCount)).gameObject.SetActive(true);
        hairsRoot.GetChild(Random.Range(0, hairsRoot.childCount)).gameObject.SetActive(true);
    }

    private void InitializeRigidbodyComponents()
    {
        Collider[] colliders = characterMeshGO.GetComponentsInChildren<Collider>();
        foreach (Collider item in colliders)
        {
            RagdollComponents comp = new RagdollComponents();
            comp.collider = item;
            comp.rigidbody = item.GetComponent<Rigidbody>();
            comp.characterJoint = item.GetComponent<CharacterJoint>();
            ragdollComponents.Add(comp);
        }
    }
    struct RagdollComponents
    {
        public Collider collider;
        public Rigidbody rigidbody;
        public CharacterJoint characterJoint;

        public void SetState(bool state)
        {
            if (collider)
            {
                collider.enabled = state;
            }
            if (rigidbody)
            {
                rigidbody.isKinematic = !state;
                rigidbody.useGravity = state;
            }
            if (characterJoint)
            {
                characterJoint.enableCollision = state;
            }
        }

    };

    public void ChangeIdleAnimBlendVal(float currentVal)
    {
        anim.SetFloat("IdleBlend", currentVal);
    }

    struct AnimationKeys
    {
        public const string Die = "Die";
        public const string Sad = "Sad";
        public const string Dance = "Dance";
        public const string Reset = "Reset";

    }
}

