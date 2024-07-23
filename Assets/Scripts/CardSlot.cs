using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using MyBox;
using DG.Tweening;
using System;
using UnityEngine.EventSystems;
using Unity.VisualScripting;

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
    /// <summary>
    /// Return false, the card will go back to the hand
    /// </summary>
    /// <param name="card"></param>
    /// <returns></returns>
    public bool TryPlaceCard(Card card)
    {
        if (!this.receiveCard)
            return false;
        //  No card here, can be placed
        else if (this.card == null)
        {
            this.card = card;
            card.AddToSlot(this);
            return true;
        }
        //  The same card, do nothing
        else if (this.card == card)
        {
            return false;
        }
        //  Already has a card, will try to merge
        else if (GameManager.Instance.HasEnoughActionPoint(2)
            && LLMManager.Instance.CardMerger.MergeCards(this.card.CardInfo, card.CardInfo, OnRequestComplete))
        {
            GameManager.Instance.TryConsumeActionPoint(2);
            //  Merge two cards
            Deck.Instance.AddCard(this.card);
            Deck.Instance.AddCard(card);

            this.card = LLMManager.Instance.CardGenerator.GenerateKnownCard(CardInfo.EmptyCardInfo(), this.transform, false);
            receiveCard = false;
            return true;
        }
       return false;
       
    }
    private void OnRequestComplete(CardInfo cardInfo)
    {
        this.card.Initialize(cardInfo, this.transform, true);
        receiveCard = true;
    }
    public void CardRemoved()
    {
        card = null;
    }
}