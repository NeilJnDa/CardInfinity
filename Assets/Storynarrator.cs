using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyBox;
using LLMUnity;
using System;

public class Storynarrator : MonoBehaviour
{
    [SerializeField]
    [ReadOnly]
    private bool waitingForResponse = false;

    [ButtonMethod]
    private void GenerateDebugRequest()
    {
        GenerateStory(debugCardInfo, null);
    }
    [SerializeField]
    private CardInfo debugCardInfo;
    
    public LLMCharacter llmCharacter;
    [TextArea(3, 5)]
    public string extraCommand = "";

    [SerializeField]
    [TextArea(3, 5)]
    private string result = "";

    private Action<string> OnAICompleteEvent;

    private void OnAIReturnToken(string text)
    {
        result = text;
    }

    private void OnAIComplete()
    {
        waitingForResponse = false;
        OnAICompleteEvent.Invoke(result);
        OnAICompleteEvent = null;
    }

    public bool GenerateStory(CardInfo cardToAppear, Action<string> callback)
    {
        if (waitingForResponse)
        {
            Debug.LogWarning(this.name + " is waiting for response, submit request later");
            return false;
        }
        string completeCommand = cardToAppear.ToJson() + extraCommand;
        Debug.Log(completeCommand);
        _ = llmCharacter.Chat(completeCommand, OnAIReturnToken, OnAIComplete, true);
        waitingForResponse = true;
        OnAICompleteEvent += callback;
        return true;
    }
}
