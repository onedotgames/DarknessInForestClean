using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;

public class BeeShot : WeaponBase
{
    Vector3 moveDirection;
    public GameObject Model;
    public Vector3 upVector;
    public ContactPoint2D[] point2D;
    public float Duration;
    public int index;
    

    private void OnEnable()
    {
        moveDirection = GameManager.JoystickManager.variableJoystick.LastDirection.normalized;
        float addition = 0;
        if(index % 2 == 0)
        {
            addition = Random.Range(25, 45);
        }else if(index % 2 == 1)
        {
            addition = -Random.Range(25, 45);
        }
        Quaternion rot = Quaternion.identity;
        rot.eulerAngles = new Vector3(0, 0, (Mathf.Atan2(-moveDirection.y, -moveDirection.x) * Mathf.Rad2Deg + addition * index));
        Model.transform.rotation = rot;

    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("ScreenBound"))
        { 
            point2D = collision.contacts;
            
            moveDirection = Vector2.Reflect(moveDirection, point2D[0].normal);

            Quaternion rot = Quaternion.identity;
            rot.eulerAngles = new Vector3(0, 0, (Mathf.Atan2(-moveDirection.y, -moveDirection.x) * Mathf.Rad2Deg));
            Model.transform.rotation = rot;
        }


        if (collision.gameObject.CompareTag("Enemy"))
        {
            var enemy = collision.gameObject.GetComponent<EnemyBase>();

            enemy.GetHit(BaseDamage);
        }
    }




    public override void AttackMethod()
    {
        base.AttackMethod();
        transform.Translate(-moveDirection * Time.deltaTime * BaseSpeed);
    }
}
