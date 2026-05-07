# Unity Modular Skill / Tech Tree System

A flexible, data-driven Skill Tree framework for Unity. Perfect for RPGs and strategy games, this system allows designers to map out complex progression webs (Directed Acyclic Graphs) directly in the Unity Editor without hardcoding any paths.

---

## ✨ Features

* **ScriptableObject Nodes:** Define skills, point costs, and prerequisites completely in the inspector.
* **Deep Prerequisite Chains:** Skills can require one, two, or multiple previous skills to be unlocked before they become available.
* **Auto-Validating UI:** Includes a "SkillNodeUI" component that automatically grays out locked skills, lights up available skills, and turns gold when purchased.
* **Built-in Save Integration:** Natively implements the "ISaveable" interface. The manager packs the player's current skill points and unlocked nodes into JSON automatically.
* **Event-Driven Architecture:** Emits static C# Actions when skills are unlocked, allowing your UI overlays and audio managers to react decoupled from the logic.

---

## 🧠 Design Notes

Building a skill tree by hardcoding logic like "if (playerHasDoubleJump && playerHasDash) unlock TripleJump" creates a nightmare scenario when a designer wants to change the layout of the tree later.

By turning "SkillData" into a ScriptableObject, the tree essentially maps itself. The "SkillManager" doesn't need to know what the tree looks like. When the player clicks a node to buy it, the Manager simply looks at the "SkillData.prerequisites" list and cross-references it with a "HashSet" of the player's previously bought skills. This $O(1)$ lookup keeps the system incredibly fast and 100% bug-free, even with hundreds of nodes.

---

## 📂 Included Scripts

* "SkillData.cs" - The ScriptableObject blueprint defining the ID, visual data, cost, and a list of required prerequisite skills.
* "SkillManager.cs" - The core Singleton that handles the wallet of Skill Points, tracks unlocked nodes, and processes save/load logic.
* "SkillNodeUI.cs" - A Canvas Button helper script that automatically listens to the Manager and updates its colors based on unlockability.

---

## 🧩 How To Use

1. **Setup the Manager:** Create an empty GameObject named "SkillManager" and attach the "SkillManager.cs" script.
2. **Create Skills:** Right-click in your Project window: "Create > Skill System > Skill Data". Make a few (e.g., "Fireball", "Meteor").
3. **Link Prerequisites:** Select the "Meteor" skill asset. In its prerequisites list, drag and drop the "Fireball" skill asset.
4. **Setup the UI:** Create UI Buttons in your Canvas. Attach the "SkillNodeUI.cs" script to them, and assign the corresponding "SkillData" asset to each button's inspector slot.
5. **Grant Points:** From an enemy death script or level-up manager, grant the player points:

"""
SkillManager.Instance.AddSkillPoints(1);
"""

6. **Check for Unlocks:** When the player tries to cast a spell, just ask the manager if they know how:

"""
if (SkillManager.Instance.IsUnlocked(meteorSkillData))
{
    CastMeteor();
}
"""

---

## 🚀 Possible Extensions

* **Skill Types:** Add an Enum to "SkillData" (e.g., Active, Passive, StatBoost). Subscribe your Player Controller to "SkillManager.OnSkillUnlocked" to immediately apply health boosts if a StatBoost node is purchased.
* **Refund System:** Add a "ResetSkills()" method to the manager that clears the HashSet, calculates the total cost of all cleared skills, and refunds those points to "availableSkillPoints".
* **UI Line Renderers:** Write a UI script that loops through a skill's prerequisites and draws a Canvas LineRenderer between the buttons to visualize the "branches" of the tree.

---

## 🛠 Unity Version

Tested in Unity6+ (should work without any problems in newer versions).

---

## 📜 License

MIT
