using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class EndGameManager : MonoBehaviour
{
	[Header("Final Game UI")]
	public TMP_Text finalScoreText;
	public TMP_Text finalExploredText;
	public TMP_Text finalAnsweredText;

	private void Start()
	{
		Time.timeScale = 1f;

		// Display final statistics
		if (finalScoreText != null)
		{
			finalScoreText.text =
				"FINAL SCORE: " +
				GameManager.totalScore;
		}

		if (finalExploredText != null)
		{
			finalExploredText.text =
				"ASSETS EXPLORED: " +
				GameManager.totalAssetsExplored;
		}

		if (finalAnsweredText != null)
		{
			finalAnsweredText.text =
				"ASSETS ANSWERED CORRECTLY: " +
				GameManager.totalAssetsAnswered;
		}

		Debug.Log("===== FINAL GAME STATS =====");
		Debug.Log("Final Score: " + GameManager.totalScore);
		Debug.Log("Total Assets Explored: " + GameManager.totalAssetsExplored);
		Debug.Log("Total Assets Answered: " + GameManager.totalAssetsAnswered);
	}

	public void PlayAgain()
	{
		Time.timeScale = 1f;

		// Reset all game statistics
		GameManager.totalScore = 0;
		GameManager.totalAssetsExplored = 0;
		GameManager.totalAssetsAnswered = 0;

		SceneManager.LoadScene("Rural Level");
	}

	public void MainMenu()
	{
		Time.timeScale = 1f;

		SceneManager.LoadScene("MainMenu");
	}

	public void QuitGame()
	{
		Debug.Log("Quit Game");
		Application.Quit();
	}
}