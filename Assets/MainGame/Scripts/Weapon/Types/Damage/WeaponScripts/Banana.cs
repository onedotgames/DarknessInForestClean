using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Banana : WeaponBase
{
    public float RotSpeed;
    public override void MovementMethod()
    {
        transform.position = GameManager.PlayerManager.CurrentPlayer.transform.position;

        transform.Rotate(RotSpeed * Time.deltaTime * Vector3.forward);
    }
}
