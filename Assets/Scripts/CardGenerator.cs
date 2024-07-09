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
    private Transform cardGenerateTransform;
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
    public void GenerateRequest()
    {
        if (!waitingForResponse)
        {
            _ = llmCharacter.Chat(command, OnAIReturnToken, OnAIComplete, false);
            waitingForResponse = true;
        }
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
        card.transform.position = cardGenerateTransform.position;
        card.transform.rotation = cardGenerateTransform.rotation;
        card.Initialize(cardInfo);
    }


}
