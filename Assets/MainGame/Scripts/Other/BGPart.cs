using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGPart : MonoBehaviour
{
    public bool PlayerOnMe = false;
    public bool IMLastPart = false;
    public BGSystem bGSystem;
    public int index;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerOnMe = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerOnMe = false;
            bGSystem.lastCenterIndex = index;
            for (int i = 0; i < bGSystem.backGrounds.Count; i++)
            {
                bGSystem.backGrounds[i].GetComponent<BGPart>().IMLastPart = false;
            }
            IMLastPart = true;
            bGSystem.CanMove = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        bGSystem.CheckCenter();
    }
}
