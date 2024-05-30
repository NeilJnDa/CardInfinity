using LLMUnity;
using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(LLMClient))]
[ExecuteInEditMode]
public class LLMClientHelper : MonoBehaviour
{
    public LLMClient llmClient
    {
        get
        {
            if(m_llmClient == null)
                m_llmClient = GetComponent<LLMClient>();
            return m_llmClient;
        }
    }
    private LLMClient m_llmClient;

    public List<ChatMessage> GetChat()
    {
        return llmClient.chat;
    }

    [ButtonMethod]
    private void DebugChatHistory()
    {
        var chat = GetChat();
        Debug.Log(llmClient.AIName + " chat history:");
        foreach (var chatMessage in chat)
        {
            Debug.Log(chatMessage.role + ": " + chatMessage.content);
        }
    }

    private void Update()
    {
    }
}
