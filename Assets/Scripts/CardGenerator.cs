using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LLMUnity;
using MyBox;

[ExecuteInEditMode]
public class CardGenerator : MonoBehaviour
{
    [SerializeField]
    [ReadOnly]
    private bool waitingForResponse = false;

    public LLMClient llm;
    [TextArea(3,5)]
    public string command = "";

    public string result = "";
    public string result2 = "";
    [ButtonMethod]
    private void GenerateRequest()
    {
        _ = llm.Chat(command, OnAIReturnToken, OnAIComplete);
        waitingForResponse = true;
    }
    [ButtonMethod]
    private void GenerateRequest2()
    {
        _ = llm.Chat(command, (text)=>result2 = text, OnAIComplete);
        waitingForResponse = true;
    }

    private void OnAIReturnToken(string text)
    {
        result = text;
    }
    private void OnAIComplete()
    {
        waitingForResponse = false;
    }

   
}
