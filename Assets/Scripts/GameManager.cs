using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
    public void EndRound()
    {


    }

    public void StartGame()
    {
        var pos = new Vector2Int(1, 1);
        var card = LLMManager.Instance.CardGenerator.GenerateKnownCard(new CardInfo("Dragon", 20, "A ferocious dragon you must defeat."), slotGrid.GetSlot(pos)?.transform, false);
        slotGrid.GetSlot(pos)?.CardPlaced(card);
    }
    public void AddCard(Vector2Int position, CardInfo cardInfo)
    {

    } 
}
