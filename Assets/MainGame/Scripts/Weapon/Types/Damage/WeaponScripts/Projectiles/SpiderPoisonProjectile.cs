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
    private float tempSpeed;
    private bool _shouldMove;
    private bool _openModel;
    private bool _returnTriggered = false;

    [SerializeField] private Vector3 _webReducedScale;
    private List<Coroutine> _AllPoisonRoutines;
    private float PoisonAreaDamage = 20f;
    public float PoisonDuration = 2f;
    public override void Initialize(GameManager gameManager)
    {
        base.Initialize(gameManager);
        _AllPoisonRoutines = new List<Coroutine>();

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

            //enemy.gameObject.transform.DOPunchScale(new Vector3(.1f, 0f, 0f), 0.5f);
            //PunchEffect(enemy.gameObject.transform, enemy.IsPunchable);
            enemy.PunchEffect();

            ChangeModel();
            enemy.AOEDamageRoutine = enemy.StartCoroutine(enemy.GetAOEHit(PoisonAreaDamage, AoETickInterval));

            _AllPoisonRoutines.Add(enemy.AOEDamageRoutine);
            Invoke("CloseEnlargement", 0.35f);
            _boxCollider.size = new Vector2(_boxColliderXSize, _boxColliderYSize);

            _shouldMove = false;
            if (!_returnTriggered)
            {
                _returnTriggered = true;
                Invoke("Return", PoisonDuration);

            }
            //Invoke("Return", PoisonDuration);
        }
        if (collision.CompareTag("Boss"))
        {
            var enemy = collision.GetComponent<BossBase>();

            //enemy.gameObject.transform.DOPunchScale(new Vector3(.1f, 0f, 0f), 0.5f);
            //PunchEffect(enemy.gameObject.transform, enemy.IsPunchable);
            enemy.PunchEffect();

            ChangeModel();
            enemy.AOEDamageRoutine = enemy.StartCoroutine(enemy.GetAOEHit(PoisonAreaDamage, AoETickInterval));

            _AllPoisonRoutines.Add(enemy.AOEDamageRoutine);
            Invoke("CloseEnlargement", 0.35f);
            _boxCollider.size = new Vector2(_boxColliderXSize, _boxColliderYSize);

            _shouldMove = false;
            if (!_returnTriggered)
            {
                _returnTriggered = true;
                Invoke("Return", PoisonDuration);

            }
        }
    }

    private void ChangeModel()
    {
        Model.SetActive(false);
        _OpenWebModel.SetActive(true);
        _openModel = true;
        _OpenWebModel.transform.DOScale(new Vector3(_spiderWebSizeX, _spiderWebSizeY), 0.25f);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            _shouldMove = false;
            var enemy = collision.GetComponent<EnemyBase>();
            if (enemy.AOEDamageRoutine != null)
            {
                enemy.StopCoroutine(enemy.AOEDamageRoutine);
                if (_AllPoisonRoutines.Contains(enemy.AOEDamageRoutine))
                {
                    _AllPoisonRoutines.Remove(enemy.AOEDamageRoutine);
                }
            }

        }
        if (collision.CompareTag("Boss"))
        {
            _shouldMove = false;
            var enemy = collision.GetComponent<BossBase>();
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

    private void DeactivatePoisons()
    {
        if (!_AllPoisonRoutines.IsNullOrEmpty())
        {
            _AllPoisonRoutines.ForEach(x => StopCoroutine(x));
        }
    }
    protected override void Return()
    {
        DeactivatePoisons();
        _boxCollider.size = _originalBoxColliderSize;
        _openModel = false;

        _OpenWebModel.SetActive(false);

        base.Return();
    }
}
