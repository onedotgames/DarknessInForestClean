using Assets.FantasyMonsters.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemy : EnemyBase
{
    
    public override void AttackMethod()
    {
        base.AttackMethod();
        if (Mathf.Abs(Vector3.Distance(Player.transform.position, transform.position)) <= MeleeRange)
        {
            Monster.ChangeAction(true);
            MonsterClean.ChangeAction(true);
            Monster.Attack();
            MonsterClean.Attack();
            Player.GetHit(BaseDamage);
            GameManager.PlayerHealthManager.SetHealthBar(Player.mMaxHealth);
            CanAttack = false;
            Monster.ChangeAction(false);
            MonsterClean.ChangeAction(false);

        }
    }

    public override void MovementMethod()
    {
        base.MovementMethod();
        if (Mathf.Abs(Vector3.Distance(Player.transform.position, transform.position)) > MeleeRange)
        {
            if (!Monster.GetAction())
            {
                if(Monster.GetState() != (int)MonsterState.Run)
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
