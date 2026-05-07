// 1. SkillData.cs
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewSkill", menuName = "Skill System/Skill Data")]
public class SkillData : ScriptableObject
{
    [Header("Skill Information")]
    public string skillID; // e.g., "tactical_strike_01"
    public string skillName;
    [TextArea] public string description;
    public Sprite icon;

    [Header("Unlock Requirements")]
    [Tooltip("How many skill points are required to unlock this skill?")]
    public int pointCost = 1;

    [Tooltip("Skills that MUST be unlocked before this one becomes available.")]
    public List<SkillData> prerequisites = new List<SkillData>();
}
