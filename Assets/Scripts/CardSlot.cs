using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using MyBox;
using DG.Tweening;

public class CardSlot : MonoBehaviour
{
    [SerializeField]
    [ReadOnly]
    private Card card = null;

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
    public void CardEnter()
    {

    }
    public void CardExit()
    {

    }
}