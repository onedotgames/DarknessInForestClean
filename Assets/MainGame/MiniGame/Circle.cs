using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class Circle : MonoBehaviour
{
    public List<Image> Images = new List<Image>();
    public MiniGameBase MiniGame;
    public GameObject ImageHolder;
    public bool isTrueImage = false;
    private float startDeg = -9;
    private int i = 0;

    public void RotateCircle()
    {
        startDeg += 45f;
        ImageHolder.transform.DORotate(new Vector3(0f, 0f, startDeg), 1.5f).SetUpdate(true).OnComplete(() =>
        {
            i++;
            if (i > 7)
            {
                i = 0;
            }
            if (Images[i].sprite == MiniGame.TargetImage)
            {
                isTrueImage = true;
            }
            else
            {
                isTrueImage = false;
            }
        });        
    }

    public void RotateRandom()
    {
        var RotateDeg = Random.Range(1, 7) * 45;
        ImageHolder.transform.DORotate(new Vector3(0f, 0f, RotateDeg), 1.5f).SetUpdate(true).OnComplete(() =>
           {
               if(Images[i].sprite == MiniGame.TargetImage)
               {
                   isTrueImage = true;
               }
               else
               {
                   isTrueImage = false;
               }
           });
    }
}
