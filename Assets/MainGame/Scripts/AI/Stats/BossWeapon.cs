using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = ("Scriptable Objects/Boss Weapons"))]
public class BossWeapon : ScriptableObject
{
    [SerializeField] private float mBaseHealth;
    public float BaseHealth
    {
        get
        {
            return mBaseHealth;
        }
    }

    [SerializeField] private float mBaseMoveSpeed;
    public float BaseMoveSpeed
    {
        get
        {
            return mBaseMoveSpeed;
        }
    }

    [SerializeField] private float mAttackRange;
    public float AttackRange
    {
        get
        {
            return mAttackRange;
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
