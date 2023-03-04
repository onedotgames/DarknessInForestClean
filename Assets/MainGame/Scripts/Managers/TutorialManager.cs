using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class TutorialManager : CustomBehaviour
{
    public TutorialPanel TutorialPanel;
    private bool equipmentTutorialShowedUp = false;
    private bool levelTutorialShowUp = false;
    public override void Initialize(GameManager gameManager)
    {
        base.Initialize(gameManager);
    }

    public void SetTutorialPopUpPosition(Vector3 target)
    {
        TutorialPanel.TutorialPopUP.transform.position = target;
        TutorialPanel.TutorialPopUP.gameObject.SetActive(true);
    }

    public void FireRay(Vector3 from,Vector3 to) 
    {
        var direction = (to - from).normalized;
        RaycastHit2D raycastHit2D = Physics2D.Raycast(from, direction,Mathf.Infinity);
        if(raycastHit2D.collider != null)
        {
            Debug.Log(raycastHit2D.collider.name);
            SetArrowPosition(raycastHit2D.point);
            
            SetArrowRot(from, to);
            Debug.Log(raycastHit2D.point);
        }
    }

    public void SetArrowPosition(Vector3 target)
    {
        TutorialPanel.TutorialPopUPArrow.transform.position = target;
    }
    public void SetArrowRot(Vector3 from, Vector3 to)
    {
        TutorialPanel.TutorialPopUPArrow.transform.eulerAngles = new Vector3(0, 0, Vector3.Angle(from, to) - 60);
    }

    public void SetTutorialText(string str)
    {
        TutorialPanel.PopUpText.SetText(str);
    }


    public void EquipmentTutorialOn()
    {
        if (!equipmentTutorialShowedUp)
        {
            equipmentTutorialShowedUp = true;
            TutorialPanel.TutorialPopUP.enabled = true;
            TutorialPanel.PopUpText.enabled = true;
            TutorialPanel.TutorialPopUPArrow.GetComponent<TutorialArrow>().MaxScale = 1.75f;
            TutorialPanel.TutorialPopUPArrow.GetComponent<TutorialArrow>().MinScale = 1.25f;
            TutorialPanel.TutorialPopUPArrow.transform.localScale = Vector3.one * TutorialPanel.TutorialPopUPArrow.GetComponent<TutorialArrow>().MinScale;
            TutorialPanel.TutorialPopUPArrow.GetComponent<TutorialArrow>().RestartSequence();


            TutorialPanel.OpenPanel();
            SetTutorialPopUpPosition(GameManager.TutorialManager.TutorialPanel.TempPos.transform.position);
            SetTutorialText("Click to icon to change Equipment!");
            FireRay(GameManager.TutorialManager.TutorialPanel.TutorialPopUP.transform.position,
            TutorialPanel.TempTargetPos.transform.position);
        }
        
    }

    public void LevelsTutorialOn()
    {
        if (!levelTutorialShowUp)
        {
            levelTutorialShowUp = true;
            TutorialPanel.TutorialPopUP.enabled = false;
            TutorialPanel.PopUpText.enabled = false;
            TutorialPanel.TutorialPopUPArrow.GetComponent<TutorialArrow>().MaxScale = 3f;
            TutorialPanel.TutorialPopUPArrow.GetComponent<TutorialArrow>().MinScale = 2.5f;
            TutorialPanel.TutorialPopUPArrow.transform.localScale = Vector3.one * TutorialPanel.TutorialPopUPArrow.GetComponent<TutorialArrow>().MinScale;
            TutorialPanel.TutorialPopUPArrow.GetComponent<TutorialArrow>().RestartSequence();
            TutorialPanel.TutorialPopUPArrow.transform.position = new Vector3(0,
                TutorialPanel.TempTutorialPopUPArrow.transform.position.y,
                0) ;
            TutorialPanel.TutorialPopUPArrow.transform.eulerAngles = Vector3.zero;
            TutorialPanel.OpenPanel();
            //SetTutorialPopUpPosition(GameManager.TutorialManager.TutorialPanel.TempLevelsPos.transform.position);

            ////SetTutorialText("Click to icon to change Equipment!");
            //FireRay(GameManager.TutorialManager.TutorialPanel.TempLevelsPos.transform.position,
            //Vector3.down);
        }

    }

}
