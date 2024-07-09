using LLMUnity;
using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(LLM))]
[ExecuteInEditMode]
public class LLMHelper : MonoBehaviour
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
    private bool serviceRunning;

    private void Update()
    {
        serviceRunning = llm.started;
    }
}
