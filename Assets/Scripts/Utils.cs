using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Utils

{
    public static string ExtractJSONString(string source)
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
}
