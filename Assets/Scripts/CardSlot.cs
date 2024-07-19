using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using MyBox;
using DG.Tweening;
using System;
using UnityEngine.EventSystems;

public class CardSlot : MonoBehaviour
{
    [SerializeField]
    public bool receiveCard = true;

    [field: SerializeField]
    public Card card { get; private set; }

    [SerializeField]
    private Transform graphicTransform;

    public void Initialize(bool receiveCard)
    {
        this.receiveCard = receiveCard;
    }
    public void CardPlaced(Card card)
    {
        if(this.card == null)
        {
            this.card = card;
            card.transform.parent = this.transform;
        }
        else
        {
            //  Merge two cards
            LLMManager.Instance.CardMerger.MergeCards(this.card.CardInfo, card.CardInfo, OnRequestComplete);
            Deck.Instance.AddCard(this.card);
            this.card = card;
            this.card.Initialize(CardInfo.EmptyCardInfo(), this.transform);
            receiveCard = false;
        }
    }
    private void OnRequestComplete(CardInfo cardInfo)
    {
        this.card.Initialize(cardInfo, this.transform);
        receiveCard = true;
    }
    public void CardRemoved()
    {
        card = null;
    }
}