using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;

public class PointerManager : MonoBehaviour
{
    #region Singleton
    public static PointerManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<PointerManager>();
            }
            return _instance;
        }
    }
    private static PointerManager _instance;
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

    // Pick up cards and drop to a slot
    [SerializeField]
    [ReadOnly]
    private Card holdingCard;

    [field: SerializeField]
    public CardSlot HoveringSlot { get; private set; }

    [SerializeField]
    private LayerMask slotLayer;
    [field: SerializeField]
    public Card HoveringCard { get; private set; }
    [SerializeField]

    private LayerMask cardLayer;

    //  Local cache
    private Camera mainCamera;

    public void Start()
    {
        mainCamera = Camera.main;
    }

    public void Update()
    {
        //  Update the slot just under the pointer. Any script can access that.
        var target = CastRayFromMouse(slotLayer);
        if (HoveringSlot && target && HoveringSlot.GetInstanceID() == target.GetInstanceID()) return;

        if (target == null)
            HoveringSlot = null;
        else
            HoveringSlot = target.GetComponent<CardSlot>();


        //  Update the card just under the pointer. Any script can access that.
        target = CastRayFromMouse(cardLayer);
        if (HoveringCard && target && HoveringCard.GetInstanceID() == target.GetInstanceID()) return;

        if (target == null)
            HoveringCard = null;
        else
        {
            HoveringCard = target.GetComponent<Card>();
            HoveringCard.HoverAndFloat();
        }
    }
    Transform CastRayFromMouse(LayerMask layers)
    {
        Ray ray = CameraRay();
        RaycastHit hit;
        Debug.DrawRay(ray.origin, ray.direction * 10f, Color.red, Time.deltaTime);
        if (Physics.Raycast(ray, out hit, 10f, layers))
        {
            //Debug.Log("Hit " + hit.collider.name);
            return hit.collider.transform;
        }
        return null;
    }
    public Ray CameraRay()
    {
        return mainCamera.ScreenPointToRay(Input.mousePosition);
    }

    public void HoldCard(Card card)
    {
        this.holdingCard = card;
    }
    public void DropCard()
    {
        this.holdingCard = null;
    }
}
