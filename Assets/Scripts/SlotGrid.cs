using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

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

    public Dictionary<Vector2Int, CardSlot> Slots { get; private set; }

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

        Slots = new Dictionary<Vector2Int, CardSlot>();

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
                Slots.Add(new Vector2Int(i,j), go.GetComponent<CardSlot>());
                
            }
        }
    }


}
