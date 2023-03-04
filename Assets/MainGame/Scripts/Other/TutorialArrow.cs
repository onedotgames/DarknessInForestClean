using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialArrow : MonoBehaviour
{
    Sequence seq;
    public float MaxScale;
    public float MinScale;
    [SerializeField] private float OriginalScale;
    // Start is called before the first frame update
    void Start()
    {
        
        //if (!seq.IsPlaying())
        //{
        //    seq.Play();
        //}
    }

    public void RestartSequence()
    {
        seq = DOTween.Sequence();
        seq.Append(transform.DOScale(Vector3.one * MaxScale, 1f));
        seq.Append(transform.DOScale(Vector3.one * MinScale, 1f));
        seq.SetLoops(-1);
        seq.Restart();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
