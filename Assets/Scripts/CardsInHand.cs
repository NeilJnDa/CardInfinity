using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Linq;

public class CardsInHand : MonoBehaviour
{
    #region Singleton
    public static CardsInHand Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<CardsInHand>();
            }
            return _instance;
        }
    }
    private static CardsInHand _instance;
    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(this.transform.root.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    #endregion

    [SerializeField]
    private float totalWidth = 5f;
    [SerializeField]
    private float depthSpacing = 0.01f;

    public List<Card> Cards { get { return cards; } }
    [SerializeField]
    private List<Card> cards = new List<Card>();

    private void Start()
    {
        if (cards.Any())
        {
            cards.ForEach(x=>x.Initialize(x.CardInfo, x.transform, true));
            OrganizeCardAnim();

        }
    }
    public void AddCard(Card card)
    {
        if(card != null)
        {
            cards.Add(card);
            card.transform.parent = this.transform;
        }
        OrganizeCardAnim();

    }
    public void RemoveCard(Card card)
    {
        if(card != null && cards.Contains(card))
            cards.Remove(card);
        OrganizeCardAnim();

    }
    private void Update()
    {
    }

    [ButtonMethod]
    public void OrganizeCardAnim()
    {
        Vector3 startingPosition = this.transform.position - new Vector3(totalWidth / 2f , 0, 0); 
        float spacing = totalWidth / cards.Count;
        for(int i = 0; i < cards.Count; i++)
        {   
            cards[i].transform.DOMove(startingPosition + new Vector3(i * spacing, 0, 0) + this.transform.forward * i * depthSpacing, 0.3f);
            cards[i].transform.DORotateQuaternion(this.transform.rotation, 0.3f);
        }
    }
}
