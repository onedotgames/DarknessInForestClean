using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Lofelt.NiceVibrations;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine.UIElements;

public class Player : CustomBehaviour
{
    [Title("Control Options")]
    public bool ShouldCharacterColliderDetectCollisions = false;

    [Title("Player Variables")]
    public PlayerVariables PlayerVariables;

    [Title("Model & Model Options")]
    public GameObject Model;
    public Vector3 Velocity;

    #region Player Variables
    public float mForwardSpeed;
    private float mAcceleration;
    private float mDeceleration;
    public float mMaxHealth;
    public float mCurrentHealth;
    public float DamageReduction = 1;
    #endregion
    public float MagnetRadius;
    #region ControlDelegates
    public delegate void ControlDelegate();
    private ControlDelegate mControlMethod;
    public delegate void RotationDelegate();
    private RotationDelegate mRotationMethod;
    #endregion


    private Vector3 mConstantMoveAxis;
    private Vector3 mStartMousePos;
    private Vector3 mSwipeDistance;
    private Vector3 mUpdatedPosition;
    public IEnumerator ShieldOnRoutine;

    public bool IsGameStarted = false;
    private bool IsGetHit = false;

    [SerializeField] private float xAxisMin;
    [SerializeField] private float xAxisMax;
    [SerializeField] private Volume volume;
    private Vignette vignette;

    public ParticleSystem RechargableShieldOn;
    public ParticleSystem RechargableShieldOff;
    public bool IsShieldOn = false;
    public float ShieldValue = 0;
    public UtilityBase ShieldUtility;
    public GameObject Indicator;
    public GameObject[] experiences;
    public Animator PlayerAnim;
    public ExpCollider ExpCollider;

    public float Angle;
    public float LastAngle;

    public Vector2 Direction;
    public Vector3 pos;
    public Vector3 oldPos;
    public override void Initialize(GameManager gameManager)
    {
        base.Initialize(gameManager);
        InitializeCustomOptions();
        
        if(volume.profile.TryGet<Vignette>(out var vig))
        {
            vignette = vig;
        }
        ExpCollider.Initialize(gameManager);
    }
    private void Update()
    {
        if (IsGameStarted)
        {
            if (mControlMethod != null)
                mControlMethod();
            if (mRotationMethod != null)
                mRotationMethod();
        }

        if (IsGetHit)
        {
            vignette.intensity.value = Mathf.PingPong(Time.time * 0.75f, .5f);
            if(vignette.intensity.value <= 0.02f)
            {
                IsGetHit = false;
                vignette.intensity.value = 0;
            }
        }
        oldPos = transform.position;
    }
    private void LateUpdate()
    {
        pos = transform.position;
        var temp = (pos - oldPos).normalized;

        
        if(temp != Vector3.zero)
        {
            Direction = temp;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.CompareTag("Coin"))
        {
            GameManager.PlayerManager.UpdateCoinCountData(5);
            GameManager.UIManager.GetPanel(Panels.Hud).UpdateUIElements();
            GameManager.PoolingManager.CoinPoolerList[(int)CoinType.Small].ReturnObjectToPool(collision.gameObject);
        }
        if (collision.CompareTag("Bullet"))
        {
            collision.GetComponent<BulletShot>().FireVFX.Play();
            GetHit(collision.GetComponent<BulletShot>().damage);
            GameManager.PlayerHealthManager.SetHealthBar(mMaxHealth);
        }
        if (collision.CompareTag("Magnet"))
        {
            var exps = Physics2D.OverlapCircleAll(transform.position, MagnetRadius);
            foreach (var item in exps)
            {
                
                if (item.transform.CompareTag("Experience"))
                {
                    if (item.transform.gameObject.activeInHierarchy)
                    {
                        item.GetComponent<Experience>().isGoing = true;
                    }
                }
                

            }
            GameManager.PoolingManager.CollectablePoolerList[(int)CollectablePoolerType.MagnetPooler].ReturnObjectToPool(collision.gameObject);

        }
        if (collision.CompareTag("Bomb"))
        {
            var particle = collision.transform.GetChild(0).gameObject;
            particle.GetComponent<ParticleSystem>().Play();
            collision.transform.DOScale(new Vector3(20f, 20f, 20f), 1.5f).OnComplete(() =>
            {
                collision.transform.localScale = Vector3.one;
                particle.GetComponent<ParticleSystem>().Stop();
                GameManager.PoolingManager.CollectablePoolerList[(int)CollectablePoolerType.BombPooler].ReturnObjectToPool(collision.transform.parent.gameObject);
            });            
        }
    }
    private void InitializeCustomOptions()
    {
        SetPlayerStats();
        DetermineModel();
        SubscribeEvents();
        mControlMethod = Custom2DController;
        mRotationMethod = JoystickRotation;
    }

    private void SetPlayerStats()
    {
        mForwardSpeed = PlayerVariables.MoveSpeed;
        mMaxHealth = PlayerVariables.PlayerMaxHealth;
        mCurrentHealth = mMaxHealth;
        mAcceleration = PlayerVariables.WalkAcceleration;
        mDeceleration = PlayerVariables.GroundDeceleration;
    }

