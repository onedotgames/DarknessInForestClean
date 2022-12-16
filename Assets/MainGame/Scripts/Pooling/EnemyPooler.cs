using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPooler : PoolerBase
{
    public void InitializeEnemies()
    {
        ObjectList.ForEach(x => x.GetComponent<EnemyBase>().Initialize(GameManager));
    }
}
