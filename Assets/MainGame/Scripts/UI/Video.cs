using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class Video : UIPanel
{
    public VideoPlayer VideoPlayer;
    public override void Initialize(UIManager uIManager)
    {
        base.Initialize(uIManager);
        VideoPlayer.loopPointReached += EndReached;
    }
   

    void EndReached(UnityEngine.Video.VideoPlayer vp)
    {
        GameManager.UIManager.GetPanel(Panels.MainMenu).OpenPanel();
        vp.enabled = false;
        this.ClosePanel();
    }
}
