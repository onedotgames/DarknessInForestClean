using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PoolingManager : CustomBehaviour
{
    public List<ProjectileSpawner> ProjectileSpawners;
    public List<ExperiencePooler> ExpPoolerList;
    public List<PoolerBase> EnemyBulletPoolerList;
    public List<PoolerBase> CoinPoolerList;
    public List<PoolerBase> CollectablePoolerList;
    public List<ObjectSpawnerV2> EnemyPoolerListV2;
    public List<ObjectSpawnerV2> EnvironmentObjPoolers;

    public override void Initialize(GameManager gameManager)
    {
        base.Initialize(gameManager);
        SetPools();
        SetPoolsV2();
    }

    private void SetPools()
    {
        ExpPoolerList.ForEach(x => {
            x.Initialize(GameManager);
            x.CreatePool();
            //x.InitializeExperiences(GameManager);
            x.ObjectList.ForEach(x => x.GetComponent<Experience>().Initialize(GameManager));
        });
        EnemyBulletPoolerList.ForEach(x => x.CreatePool());
        CoinPoolerList.ForEach(x => x.CreatePool());
        CollectablePoolerList.ForEach(x => x.CreatePool());

    }
    private void SetPoolsV2()
    {
        EnemyPoolerListV2.ForEach(x => x.Initialize(GameManager));
        ProjectileSpawners.ForEach(x => x.Initialize(GameManager));
        EnvironmentObjPoolers.ForEach(x => x.Initialize(GameManager));

    }

}
