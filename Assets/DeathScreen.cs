using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DeathScreenManager : MonoBehaviour
{
    public GameObject timeDisplayObject; // Assign the GameObject that holds the formatted time
    public TextMeshProUGUI deathScreenText; 
    public GameObject deathScreenUI; // Assign the Death Screen UI Panel

    private void Start()
    {
        deathScreenUI.SetActive(false); // Hide the death screen initially
    }

    public void ShowDeathScreen()
    {
        if (timeDisplayObject != null)
        {
            TextMeshProUGUI timeText = timeDisplayObject.GetComponent<TextMeshProUGUI>(); // Get the TMP component
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
        deathScreenUI.SetActive(true); // Show the death screen
    }
}