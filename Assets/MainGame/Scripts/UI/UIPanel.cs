using System;
using System.Collections.Generic;
using UnityEngine;

public class UIPanel : CustomBehaviour
{
    public UIManager UIManager { get; set; }

    public virtual void Initialize(UIManager uIManager)
    {
        UIManager = uIManager;
        GameManager = UIManager.GameManager;
    }

    public virtual void OpenPanel()
    {
        if (!gameObject.activeInHierarchy)
        {
            gameObject.SetActive(true);
        }

        CanvasGroup.Open();
        SetThisPanelAsCurrent();
    }

    public virtual void ClosePanel()
    {
        CanvasGroup.Close();
    }

    public virtual void SetThisPanelAsCurrent()
    {
        UIManager.SetCurrentUIPanel(this);
    }

    public virtual void UpdateUIElements()
    {

    }
}
