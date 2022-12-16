using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Skills/Utility Skills")]
public class UtilitySO : ScriptableObject
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
    public float Duration;

    [VerticalGroup("Skill Data/Attributes"), LabelWidth(100), GUIColor(0.15f, 0.9f, 0.25f)]
    public Vector3 Size;

    [VerticalGroup("Skill Data/Attributes"), LabelWidth(100)]
    public UtilityPattern UtilityPattern;

    [VerticalGroup("Skill Data/Attributes"), LabelWidth(100)]
    public List<UpgradeUtilityData> UpgradeUtilityDatas;
}

[System.Serializable]
public class UpgradeUtilityData
{
    [LabelWidth(100)]
    [TextArea]
    public string Description;

    [LabelWidth(100)]
    public PropertyChangeType PropertyChangeType;

    [LabelWidth(100)]
    public float ChangeAmount;
}
