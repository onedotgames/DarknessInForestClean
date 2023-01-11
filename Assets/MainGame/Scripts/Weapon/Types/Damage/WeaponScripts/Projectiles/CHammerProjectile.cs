using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class CHammerProjectile : ProjectileBase
{
    public bool mCycleDone = false;
    public bool mIsReturning = false;
    public float Range;
    public BoxCollider2D bCol2D;
    public SpriteRenderer spriteRenderer;
    private Player mPlayer;
    public WeaponBaseV2 WeaponBaseV2;
    [SerializeField] private float _duration;
    public override void Initialize(GameManager gameManager)
    {
        base.Initialize(gameManager);
        mIsReturning = false;
        mPlayer = gameManager.PlayerManager.CurrentPlayer;
        SetActivenessOfElements(true);
        //OneTimePlayVFX(MovementVFX);
    }

    private void Update()
    {
        if (!GameManager.IsGamePaused && GameManager.IsGameStarted && IsReady)
        {
            ContinueuslyPlayVFX(MovementVFX);
            RotateModel();
            ChammerMovement();
        }
    }

    private void ChammerMovement()
    {
        if (Mathf.Abs(Vector3.Distance(mPlayer.transform.position, transform.position)) <= Range / 2 && mIsReturning == false)
        {

            transform.Translate(Direction * Time.deltaTime * Speed);
        }
        else
        {
            mIsReturning = true;
            Direction = Vector3.Normalize(mPlayer.transform.position - transform.position);
            transform.Translate(Time.deltaTime * Speed * Direction);
        }
    }

    public virtual void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.CompareTag("Enemy"))
        {
            var enemy = collision.GetComponent<EnemyBase>();
            if (IsAoE)
            {
                enemy.AOEDamageRoutine = enemy.StartCoroutine(enemy.GetAOEHit(Damage, AoETickInterval));
            }
            else
            {

                enemy.GetHit(Damage);
                //Return();
            }
        }
        if (collision.CompareTag("Boss"))
        {
            var enemy = collision.GetComponent<BossBase>();
            if (IsAoE)
            {
                enemy.AOEDamageRoutine = enemy.StartCoroutine(enemy.GetAOEHit(Damage, AoETickInterval));
            }
            else
            {
                enemy.GetHit(Damage);
                //Return();
            }
        }
        if (mIsReturning)
        {
            if (collision.CompareTag("PlayerCombatLayer"))
            {
                SetActivenessOfElements(false);
                IsReady = false;
                if (this == WeaponBaseV2.ActiveChestnuts[WeaponBaseV2.ActiveChestnuts.Count - 1])
                {
                    Invoke("CallAgain", _duration);
                }
                else
                {
                    Return();
                }


            }
        }
    }

    private void SetActivenessOfElements(bool value)
    {
        bCol2D.enabled = value;
        spriteRenderer.enabled = value;
        MovementVFX.gameObject.SetActive(value);
        
    }

    private async void CallAgain()
    {
        Debug.Log("Recall active");
        WeaponBaseV2.ActiveChestnuts.Clear();
        Debug.Log("Cleared");
        Return();
        await WeaponBaseV2.SetAttackMethod();
        
    }
}
