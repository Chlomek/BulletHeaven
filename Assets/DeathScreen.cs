using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DeathScreenManager : MonoBehaviour
{
    public GameObject timeDisplayObject;
    public TextMeshProUGUI deathScreenText; 
    public GameObject deathScreenUI;

    private void Start()
    {
        deathScreenUI.SetActive(false);
    }

    public void ShowDeathScreen()
    {
        if (timeDisplayObject != null)
        {
            TextMeshProUGUI timeText = timeDisplayObject.GetComponent<TextMeshProUGUI>();
            if (timeText != null)
            {
                deathScreenText.text = "You survived for: " + timeText.text;
            }
            else
            {
                Debug.LogError("No Text component found on timeDisplayObject!");
            }
        }
        else
        {
            Debug.LogError("Time display object is not assigned!");
        }

        Time.timeScale = 0f;
        deathScreenUI.SetActive(true);
    }
}