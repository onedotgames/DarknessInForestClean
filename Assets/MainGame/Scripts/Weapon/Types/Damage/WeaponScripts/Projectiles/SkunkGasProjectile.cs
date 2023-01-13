using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkunkGasProjectile : ProjectileBase
{
    private float PoisonAreaDamage = 20f;
    public Player Player;
    public override void Initialize(GameManager gameManager)
    {
        base.Initialize(gameManager);
    }

    private void Update()
    {
        if (!GameManager.IsGamePaused && GameManager.IsGameStarted)
        {
            transform.position = Player.gameObject.transform.position;
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            var enemy = collision.GetComponent<EnemyBase>();

            enemy.GetHit(Damage);

            enemy.AOEDamageRoutine = enemy.StartCoroutine(enemy.GetAOEHit(PoisonAreaDamage, AoETickInterval));

        }
        if (collision.CompareTag("Boss"))
        {
            var enemy = collision.GetComponent<BossBase>();
            enemy.GetHit(Damage);

            enemy.AOEDamageRoutine = enemy.StartCoroutine(enemy.GetAOEHit(PoisonAreaDamage, AoETickInterval));


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

            if (enemy.AOEDamageRoutine != null)
            {
                enemy.StopCoroutine(enemy.AOEDamageRoutine);

            }
            
        }
    }
}
