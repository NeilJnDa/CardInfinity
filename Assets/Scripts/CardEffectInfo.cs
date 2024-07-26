using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct CardEffectInfo
{
    public int index;   // -1 = not initialized
    public string reason;

    public CardEffectInfo(int index, string reason)
    {
        this.index = index;  
        this.reason = reason;
    }
    public string ToJson()
    {
        return JsonUtility.ToJson(this);
    }
    public static CardEffectInfo EmptyCardEffectInfo()
    {
        return new CardEffectInfo(-1, " ");
    }
}

[Serializable]
public struct CardEffectInfos
{
    public CardEffectInfo[] data;
}
