using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    public GameObject TargetObj;
    public GameObject Player;


    private void Update()
    {
        if(Mathf.Abs(Vector3.Distance(TargetObj.transform.position, Player.transform.position)) < 4.5f)
        {
            TargetObj.SetActive(false);
        }
        else
        {
            TargetObj.SetActive(true);
        }
    }
}
