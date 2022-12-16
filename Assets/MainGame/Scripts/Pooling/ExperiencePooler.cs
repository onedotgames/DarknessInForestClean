using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExperiencePooler : PoolerBase
{
    public void InitializeExperiences()
    {
        ObjectList.ForEach(x => x.GetComponent<Experience>().Initialize(GameManager));
    }
}
