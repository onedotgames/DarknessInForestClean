using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBase : CustomBehaviour
{
    public bool IsReady = false;
    public bool IsAoE = false;
    public ProjectileSpawner Pooler;
    public ParticleSystem MovementVFX;
    public ParticleSystem HitVFX;
    public float RotSpeed;
    public float Speed;
    public float Damage;
    public float AoETickInterval = 0.25f;
    public GameObject Model;
    public Vector3 Direction;
    public ProjectileToSpawn ProjectileToSpawn;
    [HideInInspector]
    public PoolerBase BarrelPooler;
    public ParticlePooler ParticlePooler;
    public override void Initialize(GameManager gameManager)
    {
        base.Initialize(gameManager);
        Assign();
        ProjectileToSpawn = gameObject.GetComponent<ProjectileToSpawn>();
    }
    protected virtual void Assign()
    {
        GameManager.OnLevelCompleted += LevelCompleted;
        GameManager.OnLevelFailed += LevelFailed;
    }

    private void LevelCompleted()
    {
        Return();
    }

    private void LevelFailed()
    {
        Return();
    }

    protected virtual void Return()
    {
        IsReady = false;
        Pooler.ReturnObjectToPool(ProjectileToSpawn);
    }

    protected void PunchEffect(Transform transform, bool isPunchable)
    {
        if (isPunchable)
        {
            isPunchable = false;
            transform.DOPunchScale(new Vector3(.1f, 0f, 0f), 0.5f).OnComplete(() => isPunchable = true);
        }
    }

    protected void LinearMovement(Vector3 direction)
    {
        transform.Translate(Time.deltaTime * Speed * direction);
    }

    protected void RotateModel()
    {
        Model.transform.Rotate(RotSpeed * Time.deltaTime * Vector3.forward);
    }

    protected void ContinueuslyPlayVFX(ParticleSystem vfx)
    {
        if (!vfx.gameObject.activeInHierarchy)
        {
            vfx.gameObject.SetActive(true);
        }
        if (!vfx.isPlaying)
        {
            vfx.Play();
        }
    }

    protected void OneTimePlayVFX(ParticleSystem vfx)
    {
        if (!vfx.gameObject.activeInHierarchy)
        {
            vfx.gameObject.SetActive(true);
        }
        if (!vfx.isPlaying)
        {
            vfx.Play();
        }
    }
    protected void PlayHitVFX()
    {
        if (!HitVFX.gameObject.activeInHierarchy)
        {
            HitVFX.gameObject.SetActive(true);
        }
        if (!HitVFX.isPlaying)
        {
            HitVFX.Play();
        }
        Invoke("StopHitVfx", HitVFX.main.duration);

    }

    public void StopHitVfx()
    {
        if (HitVFX.gameObject.activeInHierarchy)
        {
            HitVFX.gameObject.SetActive(false);
        }
    }

    private void OnDisable()
    {
        GameManager.OnLevelCompleted -= LevelCompleted;
        GameManager.OnLevelFailed -= LevelFailed;
    }
}
