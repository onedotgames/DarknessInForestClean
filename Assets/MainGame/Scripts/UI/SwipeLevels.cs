using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SwipeLevels : MonoBehaviour
{
    public GameObject ScrollBar;
    float scrollPos = 0f;
    float[] pos;
    public Sprite currentLevelMat;
    public Sprite[] levelsMaterials;
    public SpriteRenderer[] backgroundRenderers;
    //public Material currentLevelMat;
    //public Material[] levelsMaterials;
    //public MeshRenderer backgroundRenderer;
    public BackGround backGround;
    private void Update()
    {
        pos = new float[transform.childCount];
        float distance = 1f / (pos.Length - 1f);
        for (int i = 0; i < pos.Length; i++)
        {
            pos[i] = distance * i;
        }

        if (Input.GetMouseButton(0))
        {
            scrollPos = ScrollBar.GetComponent<Scrollbar>().value;
        }
        else
        {
            for (int i = 0; i < pos.Length; i++)
            {
                if(scrollPos < pos[i] + (distance / 2) && scrollPos > pos[i] - (distance / 2))
                {
                    ScrollBar.GetComponent<Scrollbar>().value = Mathf.Lerp(ScrollBar.GetComponent<Scrollbar>().value, pos[i], 0.1f);
                }
            }
        }

        for (int i = 0; i < pos.Length; i++)
        {
            if (scrollPos < pos[i] + (distance / 2) && scrollPos > pos[i] - (distance / 2))
            {
                transform.GetChild(i).localScale = Vector3.Lerp(transform.GetChild(i).localScale, new Vector3(1.2f, 1.2f, 1f), 0.1f);
                currentLevelMat = levelsMaterials[i];
                for (int k = 0; k < backgroundRenderers.Length; k++)
                {
                    backgroundRenderers[k].sprite = currentLevelMat;
                }
                //backgroundRenderer.material = currentLevelMat;
                //backGround.bgMat = currentLevelMat;
                for (int j = 0; j < pos.Length; j++)
                {
                    if(j != i)
                    {
                        transform.GetChild(j).localScale = Vector3.Lerp(transform.GetChild(j).localScale, new Vector3(1f, 1f, 1f), 0.1f);
                    }
                }
            }
        }
    }
}
