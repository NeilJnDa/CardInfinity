using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class PhysicalButton : MonoBehaviour, IPointerClickHandler, IPointerUpHandler, IPointerDownHandler
{
    [SerializeField]
    private Transform movable;
    [SerializeField]
    private float moveDistanceY = 0.2f;
    private Vector3 initMovableLocalPosition;
    public UnityEvent OnButtonUp = new UnityEvent();

    [SerializeField]
    [ReadOnly]
    private bool pressing = false;

    private void Awake()
    {
        pressing = false;
    }


    public void OnPointerClick(PointerEventData eventData)
    {
        OnButtonUp.Invoke();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        pressing = true;
        initMovableLocalPosition = movable.localPosition;
        movable.localPosition = initMovableLocalPosition + new Vector3(0, moveDistanceY, 0);
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        pressing = false;
        movable.localPosition = initMovableLocalPosition;      
    }
}
