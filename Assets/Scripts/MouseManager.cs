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
    [ReadOnly]
    private Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main;
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var card = CastRayFromMouse();
            if (card != null)
            {
                holdingCard = card;
                holdingCard.PickUp();
            }
        }

        if (Input.GetMouseButtonUp(0) && holdingCard != null)
        {
            holdingCard.Drop();
        }
    }
    Card CastRayFromMouse()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        Debug.DrawRay(ray.origin, ray.direction * 10f, Color.red, Time.deltaTime);
        if (Physics.Raycast(ray, out hit, 10f))
        {
            Debug.Log("Hit " + hit.collider.name);
            var card = hit.collider.GetComponent<Card>();
            if (card != null)
            {
                return card;
            }
        }
        return null;
    }
}
