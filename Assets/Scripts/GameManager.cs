using MyBox;
using System.Collections;
using System.Collections.Generic;
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
        actionPoint = actionPointPerRound;
    }
    public void ResetAll()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void StartGame()
    {
        var pos = new Vector2Int(1, 1);
        var card = LLMManager.Instance.CardGenerator.GenerateKnownCard(new CardInfo("Dragon", 20, "A ferocious dragon you must defeat."), slotGrid.GetSlot(pos)?.transform, false);
        slotGrid.GetSlot(pos)?.TryPlaceCard(card);
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
