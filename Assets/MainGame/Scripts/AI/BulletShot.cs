using System.Collections;
using System.Collections.Generic;
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
    private void Update()
    {
        if (isShotted)
        {
            Shot();
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
        }
    }
}
