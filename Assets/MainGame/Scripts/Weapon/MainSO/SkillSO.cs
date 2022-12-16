using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Scriptable Objects/Skills/Damage Skills")]
public class SkillSO : ScriptableObject
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

    [VerticalGroup("Skill Data/Attributes"), LabelWidth(100)]
    public float Cooldown; 
    
    [VerticalGroup("Skill Data/Attributes"), LabelWidth(100)]
    public float AttackRange;

    [VerticalGroup("Skill Data/Attributes"), LabelWidth(100),GUIColor(0.8f,0.4f,0.4f)]
    public int BaseDamage;

    [VerticalGroup("Skill Data/Attributes"), LabelWidth(100), GUIColor(0.15f,0.9f,0.25f)]
    public float BaseSpeed;

    [VerticalGroup("Skill Data/Attributes"), LabelWidth(100), GUIColor(0.15f, 0.9f, 0.25f)]
    public int Count;

    [VerticalGroup("Skill Data/Attributes"), LabelWidth(100), GUIColor(0.15f, 0.9f, 0.25f)]
    public Vector3 Size;

    [VerticalGroup("Skill Data/Attributes"), LabelWidth(100), GUIColor(0.15f, 0.9f, 0.25f)]
    public int Guardian;

    [VerticalGroup("Skill Data/Attributes"), LabelWidth(100)]
    public DamagePattern DamagePattern; 

    [VerticalGroup("Skill Data/Attributes"), LabelWidth(100)]
    public PoolerType PoolerType;

    [VerticalGroup("Skill Data/Attributes"), LabelWidth(100)]
    public List<UpgradeData> UpgradeDatas;
}

[System.Serializable]
public class UpgradeData
{
    [LabelWidth(100)][TextArea]
    public string Description;

    [LabelWidth(100)]
    public UpgradeType UpgradeType;

    [ShowIf("UpgradeType", UpgradeType.PropertyChange)][LabelWidth(100)]
    public PropertyToChange PropertyToChange;

    [ShowIf("UpgradeType", UpgradeType.PropertyChange)] [LabelWidth(100)]
    public PropertyChangeType PropertyChangeType;

    [LabelWidth(100)]
    public float ChangeAmount;
}
