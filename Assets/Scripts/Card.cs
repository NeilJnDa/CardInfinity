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
    [ReadOnly]
    private bool pickedUp = false;
    [SerializeField]
    [ReadOnly]
    private CardInfo cardInfo;
    [SerializeField]
    [ReadOnly]
    private CardSlot currentSlot = null;


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
        if (cardInfo != null)
        {
            this.cardInfo = cardInfo;
            nameText.text = cardInfo.name;
            hpText.text = cardInfo.health.ToString();
            descriptionText.text = cardInfo.description;
        }

    }
    private void FixedUpdate()
    {
        if(pickedUp)
        {
            mRigidBody.MovePosition(targetPosition);           
        }
    }

    public void PickUp()
    {
        pickedUp = true;
    }
    public void Drop()
    {
        pickedUp = false;
    }

    /// <summary>
    /// Called every frame
    /// </summary>
    /// <param name="hoveringSlot"></param>
    public void SetMoveTarget(Vector3 position)
    {
        targetPosition = position;
    }
}
