using Sirenix.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonDartProjectile : ProjectileBase
{
    public ParticleSystem PoisonVFX;
    public float PoisonDuration = 2f;
    private float PoisonAreaDamage = 20f;
    private bool _shouldMove;
    private List<Coroutine> _AllPoisonRoutines;
    public override void Initialize(GameManager gameManager)
    {
        base.Initialize(gameManager);
        _AllPoisonRoutines = new List<Coroutine>();
        _shouldMove = true;
    }

    private void Update()
    {
        if (!GameManager.IsGamePaused && GameManager.IsGameStarted)
        {
            ContinueuslyPlayVFX(MovementVFX);
            //RotateModel();
            if (_shouldMove)
            {
                LinearMovement(Direction);
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
        rot.eulerAngles = new Vector3(0, 0, (Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg -45));
        Model.transform.rotation = rot;

    }
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            var enemy = collision.GetComponent<EnemyBase>();
            if (IsAoE)
            {
                enemy.GetHit(Damage);

                enemy.AOEDamageRoutine = enemy.StartCoroutine(enemy.GetAOEHit(Damage, AoETickInterval));

                if(!PoisonVFX.isPlaying)
                    PoisonVFX.Play();

                _AllPoisonRoutines.Add(enemy.AOEDamageRoutine);
                Invoke("Return", PoisonDuration);
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
                enemy.GetHit(Damage);

                enemy.AOEDamageRoutine = enemy.StartCoroutine(enemy.GetAOEHit(Damage, AoETickInterval));

                if (!PoisonVFX.isPlaying)
                    PoisonVFX.Play();

                _AllPoisonRoutines.Add(enemy.AOEDamageRoutine);
                Invoke("Return", PoisonDuration);
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
            _shouldMove = false;
            var enemy = collision.GetComponent<EnemyBase>();
            if (IsAoE)
            {
                if (enemy.AOEDamageRoutine != null)
                {
                    enemy.StopCoroutine(enemy.AOEDamageRoutine);
                    if (_AllPoisonRoutines.Contains(enemy.AOEDamageRoutine))
                    {
                        _AllPoisonRoutines.Remove(enemy.AOEDamageRoutine);
                    }
                }
            }

        }
        if (collision.CompareTag("Boss"))
        {
            _shouldMove = false;
            var enemy = collision.GetComponent<BossBase>();
            if (IsAoE)
            {
                if (enemy.AOEDamageRoutine != null)
                {
                    enemy.StopCoroutine(enemy.AOEDamageRoutine); 
                    if (_AllPoisonRoutines.Contains(enemy.AOEDamageRoutine))
                    {
                        _AllPoisonRoutines.Remove(enemy.AOEDamageRoutine);
                    }
                }
            }
        }
    }

    private void DeactivatePoisonDart()
    {
        if (!_AllPoisonRoutines.IsNullOrEmpty())
        {
            _AllPoisonRoutines.ForEach(x => StopCoroutine(x));
        }
    }

    protected override void Return()
    {
        DeactivatePoisonDart();
        base.Return();
    }

    private void OnBecameInvisible()
    {
        if (!PoisonVFX.isPlaying)
            Return();
    }
}
