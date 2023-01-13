using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CloverProjectile : ProjectileBase
{

    public override void Initialize(GameManager gameManager)
    {
        base.Initialize(gameManager);
    }

    private void Update()
    {
        if(!GameManager.IsGamePaused && GameManager.IsGameStarted)
        {
            //ContinueuslyPlayVFX(MovementVFX);
            RotateModel();
            LinearMovement(Direction);
        }
    }

    private void LevelEnd()
    {
        IsReady = false;
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
                Return();
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
                Return();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            var enemy = collision.GetComponent<EnemyBase>();
            if (IsAoE)
            {
                if (enemy.AOEDamageRoutine != null)
                {
                    enemy.StopCoroutine(enemy.AOEDamageRoutine);
                }
            }
            
        }
        if (collision.CompareTag("Boss"))
        {
            var enemy = collision.GetComponent<BossBase>();
            if (IsAoE)
            {
                if (enemy.AOEDamageRoutine != null)
                {
                    enemy.StopCoroutine(enemy.AOEDamageRoutine);
                }
            }
        }
    }

    private void OnBecameInvisible()
    {
        Return();
    }
}
