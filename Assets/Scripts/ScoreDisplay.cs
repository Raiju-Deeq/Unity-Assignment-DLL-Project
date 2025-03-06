using UnityEngine;
using UnityEngine.UI;

public class ScoreDisplay : MonoBehaviour
{
    private Text scoreText;
    private Player player;

    private void Start()
    {
        scoreText = GetComponent<Text>();
        if (scoreText == null)
        {
            Debug.LogError("Text component not found on this GameObject");
        }

        player = FindObjectOfType<Player>();
        if (player != null)
        {
            player.OnScoreChanged += UpdateScoreDisplay;
            UpdateScoreDisplay(player.GetScore());
        }
        else
        {
            Debug.LogError("Player not found in the scene!");
        }
    }

    private void UpdateScoreDisplay(int newScore)
    {
        if (scoreText != null)
        {
            scoreText.text = $"Score: {newScore}";
            Debug.Log($"Score updated: {newScore}");
        }
        else
        {
            Debug.LogError("ScoreText is null");
        }
    }

    private void OnDestroy()
    {
        if (player != null)
        {
            player.OnScoreChanged -= UpdateScoreDisplay;
        }
    }
}