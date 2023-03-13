using DG.Tweening;
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
    [SerializeField] private GameObject Aura;
    [SerializeField] private Vector3 AuraInitialScale;
    [SerializeField] private CircleCollider2D _circleCollider2D;
    //[SerializeField] private float _boxColliderXSize;
    //[SerializeField] private float _boxColliderYSize;
    [SerializeField] private float _spiderWebSizeX;
    [SerializeField] private float _spiderWebSizeY;
    private float _originalBoxColliderSize;
    [SerializeField]private float _targetBoxColliderSize;

    private bool _openModel;
    private bool _AoEMode;
    public override void Initialize(GameManager gameManager)
    {
        base.Initialize(gameManager);
        //this.TriggerReturn(5f);
        Model.SetActive(true);
        _originalBoxColliderSize = _circleCollider2D.radius;
        Aura.transform.localScale = AuraInitialScale;
        _shouldMove = true;
    }

    private void Update()
    {
        if (!GameManager.IsGamePaused && GameManager.IsGameStarted && IsReady && !GameManager.IsMiniGame)
        {
            //ContinueuslyPlayVFX(MovementVFX);
            //RotateModel();
            if (_shouldMove)
            {
                LinearMovement(Direction);
            }
            if (_AoEMode)
            {
                OpenTimeValue += Time.deltaTime;
                if (OpenTimeValue >= Cooldown)
                {
                    OpenTimeValue = 0;
                    Return();
                }
            }
            else
            {
                CloseTimeValue += Time.deltaTime;
                if (CloseTimeValue >= Cooldown * 2)
                {
                    CloseTimeValue = 0;
                    Return();
                }
            }
        }

        if (GameManager.IsMiniGame && Model.activeSelf)
        {
            Model.SetActive(false);
            Aura.SetActive(false);
        }
        else if (!GameManager.IsMiniGame && !Model.activeSelf)
        {
            Model.SetActive(true);
            Aura.SetActive(true);
        }
    }
    private void CloseEnlargement()
    {
        _openModel = false;
    }
    public void GetDirection(Vector3 direction)
    {
        if (GameManager.JoystickManager.variableJoystick.LastDirection.normalized == Vector2.zero)
        {
            direction = Vector2.left;
            //Debug.Log(Direction);
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

            enemy.GetHit(Damage);

            ChangeModel();

            if(enemy.BaseHealth > 0)
            {
                if (!enemy.FrostBitten)
                {
                    enemy.FrostBitten = true;
                    enemy.StartCoroutine(enemy.GetAOEHit(PoisonAreaDamage, AoETickInterval, PoisonDuration, enemy.FrostBitten));
                }
            }
            
            

            Invoke("CloseEnlargement", 0.35f);
            _circleCollider2D.radius = _targetBoxColliderSize;

            _shouldMove = false; 
        }
        if (collision.CompareTag("Boss"))
        {
            var enemy = collision.GetComponent<BossBase>();

            enemy.GetHit(Damage);

            ChangeModel();
            if (enemy.currentHP > 0)
            {
                if (!enemy.FrostBitten)
                {

                    enemy.FrostBitten = true;
                    enemy.StartCoroutine(enemy.GetAOEHit(PoisonAreaDamage, AoETickInterval, PoisonDuration, enemy.FrostBitten));
                }
            }
            

            Invoke("CloseEnlargement", 0.35f);
            _circleCollider2D.radius = _targetBoxColliderSize;

            _shouldMove = false;            
        }
        if (collision.CompareTag("Barrel"))
        {
            var barrelPos = collision.transform.position;
            BarrelPooler = GameManager.PoolingManager.CollectablePoolerList[(int)CollectablePoolerType.BarrelPooler];
            BarrelPooler.ReturnObjectToPool(collision.gameObject);
            GameManager.BarrelSystem.barrelCount--;
            // coin magnet ya da bomb spawn olacak.
            var k = UnityEngine.Random.Range(0, 13);
            if (k < 3)//bomba?
            {
                var bombPool = GameManager.PoolingManager.CollectablePoolerList[(int)CollectablePoolerType.BombPooler];
                var bomb = bombPool.GetObjectFromPool();
                bomb.transform.position = barrelPos;
            }
            else if (k >= 3 && k < 6)// magne?t
            {
                var magnetPool = GameManager.PoolingManager.CollectablePoolerList[(int)CollectablePoolerType.MagnetPooler];
                var magnet = magnetPool.GetObjectFromPool();
                magnet.transform.position = barrelPos;
            }
            else if (k >= 6 && k < 9) //co?in
            {
                var coinPool = GameManager.PoolingManager.CoinPoolerList[(int)CoinType.Small];
                var coin = coinPool.GetObjectFromPool();
                coin.transform.position = barrelPos;
            }
            else if (k >= 9 && k < 13)//healthPot
            {
                var healthPotPool = GameManager.PoolingManager.CollectablePoolerList[(int)CollectablePoolerType.HealthPotPooler];
                var healthPot = healthPotPool.GetObjectFromPool();
                healthPot.transform.position = barrelPos;
            }
        }

        if (collision.gameObject.CompareTag("Tower"))
        {
            collision.GetComponent<TowerSystem>().GetHitTower(Damage);
        }
    }

    private void ChangeModel()
    {
        Model.SetActive(false);
        Aura.SetActive(true);
        _openModel = true;
        _AoEMode = true;
        Aura.transform.DOScale(new Vector3(_spiderWebSizeX, _spiderWebSizeY, _spiderWebSizeX), 0.25f);
    }

    protected override void Return()
    {
        _circleCollider2D.radius = _originalBoxColliderSize;
        _openModel = false;
        _AoEMode = false;
        Aura.SetActive(false);
        base.Return();
    }

    protected override void TriggerReturn(float time)
    {
        if (!Aura.activeInHierarchy)
            Return();
    }
}
