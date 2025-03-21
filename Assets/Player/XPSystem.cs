using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class XPSystem : MonoBehaviour
{
    public int xp;
    public int level = 1;
    public float xpMultiplierForNextLevel = 1.5f;
    public float xpNeededForNextLevel;
    public int availableUpgradePoints = 0;

    public int pistolLvl = 1;
    public int rpgLvl = 0;
    public int garlicLvl = 0;
    public int laserLvl = 0;

    [Header("UI References")]
    public GameObject upgradePanel;
    public Button[] upgradeButtons;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI xpText;
    public Slider xpSlider;
    public Image xpFillBar;

    [System.Serializable]
    public class AbilityInfo
    {
        public string abilityName;
        public string description;
        public int maxLevel = 6;
        public Sprite icon;
    }

    [Header("Ability Information")]
    public AbilityInfo pistolInfo;
    public AbilityInfo rpgInfo;
    public AbilityInfo garlicInfo;
    public AbilityInfo laserInfo;

    private List<string> availableAbilities = new List<string>();
    private TextMeshProUGUI[] buttonTexts;
    private Image[] buttonIcons;

    void Start()
    {
        xp = 0;
        xpNeededForNextLevel = 10;

        if (upgradePanel != null)
            upgradePanel.SetActive(false);

        InitializeAbilities();

        buttonTexts = new TextMeshProUGUI[upgradeButtons.Length];
        buttonIcons = new Image[upgradeButtons.Length];

        for (int i = 0; i < upgradeButtons.Length; i++)
        {
            int buttonIndex = i;
            upgradeButtons[i].onClick.AddListener(() => UpgradeSelectedAbility(buttonIndex));

            buttonTexts[i] = upgradeButtons[i].GetComponentInChildren<TextMeshProUGUI>();
            buttonIcons[i] = upgradeButtons[i].transform.Find("AbilityIcon")?.GetComponent<Image>();
        }
        UpdateUI();
    }

    void Update()
    {
        // Check for level up
        if (xp >= xpNeededForNextLevel)
        {
            LevelUp();
        }

        // Update UI elements
        UpdateUI();

        // Debug input to add XP (remove in production)
        if (Input.GetKeyDown(KeyCode.X))
        {
            AddXP(5);
        }
    }

    void InitializeAbilities()
    {
        availableAbilities.Add("Pistol");

        if (rpgLvl > 0)
            availableAbilities.Add("RPG");

        if (garlicLvl > 0)
            availableAbilities.Add("Garlic");

        if (laserLvl > 0)
            availableAbilities.Add("Laser");
    }

    void LevelUp()
    {
        level++;
        xp -= (int)xpNeededForNextLevel;
        xpNeededForNextLevel = Mathf.Round(xpMultiplierForNextLevel * xpNeededForNextLevel);
        availableUpgradePoints++;

        // Show upgrade selection panel
        ShowUpgradeOptions();

        // Optionally pause the game
        Time.timeScale = 0f;
    }

    public void AddXP(int xpGain)
    {
        xp += xpGain;
    }

    void ShowUpgradeOptions()
    {
        if (upgradePanel != null)
        {
            upgradePanel.SetActive(true);

            // Create a list of abilities to choose from
            List<string> abilityOptions = new List<string>(availableAbilities);

            // Remove maxed out abilities
            if (pistolLvl >= pistolInfo.maxLevel && abilityOptions.Contains("Pistol"))
                abilityOptions.Remove("Pistol");

            if (rpgLvl >= rpgInfo.maxLevel && abilityOptions.Contains("RPG"))
                abilityOptions.Remove("RPG");

            if (garlicLvl >= garlicInfo.maxLevel && abilityOptions.Contains("Garlic"))
                abilityOptions.Remove("Garlic");

            if (laserLvl >= laserInfo.maxLevel && abilityOptions.Contains("Laser"))
                abilityOptions.Remove("Laser");

            if (!availableAbilities.Contains("RPG"))
                abilityOptions.Add("RPG");

            if (!availableAbilities.Contains("Garlic"))
                abilityOptions.Add("Garlic");

            if (!availableAbilities.Contains("Laser"))
                abilityOptions.Add("Laser");

            /*
            if (!availableAbilities.Contains("RPG") && level >= 3)
                abilityOptions.Add("RPG");

            if (!availableAbilities.Contains("Garlic") && level >= 5)
                abilityOptions.Add("Garlic");
            */
            // If we have fewer options than buttons, add placeholders or duplicate options
            while (abilityOptions.Count < upgradeButtons.Length && abilityOptions.Count > 0)
            {
                abilityOptions.Add(abilityOptions[Random.Range(0, abilityOptions.Count)]);
            }

            // Shuffle the options
            for (int i = 0; i < abilityOptions.Count; i++)
            {
                string temp = abilityOptions[i];
                int randomIndex = Random.Range(i, abilityOptions.Count);
                abilityOptions[i] = abilityOptions[randomIndex];
                abilityOptions[randomIndex] = temp;
            }

            // Assign options to buttons
            for (int i = 0; i < upgradeButtons.Length; i++)
            {
                if (i < abilityOptions.Count)
                {
                    upgradeButtons[i].gameObject.SetActive(true);
                    SetButtonAbility(i, abilityOptions[i]);
                }
                else
                {
                    upgradeButtons[i].gameObject.SetActive(false);
                }
            }
        }
    }

    void SetButtonAbility(int buttonIndex, string abilityName)
    {
        // Tag the button with the ability name (for retrieval when clicked)
        upgradeButtons[buttonIndex].name = "Button_" + abilityName;

        // Set the button text and icon based on ability
        switch (abilityName)
        {
            case "Pistol":
                buttonTexts[buttonIndex].text = $"Pistol Lvl {pistolLvl + 1}\n{GetPistolUpgradeDescription(pistolLvl)}";
                if (buttonIcons[buttonIndex] != null && pistolInfo.icon != null)
                    buttonIcons[buttonIndex].sprite = pistolInfo.icon;
                break;

            case "RPG":
                if (rpgLvl == 0)
                    buttonTexts[buttonIndex].text = "Unlock Rocket Launcher\nDeals splash damage to enemies";
                else
                    buttonTexts[buttonIndex].text = $"RPG Lvl {rpgLvl + 1}\n{GetRPGUpgradeDescription(rpgLvl)}";

                if (buttonIcons[buttonIndex] != null && rpgInfo.icon != null)
                    buttonIcons[buttonIndex].sprite = rpgInfo.icon;
                break;

            case "Garlic":
                if (garlicLvl == 0)
                    buttonTexts[buttonIndex].text = "Unlock Garlic\nDamages nearby enemies over time";
                else
                    buttonTexts[buttonIndex].text = $"Garlic Lvl {garlicLvl + 1}\n{GetGarlicUpgradeDescription(garlicLvl)}";

                if (buttonIcons[buttonIndex] != null && garlicInfo.icon != null)
                    buttonIcons[buttonIndex].sprite = garlicInfo.icon;
                break;

            case "Laser":
                if (laserLvl == 0)
                    buttonTexts[buttonIndex].text = "Unlock Laser Beam\nPenetrates through enemies in a line";
                else
                    buttonTexts[buttonIndex].text = $"Laser Lvl {laserLvl + 1}\n{GetLaserUpgradeDescription(laserLvl)}";

                if (buttonIcons[buttonIndex] != null && laserInfo.icon != null)
                    buttonIcons[buttonIndex].sprite = laserInfo.icon;
                break;
        }
    }

    void UpgradeSelectedAbility(int buttonIndex)
    {
        string buttonName = upgradeButtons[buttonIndex].name;

        if (buttonName.Contains("Pistol"))
        {
            pistolLvl++;
            Debug.Log("Pistol upgraded to level " + pistolLvl);
        }
        else if (buttonName.Contains("RPG"))
        {
            if (rpgLvl == 0)
            {
                UnlockAbility("RPG");
            }
            else
            {
                rpgLvl++;
            }
            Debug.Log("RPG upgraded to level " + rpgLvl);
        }
        else if (buttonName.Contains("Garlic"))
        {
            if (garlicLvl == 0)
            {
                UnlockAbility("Garlic");
            }
            else
            {
                garlicLvl++;
            }
            Debug.Log("Garlic upgraded to level " + garlicLvl);
        }
        else if (buttonName.Contains("Laser"))
        {
            if (laserLvl == 0)
            {
                UnlockAbility("Laser");
            }
            else
            {
                laserLvl++;
            }
            Debug.Log("Laser upgraded to level " + laserLvl);
        }

            // Reduce available upgrade points
            availableUpgradePoints--;

        // Close the upgrade panel
        if (upgradePanel != null)
            upgradePanel.SetActive(false);

        // Resume game if it was paused
        Time.timeScale = 1f;

        // Check if there are more upgrades available
        if (availableUpgradePoints > 0)
        {
            // Show more upgrade options
            ShowUpgradeOptions();
        }
    }

    void UpdateUI()
    {
        // Update level text
        if (levelText != null)
            levelText.text = "Level: " + level.ToString();

        // Update XP text
        if (xpText != null)
            xpText.text = xp + "/" + xpNeededForNextLevel;

        // Update XP fill bar
        if (xpFillBar != null)
        {
            //xpFillBar.fillAmount = (float)xp / xpNeededForNextLevel;
            xpSlider.maxValue = xpNeededForNextLevel;
            xpSlider.value = xp;
        }
    }

    // Get upgrade descriptions
    string GetPistolUpgradeDescription(int currentLevel)
    {
        switch (currentLevel)
        {
            case 1: return "Fire rate +20%";
            case 2: return "Damage +20%";
            case 3: return "Fire rate +25%";
            case 4: return "Bullet speed +20%";
            case 5: return "Evolution: Machine Gun";
            default: return "Overall damage +10%";
        }
    }

    string GetRPGUpgradeDescription(int currentLevel)
    {
        switch (currentLevel)
        {
            case 0: return "New weapon: Rocket Launcher";
            case 1: return "Splash radius +20%";
            case 2: return "Damage +20%";
            case 3: return "Fire rate +15%";
            case 4: return "Splash radius +25%";
            default: return "Overall damage +15%";
        }
    }

    string GetGarlicUpgradeDescription(int currentLevel)
    {
        switch (currentLevel)
        {
            case 0: return "New ability: Damage nearby enemies";
            case 1: return "Area of effect +20%";
            case 2: return "Damage +25%";
            case 3: return "Tick rate increased";
            case 4: return "Area of effect +25%";
            default: return "Overall damage +20%";
        }
    }

    string GetLaserUpgradeDescription(int currentLevel)
    {
        switch (currentLevel)
        {
            case 0: return "New weapon: Laser Beam";
            case 1: return "Improved fire rate";
            case 2: return "Fire rate and Damage boost";
            case 3: return "More range";
            case 4: return "Faster fire rate";
            case 5: return "Evolution: Plasma Beam";
            default: return "Overall damage +15%";
        }
    }

    // Helper function to unlock a new ability
    public void UnlockAbility(string abilityName)
    {
        if (!availableAbilities.Contains(abilityName))
        {
            availableAbilities.Add(abilityName);

            GameObject player = GameObject.Find("Player");
            switch (abilityName)
            {
                case "RPG":
                    rpgLvl = 1;
                    if (player != null)
                    {
                        Transform rpgTransform = player.transform.Find("Rocket Launcher");
                        if (rpgTransform != null)
                            rpgTransform.gameObject.SetActive(true);
                    }
                    break;
                case "Garlic":
                    garlicLvl = 1;
                    if (player != null)
                    {
                        Transform garlicTransform = player.transform.Find("Garlic");
                        if (garlicTransform != null)
                            garlicTransform.gameObject.SetActive(true);
                    }
                    break;
                case "Laser":
                    laserLvl = 1;
                    if (player != null)
                    {
                        Transform laserTransform = player.transform.Find("DeathBeam");
                        if (laserTransform != null)
                            laserTransform.gameObject.SetActive(true);
                    }
                    break;
            }

            Debug.Log(abilityName + " unlocked!");
        }
    }
}