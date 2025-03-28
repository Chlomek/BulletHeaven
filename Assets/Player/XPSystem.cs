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
    public int shotgunLvl = 0;
    public int granadeLvl = 0;

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
    public AbilityInfo shotgunInfo;
    public AbilityInfo granadeInfo;

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
        if (xp >= xpNeededForNextLevel)
        {
            LevelUp();
        }
        UpdateUI();

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

        if (shotgunLvl > 0)
            availableAbilities.Add("Shotgun");

        if (granadeLvl > 0)
            availableAbilities.Add("Granade");
    }

    void LevelUp()
    {
        level++;
        xp -= (int)xpNeededForNextLevel;
        xpNeededForNextLevel = Mathf.Round(xpMultiplierForNextLevel * xpNeededForNextLevel);
        if (xpNeededForNextLevel > 50)
            xpNeededForNextLevel = 50;
        availableUpgradePoints++;

        ShowUpgradeOptions();

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

            List<string> abilityOptions = new List<string>(availableAbilities);

            if (pistolLvl >= pistolInfo.maxLevel && abilityOptions.Contains("Pistol"))
                abilityOptions.Remove("Pistol");

            if (rpgLvl >= rpgInfo.maxLevel && abilityOptions.Contains("RPG"))
                abilityOptions.Remove("RPG");

            if (garlicLvl >= garlicInfo.maxLevel && abilityOptions.Contains("Garlic"))
                abilityOptions.Remove("Garlic");

            if (laserLvl >= laserInfo.maxLevel && abilityOptions.Contains("Laser"))
                abilityOptions.Remove("Laser");

            if (laserLvl >= laserInfo.maxLevel && abilityOptions.Contains("Shotgun"))
                abilityOptions.Remove("Shotgun");

            if (laserLvl >= laserInfo.maxLevel && abilityOptions.Contains("Granade"))
                abilityOptions.Remove("Granade");

            if (!availableAbilities.Contains("RPG"))
                abilityOptions.Add("RPG");

            if (!availableAbilities.Contains("Garlic"))
                abilityOptions.Add("Garlic");

            if (!availableAbilities.Contains("Laser"))
                abilityOptions.Add("Laser");

            if (!availableAbilities.Contains("Shotgun"))
                abilityOptions.Add("Shotgun");

            if (!availableAbilities.Contains("Granade"))
                abilityOptions.Add("Granade");

            while (abilityOptions.Count < upgradeButtons.Length && abilityOptions.Count > 0)
            {
                abilityOptions.Add(abilityOptions[Random.Range(0, abilityOptions.Count)]);
            }

            if (abilityOptions.Count != 0)
            {
                for (int i = 0; i < abilityOptions.Count; i++)
                {
                    string temp = abilityOptions[i];
                    int randomIndex = Random.Range(i, abilityOptions.Count);
                    abilityOptions[i] = abilityOptions[randomIndex];
                    abilityOptions[randomIndex] = temp;
                }

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
    }

    void SetButtonAbility(int buttonIndex, string abilityName)
    {
        upgradeButtons[buttonIndex].name = "Button_" + abilityName;

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

            case "Shotgun":
                if (shotgunLvl == 0)
                    buttonTexts[buttonIndex].text = "Unlock Shotgun\nShoots multiple projectiles";
                else
                    buttonTexts[buttonIndex].text = $"Shotgun Lvl {shotgunLvl + 1}\n{GetShotgunUpgradeDescription(shotgunLvl)}";

                if (buttonIcons[buttonIndex] != null && shotgunInfo.icon != null)
                    buttonIcons[buttonIndex].sprite = shotgunInfo.icon;
                break;

            case "Granade":
                if (granadeLvl == 0)
                    buttonTexts[buttonIndex].text = "Unlock Granade Launcher\nRandomly shoots at nearby enemy";
                else
                    buttonTexts[buttonIndex].text = $"Granade Launcher Lvl {granadeLvl + 1}\n{GetGranadeUpgradeDescription(granadeLvl)}";

                if (buttonIcons[buttonIndex] != null && granadeInfo.icon != null)
                    buttonIcons[buttonIndex].sprite = granadeInfo.icon;
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
        else if (buttonName.Contains("Shotgun"))
        {
            if (shotgunLvl == 0)
            {
                UnlockAbility("Shotgun");
            }
            else
            {
                shotgunLvl++;
            }
            Debug.Log("Shotgun upgraded to level " + shotgunLvl);
        }
        else if (buttonName.Contains("Granade"))
        {
            if (granadeLvl == 0)
            {
                UnlockAbility("Granade");
            }
            else
            {
                granadeLvl++;
            }
            Debug.Log("Granade upgraded to level " + granadeLvl);
        }

        availableUpgradePoints--;

        if (upgradePanel != null)
            upgradePanel.SetActive(false);

        Time.timeScale = 1f;

        if (availableUpgradePoints > 0)
        {
            Time.timeScale = 0f;
            ShowUpgradeOptions();
        }
    }

    void UpdateUI()
    {
        if (levelText != null)
            levelText.text = "Level: " + level.ToString();

        if (xpText != null)
            xpText.text = xp + "/" + xpNeededForNextLevel;

        if (xpFillBar != null)
        {
            xpSlider.maxValue = xpNeededForNextLevel;
            xpSlider.value = xp;
        }
    }

    string GetPistolUpgradeDescription(int currentLevel)
    {
        switch (currentLevel + 1)
        {
            case 1: return "Fire rate +25%";
            case 2: return "Damage +20%";
            case 3: return "Fire rate +33%";
            case 4: return "Damage +25%";
            case 5: return "Bullet speed +20%";
            case 6: return "Evolution: Machine Gun";
            default: return $"Fire rate increased (1/{currentLevel})";
        }
    }

    string GetRPGUpgradeDescription(int currentLevel)
    {
        switch (currentLevel + 1)
        {
            case 0: return "New weapon: Rocket Launcher (locked)";
            case 1: return "Base damage 15, fire rate 2s";
            case 2: return "Splash radius +20%";
            case 3: return "Damage +20%";
            case 4: return "Fire rate +15%";
            case 5: return "Splash radius +25%";
            default: return "Overall splash radius +25%";
        }
    }

    string GetGarlicUpgradeDescription(int currentLevel)
    {
        switch (currentLevel + 1)
        {
            case 0: return "New ability: Damage nearby enemies (locked)";
            case 1: return "Base damage 4, AoE 2.5m";
            case 2: return "Area of effect +20%";
            case 3: return "Damage +75%";
            case 4: return "Tick rate increased (faster damage over time)";
            case 5: return "Area of effect +25% & Damage +50%";
            default: return "Overall damage +20%";
        }
    }

    string GetLaserUpgradeDescription(int currentLevel)
    {
        switch (currentLevel + 1)
        {
            case 0: return "New weapon: Laser Beam (locked)";
            case 1: return "Base damage 15, fire rate 2s";
            case 2: return "Faster fire rate & damage boost";
            case 3: return "Damage +33%";
            case 4: return "Increased range";
            case 5: return "Fire rate +12% & range +25%";
            case 6: return "Evolution: Plasma Beam";
            default: return "Overall fire rate & damage improvement";
        }
    }

    string GetShotgunUpgradeDescription(int currentLevel)
    {
        switch (currentLevel + 1)
        {
            case 0: return "New weapon: Shotgun (locked)";
            case 1: return "Fire rate 1.5s, 6 pellets";
            case 2: return "Fire rate +10% & +1 pellet";
            case 3: return "Damage +20% & tighter spread";
            case 4: return "Fire rate +10%, velocity +10%";
            case 5: return "Damage +20%, +1 pellet";
            case 6: return "Evolution: Combat Shotgun";
            default: return "Further improved fire rate & pellet count";
        }
    }

    string GetGranadeUpgradeDescription(int currentLevel)
    {
        switch (currentLevel + 1)
        {
            case 0: return "New weapon: Grenade Launcher (locked)";
            case 1: return "Base damage 15, fire rate 2s";
            case 2: return "Splash radius +25%, fire rate +10%";
            case 3: return "Damage +20%, fire rate +12.5%";
            case 4: return "Splash radius +20%, damage +15%";
            case 5: return "Fire rate +15%, splash radius +15%, damage +20%";
            default: return "Further increased damage and fire rate";
        }
    }

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
                case "Shotgun":
                    shotgunLvl = 1;
                    if (player != null)
                    {
                        Transform shotgunTransform = player.transform.Find("Shotgun");
                        if (shotgunTransform != null)
                            shotgunTransform.gameObject.SetActive(true);
                    }
                    break;
                case "Granade":
                    granadeLvl = 1;
                    if (player != null)
                    {
                        Transform granadeTransform = player.transform.Find("GranadeLauncher");
                        if (granadeTransform != null)
                            granadeTransform.gameObject.SetActive(true);
                    }
                    break;
            }

            Debug.Log(abilityName + " unlocked!");
        }
    }
}