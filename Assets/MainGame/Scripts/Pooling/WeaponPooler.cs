using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPooler : PoolerBase
{
    public void InitializeWeapons()
    {
        ObjectList.ForEach(x => x.GetComponent<WeaponBase>().Initialize(GameManager));
    }

    public override GameObject GetObjectFromPool()
    {
        if (ObjectList.Count < mPoolCount * (0.3f))
        {
            ExpandPool();
            InitializeWeapons();
        }
        return base.GetObjectFromPool();
        
    }
}
