using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LLMUnity;
using MyBox;
using System;

[ExecuteInEditMode]
public class CardMerger : MonoBehaviour
{
    #region MergeFromTwoSlots
    [SerializeField]
    private Card cardPrefab;
    [SerializeField]
    private CardSlot slot1;
    [SerializeField] 
    private CardSlot slot2;
    [SerializeField] 
    private CardSlot outputSlot;

    [SerializeField]
    [ReadOnly]
    private Card card1;

    [SerializeField]
    [ReadOnly]
    private Card card2;

    [SerializeField]
    [ReadOnly]
    private Card outputCard;

    [ButtonMethod]
    private void GenerateRequest()
    {
        outputCard = null;
        result = "";
        card1 = slot1.card;
        card2 = slot2.card;

        MergeCards(card1.CardInfo, card2.CardInfo, RequestCompleted);
    }
    private void RequestCompleted(CardInfo cardInfo)
    {
        var card = Instantiate(cardPrefab, outputSlot.transform.position, outputSlot.transform.rotation);
        card.Initialize(cardInfo, outputSlot.transform);
        outputCard = card;
    }
    #endregion

    [SerializeField]
    [ReadOnly]
    private bool waitingForResponse = false;

    public LLMCharacter llmCharacter;
    [TextArea(3, 5)]
    public string command = "";

    [SerializeField]
    [TextArea(3, 5)]
    private string result = "";

    private Action<CardInfo> OnAICompleteEvent;
    [SerializeField]
    private int actionPointPerMerge = 2;
    private void OnAIReturnToken(string text)
    {
        result = text;
    }
    private void OnAIComplete()
    {
        waitingForResponse = false;
        var cardInfo = Utils.FromJson<CardInfo>(result);
        OnAICompleteEvent.Invoke(cardInfo);
        OnAICompleteEvent = null;
    }
    public bool MergeCards(CardInfo cardInfo1, CardInfo cardInfo2, Action<CardInfo> callback)
    {
        if (waitingForResponse)
        {
            Debug.LogWarning(this.name + " is waiting for response, submit request later");
            return false;
        }

        string completeCommand = command + cardInfo1.ToJson() + cardInfo2.ToJson();
        Debug.Log(completeCommand);
        _ = llmCharacter.Chat(completeCommand, OnAIReturnToken, OnAIComplete, false);
        waitingForResponse = true;
        OnAICompleteEvent += callback;
        return true;
    }
}
