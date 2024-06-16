using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using MyBox;
using DG.Tweening;
using System;

public class CardSlot : MonoBehaviour
{
    [SerializeField]
    public bool receiveCard = true;

    [field: SerializeField]
    public Card card { get; private set; }

    [SerializeField]
    private Transform graphicTransform;

    public void CardPlaced(Card card)
    {
        this.card = card;
    }
    public void CardRemoved()
    {
        card = null;
    }
}