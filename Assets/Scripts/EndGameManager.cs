using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class EndGameManager : MonoBehaviour
{
	[Header("Final Score")]
	public TMP_Text finalScoreText;

	private void Start()
	{
		Time.timeScale = 1f;

		if (finalScoreText != null)
		{
			finalScoreText.text =
				"Final Score: " + GameManager.Instance.score;
		}
	}

	public void PlayAgain()
	{
		Time.timeScale = 1f;

		SceneManager.LoadScene("Level1");
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