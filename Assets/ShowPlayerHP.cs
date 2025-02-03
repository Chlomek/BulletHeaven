using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShowPlayerHP : MonoBehaviour
{
    public TMP_Text hpshow;
    //public TextMeshProUGUI hpshow;
    private GameObject player;
    private int playerHP;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        playerHP = player.GetComponent<Health>().health;
        hpshow.text = playerHP.ToString() + " HP";
    }
}
