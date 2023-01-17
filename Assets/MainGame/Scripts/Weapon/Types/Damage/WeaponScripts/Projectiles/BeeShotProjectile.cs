using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BeeShotProjectile : ProjectileBase
{
    public int index = 0;
    [SerializeField] private float _duration;
    public ContactPoint2D[] point2D;
    public override void Initialize(GameManager gameManager)
    {
        base.Initialize(gameManager);
    }

    private void Update()
    {
        if (!GameManager.IsGamePaused && GameManager.IsGameStarted)
        {
            //ContinueuslyPlayVFX(MovementVFX);

            LinearMovement(-Direction);
        }
    }

    private void OnEnable()
    {
        Invoke("Return", _duration);
    }

    private void LevelEnd()
    {
        IsReady = false;
    }
    public void GetDirection(Vector3 direction)
    {
        if (GameManager.JoystickManager.variableJoystick.LastDirection.normalized == Vector2.zero)
        {
            direction = Vector2.left;
            Debug.Log(Direction);
        }
        float addition = 0;

        Quaternion rot = Quaternion.identity;
        rot.eulerAngles = new Vector3(0, 0, (Mathf.Atan2(-direction.y, -direction.x) * Mathf.Rad2Deg + addition * index));
        Model.transform.rotation = rot;

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("ScreenBound"))
        {
            point2D = collision.contacts;

            Direction = Vector2.Reflect(Direction, point2D[0].normal);

            Quaternion rot = Quaternion.identity;
            rot.eulerAngles = new Vector3(0, 0, (Mathf.Atan2(-Direction.y, -Direction.x) * Mathf.Rad2Deg));
            Model.transform.rotation = rot;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            var enemy = collision.gameObject.GetComponent<EnemyBase>();
            if (ParticlePooler != null && ParticlePooler.isActiveAndEnabled)
            {
                var obj = ParticlePooler.Pool.Get();
                obj.gameObject.transform.position = enemy.transform.position;
            }
            enemy.GetHit(Damage);
        }
        if (collision.gameObject.CompareTag("Boss"))
        {
            var enemy = collision.gameObject.GetComponent<BossBase>();
            if (ParticlePooler != null && ParticlePooler.isActiveAndEnabled)
            {
                var obj = ParticlePooler.Pool.Get();
                obj.gameObject.transform.position = enemy.transform.position;
            }
            enemy.GetHit(Damage);
        }
        if (collision.gameObject.CompareTag("Tower"))
        {
            collision.gameObject.GetComponent<TowerSystem>().GetHitTower(Damage);
        }
    }


}
