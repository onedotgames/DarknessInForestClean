using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class RotateIvyLeafs : MonoBehaviour
{
    private bool otherSide = false;
    private void Update()
    {
        RotateLeaf();
    }

    private void RotateLeaf()
    {
        if (!otherSide)
        {
            var rect = transform.GetComponent<RectTransform>();
            rect.DORotate(rect.rotation.eulerAngles + new Vector3(0, 0, 5), 0.3f).OnComplete(() => otherSide = true);
        }
        else
        {
            var rect = transform.GetComponent<RectTransform>();
            rect.DORotate(rect.rotation.eulerAngles + new Vector3(0, 0, -5), 0.3f).OnComplete(() => otherSide = false);
        }
    }
}
