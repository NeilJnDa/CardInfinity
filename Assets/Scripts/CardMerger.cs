using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LLMUnity;
using MyBox;

[ExecuteInEditMode]
public class CardMerger : MonoBehaviour
{
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

    [SerializeField]
    [ReadOnly]
    private bool waitingForResponse = false;

    public LLMCharacter llmCharacter;
    [TextArea(3, 5)]
    public string command = "";

    [SerializeField]
    [TextArea(3, 5)]
    private string result = "";
    [ButtonMethod]
    private void GenerateRequest()
    {
        outputCard = null;
        result = "";
        card1 = slot1.card;
        card2 = slot2.card;
        string completeCommand = command + card1.CardInfo.ToJson() + card2.CardInfo.ToJson();
        Debug.Log(completeCommand);
        _ = llmCharacter.Chat(completeCommand, OnAIReturnToken, OnAIComplete, false);
        waitingForResponse = true;
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
        var card = Instantiate(cardPrefab, outputSlot.transform.position, cardPrefab.transform.rotation);
        card.Initialize(cardInfo);
        outputCard = card;
    }
}
