using LLMUnity;
using MyBox;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardCombatJudge : MonoBehaviour
{
    #region Debugging in inspector
    [SerializeField]
    CardInfo cardAttacking;
    [SerializeField]
    CardInfo cardAttcked;

    [ButtonMethod]
    private void GenerateRequest()
    {
        result = "";
        string completeCommand = command + "Card1: " + cardAttacking.ToJson() + "Card2: " + cardAttcked.ToJson();
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
    }
    #endregion


#pragma warning disable 0414
    [SerializeField]
    [ReadOnly]
    private bool waitingForResponse = false;
#pragma warning restore 0414

    public LLMCharacter llmCharacter;
    [TextArea(3, 5)]
    public string command = "";

    [SerializeField]
    [TextArea(3, 5)]
    private string result = "";

    private Action<string> OnAICompleteEvent;
    public bool GenerateCardCombatInfo(CardInfo cardAttacking, CardInfo cardAttacked, Action<string> callback)
    {
        if (waitingForResponse)
        {
            Debug.LogWarning(this.name + " is waiting for response, submit request later");
            return false;
        }

        string completeCommand = "Card1: " + cardAttacking.ToJson() + "Card2: " + cardAttacked.ToJson() + command;
        Debug.Log(completeCommand);
        _ = llmCharacter.Chat(completeCommand, OnAIReturnToken, OnCombatAIComplete, false);
        waitingForResponse = true;
        OnAICompleteEvent += callback;
        return true;
    }

    private void OnCombatAIComplete()
    {
        waitingForResponse = false;
        var json = Utils.ExtractJSONString(result);
        OnAICompleteEvent.Invoke(result);
        OnAICompleteEvent = null;
    }

}
