// 3. SkillNodeUI.cs
using UnityEngine;
using UnityEngine.UI;


// Attach this to a Canvas Button to visually represent a specific skill.
[RequireComponent(typeof(Button))]
public class SkillNodeUI : MonoBehaviour
{
    [Header("Skill Setup")]
    public SkillData skill;
    
    [Header("Visuals")]
    public Image skillIconImage;
    public Color lockedColor = Color.gray;
    public Color availableColor = Color.white;
    public Color unlockedColor = Color.yellow;

    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnNodeClicked);
        
        if (skill != null && skillIconImage != null)
        {
            skillIconImage.sprite = skill.icon;
        }
    }

    private void OnEnable()
    {
        // Subscribe to updates so the UI refreshes when points change or a skill is bought
        SkillManager.OnSkillUnlocked += RefreshUI;
        SkillManager.OnSkillPointsChanged += RefreshUI;
    }

    private void OnDisable()
    {
        SkillManager.OnSkillUnlocked -= RefreshUI;
        SkillManager.OnSkillPointsChanged -= RefreshUI;
    }

    private void Start()
    {
        RefreshUI();
    }

    private void RefreshUI(SkillData dummyData = null)
    {
        RefreshUI();
    }

    private void RefreshUI(int dummyPoints)
    {
        RefreshUI();
    }

    public void RefreshUI()
    {
        if (SkillManager.Instance == null || skill == null) return;

        if (SkillManager.Instance.IsUnlocked(skill))
        {
            // Already owned
            skillIconImage.color = unlockedColor;
            button.interactable = false; 
        }
        else if (SkillManager.Instance.CanUnlock(skill))
        {
            // Can afford it and have prerequisites
            skillIconImage.color = availableColor;
            button.interactable = true;
        }
        else
        {
            // Missing prerequisites or points
            skillIconImage.color = lockedColor;
            button.interactable = false;
        }
    }

    private void OnNodeClicked()
    {
        SkillManager.Instance.AttemptUnlock(skill);
    }
}
