using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using MyBox;
using UnityEditor;
using System;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements;
using DG.Tweening;

public enum CardState
{
    Idle,
    PickedUp,
    FinishingMovingAfterDrop,
}
public class Card : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField]
    private float lerpSpeed = 3f;
    [SerializeField]
    private bool pickable = true;
    [SerializeField]
    [ReadOnly]
    private CardState state = CardState.Idle;
    
    [SerializeField]
    private CardInfo cardInfo;
    public CardInfo CardInfo { get { return cardInfo; }}

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
    [SerializeField]
    [ReadOnly]
    private Quaternion targetRotation;
    private void Start()
    {
        mainCamera = Camera.main;
        mRigidBody = GetComponent<Rigidbody>();
    }
    public void Initialize(CardInfo cardInfo, Transform cardInitialTransform, bool pickable = true)
    {
        this.state = CardState.Idle;
        this.pickable = pickable;
        this.cardInfo = cardInfo;
        nameText.text = cardInfo.name;
        hpText.text = cardInfo.health.ToString();
        descriptionText.text = cardInfo.description;
        this.transform.position = cardInitialTransform.position;
        this.transform.rotation = cardInitialTransform.rotation;
        SetTargetTransform(cardInitialTransform);
    }

    public void SetTargetTransform(Transform transform)
    {
        targetPosition = transform.position;
        targetRotation = transform.rotation;
    }

    private void FixedUpdate()
    {
        if(state == CardState.PickedUp)
        {
            //  Move the a slot just for showing. Will not place it.
            if (PointerManager.Instance.HoveringSlot != null && PointerManager.Instance.HoveringSlot.receiveCard)
            {
                targetPosition = PointerManager.Instance.HoveringSlot.transform.position - PointerManager.Instance.HoveringSlot.transform.forward * 0.07f;
                targetRotation = PointerManager.Instance.HoveringSlot.transform.rotation;
            }
            //  Or just follow mouse position
            else
            {
                Ray ray = PointerManager.Instance.CameraRay();
                float enter = 0.0f;
                var res = GameManager.Instance.CardHoverPlane.Raycast(ray, out enter);
                Vector3 hitPoint = ray.GetPoint(enter);
                targetPosition = hitPoint;
            }
            mRigidBody.MovePosition(Vector3.Lerp(this.transform.position, targetPosition, Mathf.Clamp01(Time.deltaTime * lerpSpeed)));
            mRigidBody.MoveRotation(Quaternion.Lerp(this.transform.rotation, targetRotation, Mathf.Clamp01(Time.deltaTime * lerpSpeed)));
        }
        if(state == CardState.FinishingMovingAfterDrop)
        {
            mRigidBody.MovePosition(Vector3.Lerp(this.transform.position, targetPosition, Mathf.Clamp01(Time.deltaTime * lerpSpeed)));
            mRigidBody.MoveRotation(Quaternion.Lerp(this.transform.rotation, targetRotation, Mathf.Clamp01(Time.deltaTime * lerpSpeed)));

            if (Math.Abs(Vector3.Distance(this.transform.position, targetPosition)) < 1e-2)
            {
                //  Close enough to the target
                state = CardState.Idle;
            }
        }

    }

    private void PickUp()
    {
        state = CardState.PickedUp;
        currentSlot?.CardRemoved();
        currentSlot = null;
        PointerManager.Instance.HoldCard(this);

    }
    private void Drop()
    {
        PointerManager.Instance.DropCard();

        //  If pointer is hovering on a slot, then this card will try to be placed there
        CardSlot targetSlot = PointerManager.Instance.HoveringSlot;

        if (targetSlot != null && targetSlot.TryPlaceCard(this)) {
            
            if (CardsInHand.Instance.Cards.Contains(this))
                CardsInHand.Instance.RemoveCard(this);
        }
        //  Otherwise, go back to the hand.
        else
        {
            if (!CardsInHand.Instance.Cards.Contains(this))
            {
                CardsInHand.Instance.AddCard(this);
            }
            state = CardState.Idle;
        }
        CardsInHand.Instance.OrganizeCardAnim();

    }
    public void AddToDeck(Deck deck)
    {
        this.transform.DOKill();
        this.state = CardState.Idle;
        this.transform.parent = deck.transform;
        this.transform.DOMove(deck.transform.position, 0.5f);
    }
    public void AddToSlot(CardSlot slot)
    {
        this.transform.DOKill();
        this.transform.parent = slot.transform;
        this.targetPosition = slot.transform.position;
        this.currentSlot = slot;
        state = CardState.FinishingMovingAfterDrop;
    }

    #region Combat
    public bool DealDamage(int damage)
    {

        cardInfo.health -= damage;
        if (cardInfo.health < 0)
        {
            //  Die
        }
        return true;

        //else
        //    return false;
    }
    #endregion
    #region PointerEvents
    public void OnPointerDown(PointerEventData eventData)
    {
        if(pickable)
            PickUp();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (pickable)
            Drop();
    }

    #endregion
}
