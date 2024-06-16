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

    public string ToJson()
    {
        return JsonUtility.ToJson(this);
    }
}
