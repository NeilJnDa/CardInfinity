using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DG.Tweening;

public class Deck : MonoBehaviour
{
    #region Singleton
    public static Deck Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<Deck>();
            }
            return _instance;
        }
    }
    private static Deck _instance;
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

    [field: SerializeField]
    public List<Card> cards { get; private set; } = new List<Card>();
    public void DrawCard()
    {
        if(cards.Any())
        {
            CardsInHand.Instance.AddCard(cards.First());
            cards.RemoveAt(0);
        }
    }
    public void Shuffle()
    {
        if (cards.Any())
        {
            cards = cards.OrderBy(x => Random.value).ToList();
        }
    }
    public void AddCard(Card card)
    {
        cards.Add(card);
        card.transform.parent = this.transform;
        card.transform.DOMove(transform.position, 0.5f);
    }
}
