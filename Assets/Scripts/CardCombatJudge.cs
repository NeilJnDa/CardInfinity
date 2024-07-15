using LLMUnity;
using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardCombatJudge : MonoBehaviour
{
    [SerializeField]
    private CardSlot slot1;
    [SerializeField]
    private CardSlot slot2;

    [SerializeField]
    [ReadOnly]
    private Card card1;

    [SerializeField]
    [ReadOnly]
    private Card card2;

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
        result = "";
        card1 = slot1.card;
        card2 = slot2.card;
        string completeCommand = command + "Card1: " + card1.CardInfo.ToJson() + "Card2: " + card2.CardInfo.ToJson();
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
    }
}
