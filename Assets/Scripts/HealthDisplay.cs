using UnityEngine;
using UnityEngine.UI;


public class HealthDisplay : MonoBehaviour
{
    private Text healthText;
    private Player player;

    private void Start()
    {
        healthText = GetComponent<Text>();
        if (healthText == null)
        {
            Debug.LogError("Text component not found on this GameObject");
        }

        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        if (player == null)
        {
            Debug.LogError("Player not found in the scene!");
        }
        else
        {
            UpdateHealthDisplay();
        }
    }

    private void Update()
    {
        UpdateHealthDisplay();
    }

    private void UpdateHealthDisplay()
    {
        if (player != null && healthText != null)
        {
            healthText.text = $"Health: {player.currentHealth}";
        }
    }
}