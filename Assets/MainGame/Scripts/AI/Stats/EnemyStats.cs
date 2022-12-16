using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName =("Scriptable Objects/Enemy Stats"))]
public class EnemyStats : ScriptableObject
{
    [SerializeField] private float mBaseHealth;
    public float BaseHealth 
    { 
        get 
        {
            return mBaseHealth;
        }
    }

    [SerializeField] private float mBaseSpeed;
    public float BaseSpeed 
    { 
        get
        {
            return mBaseSpeed;
        } 
    }

    [SerializeField] private float mMeleeRange;
    public float MeleeRange 
    { 
        get
        {
            return mMeleeRange;
        } 
    }

    [SerializeField] private float mAttackCooldown;
    public float AttackCooldown 
    { 
        get
        {
            return mAttackCooldown;
        } 
    }

    [SerializeField] private float mDamage;

    public float Damage
    {
        get
        {
            return mDamage;
        }
    }
}
