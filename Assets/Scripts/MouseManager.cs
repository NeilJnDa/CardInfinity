using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseManager : MonoBehaviour
{
    [SerializeField]
    [ReadOnly]
    private Card holdingCard = null;

    [SerializeField]
    private LayerMask pickableLayers;
    [SerializeField]
    private LayerMask slotLayers;

    [Foldout("Local Cache", true)]
    [SerializeField]
    [ReadOnly]
    private Camera mainCamera;
    [SerializeField]
    [ReadOnly]
    private CardSlot hoveringSlot = null;
    private void Start()
    {
        mainCamera = Camera.main;
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var card = CastRayFromMouse(pickableLayers);
            if (card != null)
            {
                holdingCard = card.GetComponent<Card>();
                holdingCard.PickUp();
            }
        }

        if (Input.GetMouseButton(0) && holdingCard != null)
        {
            var slotTransform = CastRayFromMouse(slotLayers);
            if (slotTransform != null)
            {
                //  Snap to slot
                //  Update the hoeverSlot
                if (hoveringSlot == null || slotTransform.name != hoveringSlot.name)
                    hoveringSlot = slotTransform.GetComponent<CardSlot>();

                holdingCard.SetMoveTarget(hoveringSlot.transform.position);
            }
            else
            {
                //  Otherwise follow mouse position;
                hoveringSlot = null;
                Ray ray = CameraRay();
                float enter = 0.0f;
                var res = GameManager.Instance.CardHoverPlane.Raycast(ray, out enter);
                Vector3 hitPoint = ray.GetPoint(enter);
                holdingCard.SetMoveTarget(hitPoint);
            }
        }

        if (Input.GetMouseButtonUp(0) && holdingCard != null)
        {
            holdingCard.Drop();
            hoveringSlot = null;
        }
    }
    Transform CastRayFromMouse(LayerMask layers)
    {
        Ray ray = CameraRay();
        RaycastHit hit;
        Debug.DrawRay(ray.origin, ray.direction * 10f, Color.red, Time.deltaTime);
        if (Physics.Raycast(ray, out hit, 10f, layers))
        {
            Debug.Log("Hit " + hit.collider.name);
            return hit.collider.transform;
        }
        return null;
    }
    Ray CameraRay()
    {
        return mainCamera.ScreenPointToRay(Input.mousePosition);
    }
}
