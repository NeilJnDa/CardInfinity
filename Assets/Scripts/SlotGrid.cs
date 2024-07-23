using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

#if UNITY_EDITOR
using UnityEditor;
#endif

[Serializable]
public struct SlotItem
{
    public Vector2Int position;
    public CardSlot slot;

    public SlotItem (Vector2Int position, CardSlot slot)
    {
        this.position = position;
        this.slot = slot;
    }
}
public class SlotGrid : MonoBehaviour
{
    [SerializeField]
    private GameObject slotPrefab;
    [SerializeField]
    private float horizontalSpacing = 2f;
    [SerializeField]
    private float verticalSpacing = 2f;
    [SerializeField]
    private int numRows = 3;
    [SerializeField]
    private int numCols = 3;

    [SerializeField]
    private bool onlyFirstRowReceiveCards = true;

    [SerializeField]
    private List <SlotItem> slots = new List<SlotItem>();

#if UNITY_EDITOR
    [ButtonMethod]
    void ResetGridEditor()
    {
        Undo.RecordObject(this, "Reset Grid");
        ResetGrid();
        PrefabUtility.RecordPrefabInstancePropertyModifications(this);
    }
#endif
    private void ResetGrid()
    {
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            if (Application.isPlaying)
                Destroy(transform.GetChild(i).gameObject);
            else
                DestroyImmediate(transform.GetChild(i).gameObject);
        }
        slots = new List<SlotItem>();
        for (int i = 0; i < numRows; i++)
        {
            for (int j = 0; j < numCols; j++)
            {
                var go = Instantiate(slotPrefab, this.transform);
                go.transform.position = this.transform.position + new Vector3(j * horizontalSpacing, 0, i * verticalSpacing);
                var cardSlot = go.GetComponent<CardSlot>();
                if (onlyFirstRowReceiveCards)
                {
                    cardSlot.Initialize(i == 0);
                }
                slots.Add(new SlotItem(new Vector2Int(i, j), go.GetComponent<CardSlot>()));            
            }
        }
    }
    public CardSlot GetSlot(Vector2Int position)
    {
        if(slots == null)
        {
            Debug.LogError("Slots in SlotGrid is not initialized.");
            return null;
        }
        var slotItem = slots.Find(x => x.position == position);
        if(slotItem.Equals (default(SlotItem)))
        {
            Debug.LogError("Can not find " + position + " in Slots");
            return null;
        }
        return slotItem.slot;
    }
    public CardSlot GetSlot(int x, int y)
    {
        return GetSlot(new Vector2Int(x, y));
    }
    public List<SlotItem> PlayerSlots()
    {
        return slots.FindAll(x => x.position.x == 0).OrderBy(x=> x.position.y).ToList();
    }
    public List<SlotItem> EnemySlots()
    {
        return slots.FindAll(x => x.position.x == 1).OrderBy(x => x.position.y).ToList();
    }
}
