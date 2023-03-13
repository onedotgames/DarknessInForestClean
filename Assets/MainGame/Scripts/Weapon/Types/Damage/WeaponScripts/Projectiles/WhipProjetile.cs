using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class WhipProjetile : ProjectileBase
{
    public ParticleSystem IvySlash;
    public ContactFilter2D contactFilter2;
    public void WhipAttack()
    {
        if (!GameManager.IsMiniGame && !GameManager.IsGamePaused)
        {
            float horizontal = GameManager.JoystickManager.GetHorizontal();
            float vertical = GameManager.JoystickManager.GetVertical();

            if (horizontal != 0 || vertical != 0)
            {

                Cast(horizontal, vertical, GameManager.PlayerManager.CurrentPlayer.Angle);
            }
            else if (horizontal == 0 && vertical == 0)
            {
                Cast(horizontal, vertical, GameManager.PlayerManager.CurrentPlayer.LastAngle);

            }

            IvySlash.Play();

            Invoke("ReturnToPooler", IvySlash.main.duration);
        }
        
    }

    private void Update()
    {
        if(GameManager.IsMiniGame || GameManager.IsGamePaused)
        {
            IvySlash.Stop();
        }
    }

    private void Cast(float horizontal, float vertical, float angle)
    {
        transform.eulerAngles = new Vector3(0, 0, angle - 90);

        RaycastHit2D[] results = Physics2D.BoxCastAll(transform.position, new Vector2(1, 3), angle - 90, GameManager.JoystickManager.variableJoystick.LastDirection, 6f);

        foreach (var item in results)
        {
            if (item.transform.CompareTag("Enemy"))
            {

                //item.transform.DOPunchScale(new Vector3(.1f, 0f, 0f), 0.5f);
                //PunchEffect(enemy.gameObject.transform, enemy.IsPunchable);
                item.transform.GetComponent<EnemyBase>().PunchEffect();

                item.transform.GetComponent<EnemyBase>().GetHit(Damage);
            }
            
            if (item.transform.CompareTag("Boss"))
            {

                //item.transform.DOPunchScale(new Vector3(.1f, 0f, 0f), 0.5f);
                //PunchEffect(enemy.gameObject.transform, enemy.IsPunchable);
                item.transform.GetComponent<BossBase>().PunchEffect();

                item.transform.GetComponent<BossBase>().GetHit(Damage);
            }
        }
    }
    public void ReturnToPooler()
    {
        Return();
    }
}
