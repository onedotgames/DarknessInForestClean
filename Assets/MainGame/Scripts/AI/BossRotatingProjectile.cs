using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRotatingProjectile : MonoBehaviour
{
    public ParticleSystem VFX;
    public float RotSpeed;
    public bool isShotted = false;
    public float damage = 0;
    public ParticleSystem FireVFX;
    public Vector3 mDirection;
    public Vector3 DirectionNorm;
    public PoolerBase PoolerBase;
    public GameManager gm;
    public GameObject Model;
    private void Update()
    {
        if (!gm.IsGamePaused)
        {
            if (isShotted)
            {
                Shot();
                Model.transform.Rotate(RotSpeed * Time.deltaTime * Vector3.forward);
            }
        }
        
    }

    public void Shot()
    {
        if (!VFX.gameObject.activeInHierarchy)
        {
            VFX.gameObject.SetActive(true);
        }
        if (!VFX.isPlaying)
        {
            VFX.Play();
        }
        transform.Translate(DirectionNorm * Time.deltaTime * 3);
    }

    public void OnGameFailed()
    {
        isShotted = false;
        VFX.Stop(); 
        VFX.gameObject.SetActive(false);
        PoolerBase.ReturnObjectToPool(gameObject);
    }

    private void OnBecameInvisible()
    {
        isShotted = false; 
        VFX.Stop();
        VFX.gameObject.SetActive(false);
        PoolerBase.ReturnObjectToPool(gameObject);
    }


    private void OnDisable()
    {
        if (gm != null)
        {
            gm.OnLevelFailed -= OnGameFailed;
        }
    }
}
