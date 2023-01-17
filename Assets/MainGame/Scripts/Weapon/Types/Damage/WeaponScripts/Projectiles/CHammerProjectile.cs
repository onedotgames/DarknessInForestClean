using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

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
            
            //ContinueuslyPlayVFX(MovementVFX);
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
                if(ParticlePooler != null && ParticlePooler.isActiveAndEnabled)
                {
                    var obj = ParticlePooler.Pool.Get();
                    obj.gameObject.transform.position = enemy.transform.position;
                }
                enemy.gameObject.transform.DOPunchScale(new Vector3(.1f, 0f, 0f), 0.5f);

                enemy.GetHit(Damage);
                //PlayHitVFX();
                
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
                if (ParticlePooler != null && ParticlePooler.isActiveAndEnabled)
                {
                    var obj = ParticlePooler.Pool.Get();
                    obj.gameObject.transform.position = enemy.transform.position;
                }
                enemy.gameObject.transform.DOPunchScale(new Vector3(.1f, 0f, 0f), 0.5f);

                enemy.GetHit(Damage);
                PlayHitVFX();
                //Return();
            }
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
        if (mIsReturning)
        {
            if (collision.CompareTag("PlayerCombatLayer"))
            {
                SetActivenessOfElements(false);
                IsReady = false;
                if (this == WeaponBaseV2.ActiveChestnuts[WeaponBaseV2.ActiveChestnuts.Count - 1])
                {
                    StopHitVfx();
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
