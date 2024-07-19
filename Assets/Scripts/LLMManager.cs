using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LLMManager : MonoBehaviour
{
    #region Singleton
    public static LLMManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<LLMManager>();
            }
            return _instance;
        }
    }
    private static LLMManager _instance;
    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    #endregion

    [field:SerializeField]
    public CardMerger CardMerger { get; private set; }

    [field: SerializeField]
    public CardGenerator CardGenerator { get; private set; }

    [field: SerializeField]
    public CardCombatJudge CardCombatJudge { get; private set; }
}
