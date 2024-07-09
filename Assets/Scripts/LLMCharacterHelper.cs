using LLMUnity;
using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(LLMCharacter))]
[ExecuteInEditMode]
public class LLMCharacterHelper : MonoBehaviour
{
    public LLMCharacter llmCharacter
    {
        get
        {
            if(m_llmCharacter == null)
                m_llmCharacter = GetComponent<LLMCharacter>();
            return m_llmCharacter;
        }
    }
    private LLMCharacter m_llmCharacter;

    public List<ChatMessage> GetChat()
    {
        return llmCharacter.chat;
    }

    [ButtonMethod]
    private void DebugChatHistory()
    {
        var chat = GetChat();
        Debug.Log(llmCharacter.AIName + " chat history:");
        foreach (var chatMessage in chat)
        {
            Debug.Log(chatMessage.role + ": " + chatMessage.content);
        }
    }

}
