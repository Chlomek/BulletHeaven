using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XPSystem : MonoBehaviour
{
    public int xp;
    public int level = 1;
    public float xpForLevelUp = 1.5f;
    public float xpNeededForNextLevel;

    // Start is called before the first frame update
    void Start()
    {
        xp = 0;
        xpNeededForNextLevel = 10;
    }

    // Update is called once per frame
    void Update()
    {
        if(xp >= xpNeededForNextLevel)
        {
            LevelUp();
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
}
