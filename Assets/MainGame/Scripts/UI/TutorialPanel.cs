using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TutorialPanel : UIPanel
{
    public Image TutorialPopUP;
    public Image TutorialPopUPArrow;
    public Image TempTutorialPopUPArrow;
    public GameObject TempPos;
    public GameObject TempLevelsPos;
    public GameObject TempTargetPos;
    public GameObject TempLevelsTargetPos;
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
