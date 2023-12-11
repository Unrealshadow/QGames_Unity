using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameOverLoop : MonoBehaviour
{
    public StageLoop stageLoop;
    public Transform uiGameOver;
    public Text scoreText;

    public TitleLoop titleLoop;

    public void StartGameOverLoop(int finalScore)
    {
        uiGameOver.gameObject.SetActive(true);
        scoreText.text = $"Final Score: {finalScore:00000}";
        StartCoroutine(GameOverCoroutine());
    }

    private IEnumerator GameOverCoroutine()
    {
        Debug.Log("Start GameOverCoroutine");

        while (true)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                uiGameOver.gameObject.SetActive(false);
                stageLoop.CleanupStage();
                titleLoop.StartTitleLoop(); // Return to title screen
                yield break;
            }
            yield return null;
        }
    }
}