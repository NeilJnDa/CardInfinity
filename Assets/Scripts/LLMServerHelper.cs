using LLMUnity;
using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(LLM))]
[ExecuteInEditMode]
public class LLMServerHelper : MonoBehaviour
{
 public LLM llm
    {
        get
        {
            if(m_llm == null)
                m_llm = GetComponent<LLM>();
            return m_llm;
        }
    }
    private LLM m_llm;


    [SerializeField]
    [ReadOnly]
    private bool serverRunning;

    [ButtonMethod]
    public void StartServer()
    {
        if (!llm.serverStarted)
        {
            llm.Awake();
        }
    }
    [ButtonMethod]

    public void EndServer()
    {
        if (llm.serverStarted)
        {
            llm.StopProcess();
        }
    }
    private void Update()
    {
        serverRunning = llm.serverStarted;
    }
}
