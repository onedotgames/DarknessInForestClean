using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class Sling : WeaponBase
{
    Vector3 moveDirection;
    public ParticleSystem VFX;
    private bool particlePlayed = false;
    private void OnEnable()
    {
        float horizontal = GameManager.JoystickManager.GetHorizontal();
        float vertical = GameManager.JoystickManager.GetVertical();

        if (horizontal != 0 || vertical != 0)
        {

            if (GameManager.PlayerManager.CurrentPlayer.Angle > 0 && GameManager.PlayerManager.CurrentPlayer.Angle < 180)
            {
                moveDirection = GameManager.PlayerManager.CurrentPlayer.Model.transform.right;
            }
            else
            {
                moveDirection = -1 * GameManager.PlayerManager.CurrentPlayer.Model.transform.right;
            }
        }
        else if (horizontal == 0 && vertical == 0)
        {
            if (GameManager.PlayerManager.CurrentPlayer.LastAngle > 0 && GameManager.PlayerManager.CurrentPlayer.LastAngle < 180)
            {
                moveDirection = GameManager.PlayerManager.CurrentPlayer.Model.transform.right;
            }
            else
            {
                moveDirection = -1 * GameManager.PlayerManager.CurrentPlayer.Model.transform.right;
            }
        }
    }
    public override void AttackMethod()
    {
        base.AttackMethod();
        if (!particlePlayed)
        {
            VFX.gameObject.SetActive(true);
            VFX.Play();
            particlePlayed = true;
        }        
        transform.Translate(moveDirection * Time.deltaTime * BaseSpeed);
    }
}
