using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LLMUnity;
using MyBox;

[ExecuteInEditMode]
public class CardGenerator : MonoBehaviour
{
    [SerializeField]
    private Card cardPrefab;
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
        result = result.Substring(result.IndexOf('{'), result.LastIndexOf('}') - result.IndexOf('{') + 1);

        var cardInfo = JsonUtility.FromJson<CardInfo>(result);
        if (cardInfo == null)
        {
            Debug.LogWarning("From Json Failed");
        }
        var card = Instantiate(cardPrefab);
        card.Initialize(cardInfo);
    }


}
