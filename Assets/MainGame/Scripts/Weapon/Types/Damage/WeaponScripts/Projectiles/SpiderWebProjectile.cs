using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderWebProjectile : ProjectileBase
{
    [SerializeField] private GameObject _OpenWebModel;
    [SerializeField] private BoxCollider2D _boxCollider;
    [SerializeField] private float _boxColliderXSize;
    [SerializeField] private float _boxColliderYSize; 
    [SerializeField] private float _spiderWebSizeX;
    [SerializeField] private float _spiderWebSizeY;
    private Vector2 _originalBoxColliderSize;
    public float SlowPower;
    public float Duration;
    private float tempSpeed;
    private bool _shouldMove;
    private bool _openModel;
    private bool _returnTriggered = false;
    [SerializeField] private Vector3 _webReducedScale;
    public override void Initialize(GameManager gameManager)
    {
        base.Initialize(gameManager);

        Model.SetActive(true);
        _originalBoxColliderSize = _boxCollider.size;
        _OpenWebModel.transform.localScale = _webReducedScale;
        _shouldMove = true;
        _returnTriggered = false;

    }
    private void Update()
    {
        if (!GameManager.IsGamePaused && GameManager.IsGameStarted)
        {
            //ContinueuslyPlayVFX(MovementVFX);
            //RotateModel();
            if (_shouldMove)
            {
                LinearMovement(Direction);
            }
        }
    }


    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            var enemy = collision.GetComponent<EnemyBase>();

            //PunchEffect(enemy.gameObject.transform, enemy.IsPunchable);
            enemy.PunchEffect();
            //enemy.gameObject.transform.DOPunchScale(new Vector3(.1f, 0f, 0f), 0.5f);

            Model.SetActive(false);
            _OpenWebModel.SetActive(true);
            _openModel = true;
            _OpenWebModel.transform.DOScale(new Vector3(_spiderWebSizeX, _spiderWebSizeY), 0.25f);
            if (!_returnTriggered)
            {
                _returnTriggered = true;
                Invoke("Return", Duration + 0.25f);

            }
            _boxCollider.size = new Vector2(_boxColliderXSize,_boxColliderYSize);
            tempSpeed = enemy.mStats.BaseSpeed;
            enemy.BaseSpeed -= SlowPower;
            _shouldMove = false;
            
        }
        if (collision.CompareTag("Boss"))
        {
            var enemy = collision.GetComponent<BossBase>();
            //enemy.gameObject.transform.DOPunchScale(new Vector3(.1f, 0f, 0f), 0.5f);
            //PunchEffect(enemy.gameObject.transform, enemy.IsPunchable);
            enemy.PunchEffect();

            Model.SetActive(false);
            _OpenWebModel.SetActive(true);
            _openModel = true;
            _OpenWebModel.transform.DOScale(new Vector3(_spiderWebSizeX, _spiderWebSizeY), 0.25f);
            if (!_returnTriggered)
            {
                _returnTriggered = true;
                Invoke("Return", Duration + 0.25f);

            }
            _boxCollider.size = new Vector2(_boxColliderXSize, _boxColliderYSize);
            tempSpeed = enemy.Stats.BaseMoveSpeed;
            enemy.BaseMoveSpeed -= SlowPower;
            _shouldMove = false;
            
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            _shouldMove = false;
            var enemy = collision.GetComponent<EnemyBase>();

            enemy.BaseSpeed = tempSpeed;
        }
        if (collision.CompareTag("Boss"))
        {
            _shouldMove = false;
            var enemy = collision.GetComponent<BossBase>();
      
            enemy.BaseMoveSpeed = tempSpeed;
        }
    }

    protected override void Return()
    {
        _boxCollider.size = _originalBoxColliderSize;
        _openModel = false;

        _OpenWebModel.SetActive(false);

        base.Return();
    }
}
