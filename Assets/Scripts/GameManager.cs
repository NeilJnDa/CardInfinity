using MyBox;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using Cinemachine;

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

    [Header("Camera")]
    [SerializeField]
    private Cinemachine.CinemachineVirtualCamera overviewwVCamera;
    [SerializeField]
    private Cinemachine.CinemachineVirtualCamera cardPlayVCamera;

    [Header("Others")]
    [SerializeField]
    private Transform cardHoverPlaneTransform;
    [SerializeField]
    private SlotGrid slotGrid;

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

    }

    #region Round
    public void StartRound(int numEmptyCards = 0){
        StartCoroutine(StartRoundCoroutine(numEmptyCards));

    }
    IEnumerator StartRoundCoroutine(int numEmptyCards = 0){
        for(int i = 0; i < 3 - numEmptyCards; i++){
            Deck.Instance.DrawCard();
            yield return new WaitForSeconds(0.5f);
        }

        for(int i = 0; i < numEmptyCards; i++){
            var card = LLMManager.Instance.CardGenerator.GenerateKnownCard(CardInfo.EmptyCardInfo(), Deck.Instance.transform, true);
            CardsInHand.Instance.AddCard(card);
            yield return new WaitForSeconds(0.5f);
        }
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
    }
    #endregion
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
    #region Game Start
    public void StartGame()
    {
        StartCoroutine(StartGameCoroutine());
    }
    IEnumerator StartGameCoroutine(){
        PointerManager.Instance.Interactable = false;
        SwitchToOverviewCamera();
        yield return new WaitForSeconds(0.5f);
        
        //Overview Do something

        SwitchToCardPlayCamera();
        yield return new WaitForSeconds(0.5f);

        var pos = new Vector2Int(1, 1);
        var card = LLMManager.Instance.CardGenerator.GenerateKnownCard(new CardInfo("Dragon", 20, "A ferocious dragon you must defeat."), slotGrid.GetSlot(pos)?.transform, false);
        slotGrid.GetSlot(pos)?.SetCardAtStart(card);

        yield return new WaitForSeconds(0.5f);

        pos = new Vector2Int(1, 0);
        card = LLMManager.Instance.CardGenerator.GenerateKnownCard(new CardInfo("Wind", 5, "The wind blown by the dragon's wings."), slotGrid.GetSlot(pos)?.transform, false);
        slotGrid.GetSlot(pos)?.SetCardAtStart(card);      
        yield return new WaitForSeconds(0.5f);

        pos = new Vector2Int(1, 2);
        card = LLMManager.Instance.CardGenerator.GenerateKnownCard(new CardInfo("Wind", 5, "The wind blown by the dragon's wings."), slotGrid.GetSlot(pos)?.transform, false);
        slotGrid.GetSlot(pos)?.SetCardAtStart(card);
        yield return new WaitForSeconds(0.5f);

        PointerManager.Instance.Interactable = true;

        StartRound();
    }
    #endregion
    #region Camera
    public void SwitchToOverviewCamera()
    {
        overviewwVCamera.Priority = 10;
        cardPlayVCamera.Priority = 0;
    }
    public void SwitchToCardPlayCamera()
    {
        overviewwVCamera.Priority = 0;
        cardPlayVCamera.Priority = 10;
    }
    #endregion
}
