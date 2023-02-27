using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayAnimation : MonoBehaviour
{
    public List<Sprite> Images;
    public Image BaseImage;
    public float Time;
    private Coroutine coroutineLoop;
    public void Play()
    {
        StartCoroutine(Delay());
    }

    private IEnumerator Delay()
    {
        for (int i = 0; i < Images.Count; i++)
        {
            yield return new WaitForSeconds(Time);
            BaseImage.sprite = Images[i];
        }
    }

    public void PlayLoop()
    {
        coroutineLoop = StartCoroutine(DelayLoop());
    }

    public IEnumerator DelayLoop()
    {
        for (int i = 0; i < Images.Count; i++)
        {
            yield return new WaitForSecondsRealtime(Time);
            BaseImage.sprite = Images[i];
        }
        StartCoroutine(DelayLoop());
    }

    public void StopLoop()
    {
        if(coroutineLoop != null)
        {
            StopCoroutine(coroutineLoop);
        }
    }
}
