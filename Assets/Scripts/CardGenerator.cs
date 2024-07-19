using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LLMUnity;
using MyBox;
using System;

[ExecuteInEditMode]
public class CardGenerator : MonoBehaviour
{
    [SerializeField]
    private Card cardPrefab;
    [SerializeField]
    private Transform cardDefaultTransform;
    [SerializeField]
    [ReadOnly]
    private bool waitingForResponse = false;

    public LLMCharacter llmCharacter;
    [TextArea(3, 5)]
    public string command = "";

    [SerializeField]
    [TextArea(3, 5)]
    private string result = "";

    private Action<Card> OnAICompleteEvent;
    private Transform cardInitialTransform;

    public void GenerateNewCard(Transform initialTransform, Action<Card> callback)
    {
        if (!waitingForResponse)
        {
            _ = llmCharacter.Chat(command, OnAIReturnToken, OnAIComplete, false);
            waitingForResponse = true;
            cardInitialTransform = initialTransform;
            OnAICompleteEvent += callback;
        }
    }
    public Card GenerateKnownCard(CardInfo cardInfo, Transform initialTransform, bool pickable)
    {
        var card = Instantiate(cardPrefab);
        card.Initialize(cardInfo, initialTransform, pickable);
        return card;
    }

    private void Awake()
    {
        waitingForResponse = false;
    }
    private void OnAIReturnToken(string text)
    {
        result = text;
    }
    private void OnAIComplete()
    {
        waitingForResponse = false;
        var json = Utils.ExtractJSONString(result);

        var cardInfo = JsonUtility.FromJson<CardInfo>(json);
        if (cardInfo.Equals(default(CardInfo)))
        {
            Debug.LogWarning("From Json Failed");
        }
        var card = Instantiate(cardPrefab);
        card.Initialize(cardInfo, cardInitialTransform == null ? cardDefaultTransform : cardInitialTransform);
        OnAICompleteEvent.Invoke(card);
        OnAICompleteEvent = null;
    }


}
