using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct CardInfo
{
    public string name;
    public int health;
    public string description;

    public CardInfo(string name, int health, string description)
    {
        this.name = name;  
        this.health = health;
        this.description = description;
    }
    public string ToJson()
    {
        return JsonUtility.ToJson(this);
    }
    public static CardInfo EmptyCardInfo()
    {
        return new CardInfo("___", 0, "");
    }
}
