using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkillHandler : CustomBehaviour
{
    //public List<WeaponSlot> WeaponSlotList;
    //public int SlotIndex = 0;
    ////public List<Transform> TargetList;

    //[SerializeField] private WeaponSlot mDefaultWeapon;
    
    //public override void Initialize(GameManager gameManager)
    //{
    //    base.Initialize(gameManager);

    //    if(GameManager != null)
    //    {
    //        GameManager.OnStartGame += OnLevelStart;
    //        GameManager.OnLevelFailed += OnLevelFailed;
    //    }
    //}

    //public void AddAndFireDamageSkill(WeaponSlot slot, PoolerBase Pooler)
    //{
    //    WeaponSlotList.Add(slot);
    //    slot.Initialize(GameManager);
    //    slot.ConfigureSkill(Pooler);
    //}

    //private void OnTriggerEnter(Collider other)
    //{
        
    //}

    //private void OnLevelStart()
    //{
    //    AddAndFireDamageSkill(mDefaultWeapon,GameManager.PoolingManager.WeaponPooler[(int)PoolerType.CloverPooler]);
    //}

    //private void OnLevelFailed()
    //{
    //    WeaponSlotList.ForEach(x => x.CancelInvoke());
    //    WeaponSlotList.Clear();
    //}

    //private void OnDestroy()
    //{
    //    if (GameManager != null)
    //    {
    //        GameManager.OnStartGame -= OnLevelStart;
    //        GameManager.OnLevelFailed -= OnLevelFailed;
    //    }
    //}
}
