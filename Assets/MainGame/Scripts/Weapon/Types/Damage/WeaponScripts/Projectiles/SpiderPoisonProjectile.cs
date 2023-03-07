using DG.Tweening;
using Sirenix.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderPoisonProjectile : ProjectileBase
{
    [SerializeField] private GameObject _OpenWebModel;
    [SerializeField] private BoxCollider2D _boxCollider;
    [SerializeField] private float _boxColliderXSize;
    [SerializeField] private float _boxColliderYSize;
    [SerializeField] private float _spiderWebSizeX;
    [SerializeField] private float _spiderWebSizeY;
    private Vector2 _originalBoxColliderSize;
    public float SlowPower;
    private bool _shouldMove;
    private bool _openModel;
    private bool _returnTriggered = false;

    [SerializeField] private Vector3 _webReducedScale;
    private float PoisonAreaDamage = 20f;
    public float PoisonDuration = 2f;
    private float _timeValue;
    private bool _AoEMode;

    public override void Initialize(GameManager gameManager)
    {
        base.Initialize(gameManager);

        Model.SetActive(true);
        _originalBoxColliderSize = _boxCollider.size;
        _OpenWebModel.transform.localScale = _webReducedScale;
        _shouldMove = true;

    }
    private void Update()
    {
        if (!GameManager.IsGamePaused && GameManager.IsGameStarted && IsReady)
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
    }

    private void CloseEnlargement()
    {
        _openModel = false;
    }


    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            var enemy = collision.GetComponent<EnemyBase>();

            enemy.PunchEffect();

            ChangeModel();

            if (!enemy.Burned)
            {
                enemy.Burned = true;
                enemy.StartCoroutine(enemy.GetAOEHit(PoisonAreaDamage, AoETickInterval, PoisonDuration, enemy.Burned));
            }

            Invoke("CloseEnlargement", 0.35f);
            _boxCollider.size = new Vector2(_boxColliderXSize, _boxColliderYSize);

            _shouldMove = false;
        }
        if (collision.CompareTag("Boss"))
        {
            var enemy = collision.GetComponent<BossBase>();

            enemy.PunchEffect();

            ChangeModel();

            if (!enemy.Poisoned)
            {
                enemy.Poisoned = true;
                enemy.StartCoroutine(enemy.GetAOEHit(PoisonAreaDamage, AoETickInterval, PoisonDuration, enemy.Burned));
            }

            Invoke("CloseEnlargement", 0.35f);
            _boxCollider.size = new Vector2(_boxColliderXSize, _boxColliderYSize);

            _shouldMove = false;
        }
    }

    private void ChangeModel()
    {
        Model.SetActive(false);
        _OpenWebModel.SetActive(true);
        _openModel = true;
        _AoEMode = true;
        _OpenWebModel.transform.DOScale(new Vector3(_spiderWebSizeX, _spiderWebSizeY), 0.25f);
    }

    protected override void Return()
    {
        _boxCollider.size = _originalBoxColliderSize;
        _openModel = false;
        _AoEMode = false;
        _OpenWebModel.SetActive(false);

        base.Return();
    }
}
