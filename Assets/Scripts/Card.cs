using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using MyBox;
using UnityEditor;

public class Card : MonoBehaviour
{
    [SerializeField]
    [ReadOnly]
    private bool pickedUp = false;
    [SerializeField]
    [ReadOnly]
    private CardInfo cardInfo;
    [SerializeField]
    [ReadOnly]
    private CardSlot currentSlot = null;
    [SerializeField]
    [ReadOnly]
    private Camera mainCamera;

    [SerializeField]
    private TMP_Text nameText;
    [SerializeField]
    private TMP_Text hpText;
    [SerializeField]
    private TMP_Text descriptionText;

    private void Start()
    {
        mainCamera = Camera.main;
    }
    public void Initialize(CardInfo cardInfo)
    {
        if (cardInfo != null)
        {
            this.cardInfo = cardInfo;
            nameText.text = cardInfo.name;
            hpText.text = cardInfo.health.ToString();
            descriptionText.text = cardInfo.description;
        }

    }
    private void Update()
    {
        if (pickedUp)
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            Debug.DrawRay(ray.origin, ray.direction * 10f);
            float enter = 0.0f;
            var res = GameManager.Instance.CardHoverPlane.Raycast(ray, out enter);
            Vector3 hitPoint = ray.GetPoint(enter);

            this.transform.position = hitPoint;
        }
    }
    public void EnterSlot(CardSlot slot)
    {
        slot.CardEnter();
    }
    public void ExitSlot()
    {

    }
    public void PickUp()
    {
        pickedUp = true;
    }
    public void Drop()
    {
        pickedUp = false;
    }

}
