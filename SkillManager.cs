// 2. SkillManager.cs
using System;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour, ISaveable
{
    public static SkillManager Instance { get; private set; }

    // Global events for your UI to listen to
    public static event Action<SkillData> OnSkillUnlocked;
    public static event Action<int> OnSkillPointsChanged;

    [Header("Player Data")]
    [SerializeField] private int availableSkillPoints = 0;

    // Fast lookup for unlocked skills
    private HashSet<string> unlockedSkills = new HashSet<string>();

    // --- ISaveable Implementation ---
    public string SaveID => "Global_SkillTree";

    [System.Serializable]
    private struct SkillSaveData
    {
        public int points;
        public List<string> unlockedList;
    }

    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(gameObject);
        else Instance = this;
    }

    public void AddSkillPoints(int amount)
    {
        availableSkillPoints += amount;
        OnSkillPointsChanged?.Invoke(availableSkillPoints);
    }

    public bool IsUnlocked(SkillData skill)
    {
        return skill != null && unlockedSkills.Contains(skill.skillID);
    }

    public bool CanUnlock(SkillData skill)
    {
        if (skill == null || IsUnlocked(skill)) return false;
        
        // Check point cost
        if (availableSkillPoints < skill.pointCost) return false;

        // Check prerequisites
        foreach (SkillData prereq in skill.prerequisites)
        {
            if (!IsUnlocked(prereq)) return false; // A required skill is missing
        }

        return true;
    }

    public void AttemptUnlock(SkillData skill)
    {
        if (CanUnlock(skill))
        {
            availableSkillPoints -= skill.pointCost;
            unlockedSkills.Add(skill.skillID);
            
            Debug.Log($"Unlocked Skill: {skill.skillName}");
            
            OnSkillPointsChanged?.Invoke(availableSkillPoints);
            OnSkillUnlocked?.Invoke(skill);

            // Optional: Broadcast to the Global Event Bus for Achievements!
            // GameEvents.OnProgressMade?.Invoke("skill_unlocked", 1);
        }
        else
        {
            Debug.LogWarning($"Cannot unlock {skill.skillName}. Missing points or prerequisites.");
        }
    }

    // --- Save & Load Logic ---
    public string SaveState()
    {
        SkillSaveData data = new SkillSaveData
        {
            points = availableSkillPoints,
            unlockedList = new List<string>(unlockedSkills)
        };
        return JsonUtility.ToJson(data);
    }

    public void LoadState(string stateJson)
    {
        SkillSaveData data = JsonUtility.FromJson<SkillSaveData>(stateJson);
        
        availableSkillPoints = data.points;
        unlockedSkills = new HashSet<string>(data.unlockedList);
        
        OnSkillPointsChanged?.Invoke(availableSkillPoints);
    }
}
