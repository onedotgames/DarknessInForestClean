using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SlingBaseProjectile : ProjectileBase
{
    public override void Initialize(GameManager gameManager)
    {
        base.Initialize(gameManager);
    }
    private void OnEnable()
    {
        float horizontal = GameManager.JoystickManager.GetHorizontal();
        float vertical = GameManager.JoystickManager.GetVertical();

        if (horizontal != 0 || vertical != 0)
        {

            if (GameManager.PlayerManager.CurrentPlayer.Angle > 0 && GameManager.PlayerManager.CurrentPlayer.Angle < 180)
            {
                Direction = GameManager.PlayerManager.CurrentPlayer.Model.transform.right;
            }
            else
            {
                Direction = -1 * GameManager.PlayerManager.CurrentPlayer.Model.transform.right;
            }
        }
        else if (horizontal == 0 && vertical == 0)
        {
            if (GameManager.PlayerManager.CurrentPlayer.LastAngle > 0 && GameManager.PlayerManager.CurrentPlayer.LastAngle < 180)
            {
                Direction = GameManager.PlayerManager.CurrentPlayer.Model.transform.right;
            }
            else
            {
                Direction = -1 * GameManager.PlayerManager.CurrentPlayer.Model.transform.right;
            }
        }
    }
    private void Update()
    {
        LinearMovement(Direction);
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
                if (ParticlePooler != null && ParticlePooler.isActiveAndEnabled)
                {
                    var obj = ParticlePooler.Pool.Get();
                    obj.gameObject.transform.position = enemy.transform.position;
                }
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
                if (ParticlePooler != null && ParticlePooler.isActiveAndEnabled)
                {
                    var obj = ParticlePooler.Pool.Get();
                    obj.gameObject.transform.position = enemy.transform.position;
                }
                enemy.GetHit(Damage);
                Return();
            }
        }
    }



    public void GetDirection(Vector3 direction)
    {
        if (GameManager.JoystickManager.variableJoystick.LastDirection.normalized == Vector2.zero)
        {
            direction = Vector2.left;
            Debug.Log(Direction);
        }

        Quaternion rot = Quaternion.identity;
        rot.eulerAngles = new Vector3(0, 0, (Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 45));
        Model.transform.rotation = rot;

    }

    private void OnBecameInvisible()
    {
        Return();
    }
}
