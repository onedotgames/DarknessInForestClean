using Sirenix.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestnutHammer : WeaponBase
{
    public bool mCycleDone = false;
    public bool mIsReturning = false;
    public GameObject Model;
    public float RotSpeed;
    public BoxCollider2D bCol2D;
    public SpriteRenderer spriteRenderer;
    public override void AttackMethod()
    {
        base.AttackMethod();
        if(Mathf.Abs(Vector3.Distance(mPlayer.transform.position,transform.position)) <= AttackRange/2 && mIsReturning == false)
        {
            
            transform.Translate(mDirection * Time.deltaTime * BaseSpeed);
        }
        else
        {
            mIsReturning = true;
            mDirection = Vector3.Normalize(mPlayer.transform.position -transform.position);
            transform.Translate(mDirection * Time.deltaTime * BaseSpeed);
        }
    }

    public override void MovementMethod()
    {
        base.MovementMethod();

        Model.transform.Rotate(RotSpeed * Time.deltaTime * Vector3.forward);

    }


    public override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        if (mIsReturning)
        {
            if (collision.CompareTag(TAGS.Player))
            {
                bCol2D.enabled = false;
                spriteRenderer.enabled = false;
                if(this == GameManager.WeaponManager.ActiveChestnuts[GameManager.WeaponManager.ActiveChestnuts.Count-1])
                {
                    if (UpgradeLevel == 0)
                    {
                        Invoke("InvokeAgain", SkillSO.Cooldown);
                    }
                    else
                    {
                        Invoke("InvokeAgain", StatList.Cooldown);
                    }
                    
                }
                
                
            }
        }
        
    }

    private void InvokeAgain()
    {
        GameManager.WeaponManager.YoyoWeaponSlotRoutine(this);
        bCol2D.enabled = true;
        spriteRenderer.enabled = true;
        PoolerBase.ReturnObjectToPool(gameObject);
    }

    
}
