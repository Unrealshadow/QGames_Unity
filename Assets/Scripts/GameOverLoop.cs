using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameOverLoop : MonoBehaviour
{
    public Transform uiGameOver;
    public Text scoreText;

    public TitleLoop titleLoop;
    private const string HighScoreKey = "HighScore";

    public void StartGameOverLoop(int finalScore)
    {
        uiGameOver.gameObject.SetActive(true);

        // Load the previous high score
        int highScore = PlayerPrefs.GetInt(HighScoreKey, 0);

        // Check if the current score is greater than the high score
        if (finalScore > highScore)
        {
            // Update the high score
            PlayerPrefs.SetInt(HighScoreKey, finalScore);
            highScore = finalScore;
        }

        // Update the UI
        scoreText.text = $"High Score: {highScore:00000}";

        StartCoroutine(GameOverCoroutine());
    }


    private IEnumerator GameOverCoroutine()
    {
        Debug.Log("Start GameOverCoroutine");
        while (true)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                StageLoop.Instance.CleanupStage();

                uiGameOver.gameObject.SetActive(false);
                titleLoop.StartTitleLoop(); // Return to title screen
                yield break;
            }
            yield return null;
        }
    }
}