using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(menuName = "Scriptable Objects/Inventory Objects/Equipment")]
public class InventoryObjectStats : ScriptableObject
{


    [BoxGroup("Basic Info")]
    [LabelWidth(80)]
    public string Name;

    [BoxGroup("Basic Info")]
    [LabelWidth(80)]
    [TextArea]
    public string Description;

    [HorizontalGroup("Skill Data", 80)]
    [HideLabel, PreviewField(80, ObjectFieldAlignment.Left)]
    public Sprite Icon;  

    [Space(10)]
    [Title("Choose Equipment Type")]
    public EquipmentType EquipmentType;

    [EnableIf("EquipmentType", EquipmentType.Weapon)]
    [SerializeField] private float mDamage;
    public float Damage
    {
        get { return mDamage; }
    }

    [EnableIf("EquipmentType", EquipmentType.Weapon)]
    public SkillSO SkillSO;

    [EnableIf("EquipmentType", EquipmentType.Armor)]
    [Space(10)]
    [Title("Choose Equipment Type")]
    public ArmorType ArmorType;

    [EnableIf("EquipmentType", EquipmentType.Armor)]
    [SerializeField] private int mHealth;
    public int Health
    {
        get { return mHealth; }
    }
    [EnableIf("EquipmentType", EquipmentType.Armor)]
    [SerializeField] private int mSpeed;
    public int Speed
    {
        get { return mSpeed; }
    }

    [EnableIf("EquipmentType", EquipmentType.Armor)]
    [SerializeField] private int mDmgRdc;
    public int DamageReduction
    {
        get { return mDmgRdc; }
    }


}

public enum EquipmentType
{
    Weapon,
    Armor
}

public enum ArmorType
{
    Chest,
    Helmet,
    Boot,
    Glove,
    Jewel
}
