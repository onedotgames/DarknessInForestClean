using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BulletShot : MonoBehaviour
{
    public bool isShotted = false;
    public float damage = 0;
    public ParticleSystem FireVFX;
    public Vector3 mDirection;
    public Vector3 DirectionNorm;
    public PoolerBase PoolerBase;
    public GameManager gm;
    [SerializeField] private SpriteRenderer spriteRenderer;
    private void Update()
    {
        if (isShotted && !gm.IsMiniGame)
        {
            Shot();
        }

        if (isShotted && gm.IsMiniGame)
        {
            spriteRenderer.enabled = false;
        }
        else if(isShotted && !gm.IsMiniGame && !spriteRenderer.enabled)
        {
            spriteRenderer.enabled = true;
        }
    }
    public void Subscribe()
    {
        if (gm != null)
        {
            gm.OnLevelFailed += OnGameFailed;
            gm.OnLevelCompleted += OnGameCompleted;
            gm.OnRestartGame += RestartGame;
        }
    }
    


    public void Shot()
    {
        transform.Translate(DirectionNorm * Time.deltaTime * 3);
    }

    public void OnGameFailed()
    {
        isShotted = false;
        PoolerBase.ReturnObjectToPool(gameObject);
    }
    public void OnGameCompleted()
    {
        isShotted = false;
        PoolerBase.ReturnObjectToPool(gameObject);
    }
    private void RestartGame()
    {
        isShotted = false;
        PoolerBase.ReturnObjectToPool(gameObject);
    }

    private void OnBecameInvisible()
    {
        isShotted = false;
        PoolerBase.ReturnObjectToPool(gameObject);
    }


    private void OnDisable()
    {
        if(gm != null)
        {
            gm.OnLevelFailed -= OnGameFailed;
            gm.OnLevelCompleted -= OnGameCompleted;
            gm.OnRestartGame -= RestartGame;
        }
    }
}
