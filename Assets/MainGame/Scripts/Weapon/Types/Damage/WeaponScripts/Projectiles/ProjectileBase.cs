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

    public override void Initialize(GameManager gameManager)
    {
        base.Initialize(gameManager);
        ProjectileToSpawn = gameObject.GetComponent<ProjectileToSpawn>();
    }
    protected virtual void Return()
    {
        IsReady = false;
        Pooler.ReturnObjectToPool(ProjectileToSpawn);
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
}
