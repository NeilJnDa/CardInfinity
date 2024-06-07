using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using MyBox;

public class Card : MonoBehaviour
{
    [SerializeField]
    [ReadOnly]
    private CardInfo cardInfo;

    [SerializeField]
    private TMP_Text nameText;
    [SerializeField]
    private TMP_Text hpText;
    [SerializeField]
    private TMP_Text descriptionText;

    public void Initialize(CardInfo cardInfo)
    {
        this.cardInfo = cardInfo;
        nameText.text = cardInfo.name;
        hpText.text = cardInfo.health.ToString();
        descriptionText.text = cardInfo.description;
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
