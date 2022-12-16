using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class WeaponSlot : CustomBehaviour
{
    public SkillSO SkillSO;
    private float mCooldown;

    public float BaseDamage;
    public float BaseSpeed;
    public float AttackRange;
    private PoolerBase mPooler;

    private Player mPlayer;
    public override void Initialize(GameManager gameManager)
    {
        base.Initialize(gameManager);

    }

    public void ConfigureSkill(PoolerBase Pooler)
    {
        mPlayer = GameManager.PlayerManager.CurrentPlayer;
        BaseDamage = SkillSO.BaseDamage;
        BaseSpeed = SkillSO.BaseSpeed;
        mCooldown = SkillSO.Cooldown;
        mPooler = Pooler;
        AttackRange = SkillSO.AttackRange;
        InvokeRepeating("StartSkill", 0.2f, mCooldown);
    }

    //private void StartSkill()
    //{
    //    if(GameManager.PlayerManager.PlayerSkillHandler.TargetList.Count != 0)
    //    {
            
    //        if(GetClosestEnemy(GameManager.PlayerManager.PlayerSkillHandler.TargetList) != null)
    //        {
    //            var target = GetClosestEnemy(GameManager.PlayerManager.PlayerSkillHandler.TargetList);
    //            var obj = mPooler.GetObjectFromPool();
    //            obj.transform.position = GameManager.PlayerManager.CurrentPlayer.transform.position;
    //            //obj.GetComponent<DamageSkill>().SetSkill(target, mPooler);
    //        }
            
    //    }
        
    //}

    private Transform GetClosestEnemy(List<Transform> enemies)
    {
        Transform bestTarget = null;
        float closestDistanceSqr = AttackRange;
        Vector3 currentPosition = mPlayer.transform.position;
        foreach (Transform potentialTarget in enemies)
        {
            Vector3 directionToTarget = potentialTarget.position - currentPosition;
            float dSqrToTarget = directionToTarget.sqrMagnitude;
            if (dSqrToTarget < closestDistanceSqr)
            {
                closestDistanceSqr = dSqrToTarget;
                bestTarget = potentialTarget;
            }
        }
        return bestTarget;
    }
}
