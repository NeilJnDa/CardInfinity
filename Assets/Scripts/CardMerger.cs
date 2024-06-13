using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LLMUnity;
using MyBox;

[ExecuteInEditMode]
public class CardMerger : MonoBehaviour
{
    [SerializeField]
    private CardInfo card1;

    [SerializeField]
    private CardInfo card2;

    [SerializeField]
    [ReadOnly]
    private bool waitingForResponse = false;

    public LLMClient llm;
    [TextArea(3, 5)]
    public string command = "";

    [SerializeField]
    [TextArea(3, 5)]
    private string result = "";
    [ButtonMethod]
    private void GenerateRequest()
    {
        _ = llm.Chat(command, OnAIReturnToken, OnAIComplete, false);
        waitingForResponse = true;
    }

    private void OnAIReturnToken(string text)
    {
        result = text;
    }
    private void OnAIComplete()
    {
        waitingForResponse = false;
        result = Utils.ExtractJSONString(result);
    }
}
