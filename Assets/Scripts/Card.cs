using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using MyBox;
using UnityEditor;
using System;

public class Card : MonoBehaviour
{
    [SerializeField]
    private float lerpSpeed = 3f;
    [SerializeField]
    [ReadOnly]
    private bool pickedUp = false;
    [SerializeField]
    [ReadOnly]
    private CardInfo cardInfo;
    public CardInfo CardInfo { get { return cardInfo; }}

    [SerializeField]
    [ReadOnly]
    private CardSlot currentSlot = null;
    /// <summary>
    /// When it is moving, which slot is overlapped and could be placed, but not yet placed.
    /// </summary>
    [SerializeField]
    [ReadOnly]
    private CardSlot overlappingSlot = null;



    [Header("Reference")]
    [SerializeField]
    private TMP_Text nameText;
    [SerializeField]
    private TMP_Text hpText;
    [SerializeField]
    private TMP_Text descriptionText;

    [Foldout("Local Cache", true)]
    [SerializeField]
    [ReadOnly]
    private Camera mainCamera;
    [SerializeField]
    [ReadOnly]
    private Rigidbody mRigidBody;
    [SerializeField]
    [ReadOnly]
    private Vector3 targetPosition;
    private void Start()
    {
        mainCamera = Camera.main;
        mRigidBody = GetComponent<Rigidbody>();
    }
    public void Initialize(CardInfo cardInfo)
    {
        this.cardInfo = cardInfo;
        nameText.text = cardInfo.name;
        hpText.text = cardInfo.health.ToString();
        descriptionText.text = cardInfo.description;
        
    }
    private void FixedUpdate()
    {
        if(pickedUp)
        {
            mRigidBody.MovePosition(Vector3.Lerp(this.transform.position, targetPosition, Mathf.Clamp01(Time.deltaTime * lerpSpeed)));        
        }
    }

    public void PickUp()
    {
        pickedUp = true;
        overlappingSlot = null;
        currentSlot?.CardRemoved();
        currentSlot = null;
    }
    public void Drop()
    {
        pickedUp = false;
        if (overlappingSlot)
        {
            overlappingSlot.CardPlaced(this);
            this.currentSlot = overlappingSlot;
            overlappingSlot = null;
        }
    }

    public void SetMoveTarget(Vector3 position)
    {
        targetPosition = position;
    }

    private void OnTriggerEnter(Collider other)
    {
        var slot = other.GetComponent<CardSlot>(); 
        if(slot != null && slot.receiveCard)
        {
            overlappingSlot = slot;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        var slot = other.GetComponent<CardSlot>();
        if (slot != null && overlappingSlot == slot)
        {
            overlappingSlot = null;
        }
    }
}
