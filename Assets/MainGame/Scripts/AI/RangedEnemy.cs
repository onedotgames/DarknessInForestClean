using Assets.FantasyMonsters.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemy : EnemyBase
{
    public override void AttackMethod()
    {
        base.AttackMethod();
        if(Mathf.Abs(Vector3.Distance(Player.transform.position, transform.position)) <= MeleeRange)
        {
            Monster.ChangeAction(true);
            Monster.Attack();
            //mermi gidicek rakibe deðerse get hit çalýþacak.
            var bullet = GameManager.PoolingManager.EnemyBulletPoolerList[(int)EnemyBulletPoolerType.BasicBullet].GetObjectFromPool();
            var bulletShot = bullet.GetComponent<BulletShot>();
            bulletShot.gm = GameManager;
            GameManager.OnLevelFailed += bulletShot.OnGameFailed;
            bulletShot.mDirection = Player.transform.position - transform.position;
            bulletShot.DirectionNorm = bullet.GetComponent<BulletShot>().mDirection.normalized;
            bulletShot.PoolerBase = GameManager.PoolingManager.EnemyBulletPoolerList[(int)EnemyBulletPoolerType.BasicBullet];
            bullet.transform.position = transform.position;
            bulletShot.isShotted = true;
            bulletShot.damage = BaseDamage;
            CanAttack = false;
            Monster.ChangeAction(false);

        }
    }
    public override void MovementMethod()
    {
        base.MovementMethod();
        if (Mathf.Abs(Vector3.Distance(Player.transform.position, transform.position)) > MeleeRange)
        {
            if (!Monster.GetAction())
            {
                if (Monster.GetState() != (int)MonsterState.Run)
                {
                    Monster.SetState(MonsterState.Run);
                    Monster.ChangeAction(false);
                }
                transform.position = Vector3.MoveTowards(transform.position, Player.transform.position, BaseSpeed * Time.deltaTime);
            }
        }
    }
}
