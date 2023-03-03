using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TutorialPanel : UIPanel
{
    public Image TutorialPopUP;
    public Image TutorialPopUPArrow;
    public GameObject TempPos;
    public GameObject TempTargetPos;
    public TMP_Text PopUpText;
    public override void Initialize(UIManager uIManager)
    {
        base.Initialize(uIManager);
    }

    private void Update()
    {
        if (CanvasGroup.interactable)
        {
            if (Input.GetMouseButtonDown(0))
            {
                this.ClosePanel();
            }
        }
    }
}
