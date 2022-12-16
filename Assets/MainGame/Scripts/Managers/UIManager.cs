using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : CustomBehaviour
{
    public UIPanel CurrentPanel { get; set; }
    public List<UIPanel> UIPanels;
    public override void Initialize(GameManager gameManager)
    {
        base.Initialize(gameManager);

        InitializeOptions();
    }

    private void InitializeOptions()
    {
        UIPanels.ForEach(x => { x.Initialize(this); x.gameObject.SetActive(false); });

        GetPanel(Panels.Initial).OpenPanel();
    }

    public UIPanel GetPanel(Panels panel)
    {
        return UIPanels[(int)panel];
    }

    public void SetCurrentUIPanel(UIPanel uiPanel)
    {
        CurrentPanel = uiPanel;
    }
}
