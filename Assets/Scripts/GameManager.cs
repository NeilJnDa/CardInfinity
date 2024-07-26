using MyBox;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    #region Singleton
    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<GameManager>();
            }
            return _instance;
        }
    }
    private static GameManager _instance;
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

    [SerializeField]
    private Transform cardHoverPlaneTransform;
    [SerializeField]
    private SlotGrid slotGrid;

    [SerializeField]
    [ReadOnly]
    private int actionPoint = 0;
    [SerializeField]
    private int actionPointPerRound = 5;
    [SerializeField]
    private TMP_Text actionPointText;
    public Plane CardHoverPlane
    {
        get
        {
            return new Plane(cardHoverPlaneTransform.up, cardHoverPlaneTransform.position);
        }
    }
    private void Start()
    {
        StartGame();
        actionPoint = actionPointPerRound;
        actionPointText.text = actionPoint.ToString();

    }
    public void EndRound()
    {
        StartCoroutine(EndRoundExecutor());
    }
    private bool completed = false;
    private IEnumerator EndRoundExecutor()
    {

        var playerSlots = slotGrid.PlayerSlots();
        foreach (var slot in playerSlots)
        {
            if (slot.slot.card != null)
            {
                var targetSlot = slotGrid.GetSlot(slot.position + new Vector2Int(1, 0));
                if (targetSlot != null && targetSlot.card != null)
                {
                    Debug.Log(slot.position + ": " + slot.slot.card.CardInfo.name + " attacks " + targetSlot.card.CardInfo.name);

                    completed = false;
                    if (LLMManager.Instance.CardCombatJudge.GenerateCardCombatInfo(slot.slot.card.CardInfo, targetSlot.card.CardInfo, OnCombatJudgeComplete))
                    {
                        yield return new WaitUntil(() => completed == true);
                    }
                    else
                    {
                        Debug.LogError("CardCombatJudge returns false");
                    }
                }
            }
        }
        actionPoint = actionPointPerRound;
        actionPointText.text = actionPoint.ToString();
    }
    private void OnCombatJudgeComplete(CardEffectInfos result)
    {
        foreach(var effect in result.data)
        {
            Debug.Log(effect.ToJson());
        }
        //TODO: Invoke Effects
        completed = true;
    }
    public void ResetAll()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void StartGame()
    {
        var pos = new Vector2Int(1, 1);
        var card = LLMManager.Instance.CardGenerator.GenerateKnownCard(new CardInfo("Dragon", 20, "A ferocious dragon you must defeat."), slotGrid.GetSlot(pos)?.transform, false);
        slotGrid.GetSlot(pos)?.SetCardAtStart(card);

        pos = new Vector2Int(1, 2);
        card = LLMManager.Instance.CardGenerator.GenerateKnownCard(new CardInfo("Wind", 5, "The wind blown by the dragon's wings."), slotGrid.GetSlot(pos)?.transform, false);
        slotGrid.GetSlot(pos)?.SetCardAtStart(card);

    }
    public bool HasEnoughActionPoint(int leastPoint)
    {
        return actionPoint >= leastPoint;
    }
    public bool TryConsumeActionPoint(int point)
    {
        if (actionPoint - point < 0)
            return false;
        else
        {
            actionPoint -= point;
            actionPointText.text = actionPoint.ToString();
            return true;
        }
    }
}
