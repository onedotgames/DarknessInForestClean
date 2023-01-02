using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PoolingManager : CustomBehaviour
{
    public List<WeaponPooler> WeaponPooler;
    public List<WeaponSpawner> WeaponPoolerListV2;
    public List<EnemyPooler> EnemyPoolerList;
    public List<ExperiencePooler> ExpPoolerList;
    public List<PoolerBase> EnemyBulletPoolerList;
    public List<PoolerBase> CoinPoolerList;
    public List<PoolerBase> CollectablePoolerList;
    public List<ObjectSpawnerV2> EnemyPoolerListV2;

    public override void Initialize(GameManager gameManager)
    {
        base.Initialize(gameManager);
        SetPools();
        SetPoolsV2();
    }

    private void SetPools()
    {
        WeaponPooler.ForEach(x => 
        {
            x.Initialize(GameManager);
            x.CreatePool();
            x.InitializeWeapons();
        });
        //EnemyPoolerList.ForEach(x => 
        //{
        //    x.Initialize(GameManager);
        //    x.CreatePool();
        //    x.InitializeEnemies();
        //});
        ExpPoolerList.ForEach(x => {
            x.Initialize(GameManager);
            x.CreatePool();
            x.InitializeExperiences();
        });
        EnemyBulletPoolerList.ForEach(x => x.CreatePool());
        CoinPoolerList.ForEach(x => x.CreatePool());
        CollectablePoolerList.ForEach(x => x.CreatePool());

    }
    private void SetPoolsV2()
    {
        EnemyPoolerListV2.ForEach(x => x.Initialize(GameManager));
        WeaponPoolerListV2.ForEach(x => x.Initialize(GameManager));
    }

    public PoolerBase GetPool(int index)
    {
        return WeaponPooler[index];
    }
}
