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

		// Display final score
		if (finalScoreText != null)
		{
			finalScoreText.text =
				"Final Score: " +
				GameManager.totalScore;
		}

		// Display total assets explored
		if (finalExploredText != null)
		{
			finalExploredText.text =
				"Assets Explored: " +
				GameManager.totalAssetsExplored;
		}

		// Display total assets answered correctly
		if (finalAnsweredText != null)
		{
			finalAnsweredText.text =
				"Assets Answered: " +
				GameManager.totalAssetsAnswered;
		}
	}


	// =========================================================
	// PLAY AGAIN
	// =========================================================

	public void PlayAgain()
	{
		Time.timeScale = 1f;

		// Reset total game statistics
		GameManager.totalScore = 0;
		GameManager.totalAssetsExplored = 0;
		GameManager.totalAssetsAnswered = 0;

		// Load first level
		SceneManager.LoadScene(1);
	}


	// =========================================================
	// MAIN MENU
	// =========================================================

	public void MainMenu()
	{
		Time.timeScale = 1f;

		SceneManager.LoadScene(0);
	}


	// =========================================================
	// QUIT
	// =========================================================

	public void QuitGame()
	{
		Debug.Log("Quit Game");

		Application.Quit();
	}
}