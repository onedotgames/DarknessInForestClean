using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayAnimation : MonoBehaviour
{
    public List<Sprite> Images;
    public Image BaseImage;

    public void Play()
    {
        StartCoroutine(Delay());
    }

    private IEnumerator Delay()
    {
        for (int i = 0; i < Images.Count; i++)
        {
            yield return new WaitForSeconds(0.01f);
            BaseImage.sprite = Images[i];
        }
        
    }
}
