using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Utils

{
    private static string ExtractJSONString(string source)
    {
        int start = source.IndexOf('{');
        int end = source.LastIndexOf('}');
        if (start < end && start != -1 && end != -1)
        {
            return source.Substring(start, end - start + 1);
        }
        else
            return "{}";
    }
    public static T FromJson<T>(string source)
    {
        try
        {
            var json = Utils.ExtractJSONString(source);
            var result = JsonUtility.FromJson<T>(json);
            if(result.Equals(default(T)))
            {
                Debug.LogWarning("From Json Failed: A default value is returned. " + "Original String: " + source + " ExtractJSONString: " + json);
            }
            return result;
        }
        catch (Exception e)
        {
            var json = Utils.ExtractJSONString(source);
            Debug.LogWarning("From Json Failed: " + e.Message + "Original String: " + source + " ExtractJSONString: " + json);
            return default(T);
        }

    }
}
