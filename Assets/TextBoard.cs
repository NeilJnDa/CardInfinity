using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;

public class TextBoard : MonoBehaviour
{
    [SerializeField]
    private CanvasGroup canvasGroup;
    [SerializeField]
    private TMP_Text text;

    public void ShowTextBoard(string newText){
        text.text = newText;
        canvasGroup.blocksRaycasts = true;

        canvasGroup.DOFade(1f, 0.5f);

    }
    public void HideTextBoard(){
        canvasGroup.DOFade(0f, 0.5f);
        canvasGroup.blocksRaycasts = false;
    }
}
