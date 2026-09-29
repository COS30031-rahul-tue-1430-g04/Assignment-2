using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class PauseManager : MonoBehaviour
{
	[Header("Pause UI")]
	public GameObject pausePanel;

	private bool isPaused = false;

	private void Start()
	{
		// Make sure the game starts unpaused
		Time.timeScale = 1f;

		if (pausePanel != null)
			pausePanel.SetActive(false);
	}

	private void Update()
	{
		// Press ESC to pause/resume
		if (Keyboard.current != null &&
			Keyboard.current.escapeKey.wasPressedThisFrame)
		{
			TogglePause();
		}
	}

	public void TogglePause()
	{
		if (isPaused)
			ResumeGame();
		else
			PauseGame();
	}

	public void PauseGame()
	{
		isPaused = true;

		if (pausePanel != null)
			pausePanel.SetActive(true);

		Time.timeScale = 0f;
	}

	public void ResumeGame()
	{
		isPaused = false;

		if (pausePanel != null)
			pausePanel.SetActive(false);

		Time.timeScale = 1f;
	}

	public void RestartLevel()
	{
		Time.timeScale = 1f;

		SceneManager.LoadScene(
			SceneManager.GetActiveScene().name
		);
	}

	public void MainMenu()
	{
		Time.timeScale = 1f;

		SceneManager.LoadScene("MainMenu");
	}
}