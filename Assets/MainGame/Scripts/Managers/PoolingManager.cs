using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PoolingManager : CustomBehaviour
{
    public List<WeaponPooler> WeaponPooler;
    public List<EnemyPooler> EnemyPoolerList;
    public List<ExperiencePooler> ExpPoolerList;
    public List<PoolerBase> EnemyBulletPoolerList;
    public List<PoolerBase> CoinPoolerList;
    public List<PoolerBase> CollectablePoolerList;
    public override void Initialize(GameManager gameManager)
    {
        base.Initialize(gameManager);
        SetPools();
    }

    private void SetPools()
    {
        WeaponPooler.ForEach(x => 
        {
            x.Initialize(GameManager);
            x.CreatePool();
            x.InitializeWeapons();
        }
        );
        EnemyPoolerList.ForEach(x => 
        {
            x.Initialize(GameManager);
            x.CreatePool();
            x.InitializeEnemies();
        });
        ExpPoolerList.ForEach(x => {
            x.Initialize(GameManager);
            x.CreatePool();
            x.InitializeExperiences();
        });
        EnemyBulletPoolerList.ForEach(x => x.CreatePool());
        CoinPoolerList.ForEach(x => x.CreatePool());
        CollectablePoolerList.ForEach(x => x.CreatePool());
    }

    public PoolerBase GetPool(int index)
    {
        return WeaponPooler[index];
    }

    private void LevelFailed()
    {

    }

    private void SubEvents()
    {
        if (GameManager != null)
        {
            GameManager.OnLevelFailed += LevelFailed;
        }
    }

    private void UnSubEvents()
    {
        if (GameManager != null)
        {
            GameManager.OnLevelFailed -= LevelFailed;

        }
    }

    //private void OnDisable()
    //{
    //    UnSubEvents();
    //}

    //private void OnDestroy()
    //{
    //    UnSubEvents();
    //}
}
