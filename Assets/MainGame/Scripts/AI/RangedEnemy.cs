using Assets.FantasyMonsters.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemy : EnemyBase
{
    public float ChaseRange;
    public override void AttackMethod()
    {
        base.AttackMethod();
        if(Mathf.Abs(Vector3.Distance(Player.transform.position, transform.position)) <= MeleeRange)
        {
            Monster.ChangeAction(true);
            MonsterClean.ChangeAction(true);
            Monster.Attack();
            MonsterClean.Attack();
            //mermi gidicek rakibe de?erse get hit ?al??acak.
            var bullet = GameManager.PoolingManager.EnemyBulletPoolerList[(int)EnemyBulletPoolerType.BasicBullet].GetObjectFromPool();
            var bulletShot = bullet.GetComponent<BulletShot>();
            bulletShot.gm = GameManager;
            //GameManager.OnLevelFailed += bulletShot.OnGameFailed;
            bulletShot.Subscribe();
            bulletShot.mDirection = Player.transform.position - transform.position;
            bulletShot.DirectionNorm = bullet.GetComponent<BulletShot>().mDirection.normalized;
            bulletShot.PoolerBase = GameManager.PoolingManager.EnemyBulletPoolerList[(int)EnemyBulletPoolerType.BasicBullet];
            bullet.transform.position = transform.position;
            bulletShot.isShotted = true;
            bulletShot.damage = BaseDamage;
            CanAttack = false;
            Monster.ChangeAction(false);
            MonsterClean.ChangeAction(false);

        }
    }
    public override void MovementMethod()
    {
        base.MovementMethod();
        if (Mathf.Abs(Vector3.Distance(Player.transform.position, transform.position)) > ChaseRange)
        {
            if (!Monster.GetAction())
            {
                if (Monster.GetState() != (int)MonsterState.Run)
                {
                    Monster.SetState(MonsterState.Run);
                    MonsterClean.SetState(MonsterState.Run);
                    Monster.ChangeAction(false);
                    MonsterClean.ChangeAction(false);
                }
                transform.position = Vector3.MoveTowards(transform.position, Player.transform.position, BaseSpeed * Time.deltaTime);
            }
        }
        
    }
}
