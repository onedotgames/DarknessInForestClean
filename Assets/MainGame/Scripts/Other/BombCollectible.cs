using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombCollectible : MonoBehaviour
{
    public float Damage = 200f;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            var enemy = collision.GetComponent<EnemyBase>();
            enemy.GetHit(Damage);
        }
    }
}