    private void Custom2DController()
    {
        float horizontalInput = GameManager.JoystickManager.GetHorizontal();
        float verticalInput = GameManager.JoystickManager.GetVertical();

        if (horizontalInput != 0)
        {
            Velocity.x = Mathf.MoveTowards(Velocity.x, mForwardSpeed * horizontalInput, mAcceleration * Time.deltaTime);
        }
        else
        {
            Velocity.x = Mathf.MoveTowards(Velocity.x, 0, mDeceleration * Time.deltaTime);
        }

        if (verticalInput != 0)
        {
            Velocity.y = Mathf.MoveTowards(Velocity.y, mForwardSpeed * verticalInput, mAcceleration * Time.deltaTime);
        }
        else
        {
            Velocity.y = Mathf.MoveTowards(Velocity.y, 0, mDeceleration * Time.deltaTime);
        }

        transform.Translate(Velocity * Time.deltaTime);
        if(Velocity != Vector3.zero)
        {
            PlayerAnim.SetBool("isMoving", true);
        }
        else
        {
            PlayerAnim.SetBool("isMoving", false);
        }
    }

    private void SubscribeEvents()
    {
        if (GameManager != null)
        {
            GameManager.OnStartGame += StartGame;
            GameManager.OnReturnToMainMenu += ReturnToMainMenu;
        }
    }

    private void DetermineModel()
    {
        Model.SetActive(true);
    }   

    private void JoystickRotation()
    {
        float horizontal = GameManager.JoystickManager.GetHorizontal();
        float vertical = GameManager.JoystickManager.GetVertical();

        
        
        if(horizontal != 0 || vertical != 0)
        {
            Angle = Mathf.Atan2(vertical, horizontal) * Mathf.Rad2Deg + 90;
            //Debug.Log(angle);
            Indicator.transform.eulerAngles = new Vector3(0, 0, Angle);
            if (Angle > 0 && Angle < 180)
            {
                Model.transform.eulerAngles = Vector3.zero;
            }
            else
            {
                Model.transform.eulerAngles = new Vector3(0, 180, 0);
            }
        }
        else if (horizontal == 0 && vertical == 0)
        {
            LastAngle = GameManager.JoystickManager.variableJoystick.LastAngle + 90;
            Indicator.transform.eulerAngles = new Vector3(0,0,LastAngle);

            if (LastAngle > 0 && LastAngle < 180)
            {
                Model.transform.eulerAngles = Vector3.zero;
            }
            else
            {
                Model.transform.eulerAngles = new Vector3(0, 180, 0);
            }
        }

        
    }


    public void GetHit(float amount)
    {
        
        amount *= DamageReduction;
        IsGetHit = true;
        if (!IsShieldOn)
        {
            mCurrentHealth -= amount;
            GameManager.PlayerHealthManager.SetHealthBar(mMaxHealth);
        }
        else
        {
            if(ShieldValue > 0)
            {
                ShieldValue -= amount;
                if(ShieldValue <= 0)
                {
                    ShieldOff();
                }

            }
        }
        CheckDeath();
    }

    private void CheckDeath()
    {
        if (mCurrentHealth <= 0)
        {
            OnDeath();
        }
    }

    public void CacheShieldRoutine()
    {
        
        ShieldOnRoutine = ShieldOn();
        StartCoroutine(ShieldOnRoutine);
        
    }

    public IEnumerator ShieldOn()
    {
        RechargableShieldOn.gameObject.SetActive(true);

        RechargableShieldOn.Play();
        ShieldValue = ShieldUtility.UtilitySO.UpgradeUtilityDatas[ShieldUtility.UpgradeLevel].ChangeAmount;
        IsShieldOn = true;
        yield return new WaitForSeconds(ShieldUtility.UtilitySO.Duration);
        if(ShieldValue > 0)
        {
            ShieldOff();
        }
    }

    public void ShieldOff()
    {
        RechargableShieldOn.gameObject.SetActive(false);
        RechargableShieldOff.Play();
        IsShieldOn = false;

        if(ShieldOnRoutine != null)
        {
            StopCoroutine(ShieldOnRoutine);

        }
        
        Invoke("ReOpenShield", ShieldUtility.UtilitySO.Cooldown);
    }

    public void ReOpenShield()
    {
        CacheShieldRoutine();
    }

    private void OnDeath()
    {
        GameManager.LevelFailed();
        IsGameStarted = false;
    }

    public void OpenRechargableShield()
    {
        IsShieldOn = true;

        RechargableShieldOn.Play(); 
    }


    private void StartGame()
    {
        IsGameStarted = true;
    }

    private void ReturnToMainMenu()
    {
        transform.position = Vector3.zero;
        SetPlayerStats(); 
        PlayerAnim.SetBool("isMoving", false);
    }

    private void OnDisable()
    {
        if (GameManager != null)
        {
            GameManager.OnStartGame -= StartGame;
            GameManager.OnReturnToMainMenu -= ReturnToMainMenu;
        }
    }
}
