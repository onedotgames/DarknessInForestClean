﻿using System;
using System.Collections.Generic;
using Assets.FantasyMonsters.Scripts.Tweens;
using UnityEngine;

namespace Assets.FantasyMonsters.Scripts
{
    /// <summary>
    /// The main script to control monsters.
    /// </summary>
    public class Monster : MonoBehaviour
    {
        public SpriteRenderer Head;
        public List<Sprite> HeadSprites;
        public Animator Animator;
        public bool Variations;
        public event Action<string> OnEvent = eventName => { };

        /// <summary>
        /// Called on Awake.
        /// </summary>
        public void Awake()
        {
            if (Variations)
            {
                var variations = GetComponents<MonsterVariation>();
                var random = UnityEngine.Random.Range(0, variations.Length + 1);

                if (random > 0)
                {
                    variations[random - 1].Apply();
                }
            }

            GetComponent<LayerManager>().SetSortingGroupOrder((int)-transform.localPosition.y);
        }

        public void ChangeState(int num)
        {
            Animator.SetInteger("State", num);
        }

        public void ChangeAction(bool action)
        {
            Animator.SetBool("Action", action);
        }

        /// <summary>
        /// Set animation parameter State to control transitions. Play different state animations (except Attack).
        /// </summary>
        public void SetState(MonsterState state)
        {
            Animator.SetInteger("State", (int) state);
        }
        public bool GetAction()
        {
            return Animator.GetBool("Action");
        }
        public int GetState()
        {
            return Animator.GetInteger("State");
        }

        /// <summary>
        /// Play Attack animation.
        /// </summary>
        public void Attack()
        {
            Animator.SetTrigger("Attack");
            SetHead(1);
        }
        public void ResetAttack()
        {
            Animator.ResetTrigger("Attack");
            SetHead(0);
        }
        /// <summary>
        /// Play scale spring animation.
        /// </summary>
        public virtual void Spring()
        {
            ScaleSpring.Begin(this, 1f, 1.1f, 40, 2);
        }

        // Play Die animation.
        public void Die()
        {
            SetState(MonsterState.Death);
            SetHead(2);
        }

        /// <summary>
        /// Called from animation. Can be used by the game to handle animation events.
        /// </summary>
        public void Event(string eventName)
        {
            OnEvent(eventName);
        }

        /// <summary>
        /// Called from animation.
        /// </summary>
        public void SetHead(int index)
        {
            if (index < HeadSprites.Count)
            {
                Head.sprite = HeadSprites[index];
            }
        }
    }
}