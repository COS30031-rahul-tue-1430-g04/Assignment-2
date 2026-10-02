using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
	[Header("UI")]
	public GameObject howToPlayPanel;

	public void PlayGame()
	{
		Time.timeScale = 1f;
		SceneManager.LoadScene(1);
	}

	public void OpenHowToPlay()
	{
		if (howToPlayPanel != null)
			howToPlayPanel.SetActive(true);
	}

	public void CloseHowToPlay()
	{
		if (howToPlayPanel != null)
			howToPlayPanel.SetActive(false);
	}

	public void QuitGame()
	{
		Debug.Log("Quit Game");

		Application.Quit();
	}
}