using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class XPSystem : MonoBehaviour
{
    public int xp;
    public int level = 1;
    public float xpForLevelUp = 1.5f;
    public float xpNeededForNextLevel;
    public Button upgradeButton;
    private int usedLevels = 1;

    public int pistolLvl = 1;
    public int rpgLvl = 1;

    // Start is called before the first frame update
    void Start()
    {
        xp = 0;
        xpNeededForNextLevel = 10;
        upgradeButton.gameObject.SetActive(false);
        upgradeButton.onClick.AddListener(UpgradeAbility);
    }

    // Update is called once per frame
    void Update()
    {
        if(xp >= xpNeededForNextLevel)
        {
            LevelUp();
        }

        if (level - usedLevels > 0)
        {
            upgradeButton.gameObject.SetActive(true);
        }
    }

    private void LevelUp()
    {
        xpNeededForNextLevel = Mathf.Round(xpForLevelUp * xpNeededForNextLevel);
        xp = 0;
        level++;     
    }

    public void AddXP(int XPGain)
    {
        xp += XPGain;
    }

    private void UpgradeAbility()
    {

        Debug.Log("Pistol Upgraded!");
        pistolLvl++;
        usedLevels++;
        upgradeButton.gameObject.SetActive(false);
    }
}
