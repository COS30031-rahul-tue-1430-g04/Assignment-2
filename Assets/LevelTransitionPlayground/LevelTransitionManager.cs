using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using System.Collections;

public class LevelTransitionManager : MonoBehaviour
{
	[Header("Level Complete UI")]
	public GameObject levelCompletePanel;
	public CanvasGroup backgroundOverlay;
	public RectTransform panelTransform;

	[Header("Sound")]
	public AudioSource audioSource;
	public AudioClip levelCompleteClip;

	[Header("Fade Settings")]
	public float fadeDuration = 0.5f;

	[Header("End Game")]
	public string endGameSceneName = "EndGame";

	private bool levelCompleteShown = false;
	private bool isTransitioning = false;


	// =========================================================
	// START
	// =========================================================

	private void Start()
	{
		if (levelCompletePanel != null)
			levelCompletePanel.SetActive(false);

		// Start scene dark
		if (backgroundOverlay != null)
		{
			backgroundOverlay.alpha = 1f;

			StartCoroutine(FadeInFromBlack());
		}
	}


	// =========================================================
	// UPDATE
	// =========================================================

	private void Update()
	{
		// TEST KEY
		// Press L to show Level Complete
		if (Keyboard.current != null &&
			Keyboard.current.lKey.wasPressedThisFrame &&
			!levelCompleteShown &&
			!isTransitioning)
		{
			ShowLevelComplete();
		}
	}


	// =========================================================
	// FADE IN
	// =========================================================

	private IEnumerator FadeInFromBlack()
	{
		float time = 0f;

		while (time < fadeDuration)
		{
			time += Time.deltaTime;

			float progress =
				time / fadeDuration;

			if (backgroundOverlay != null)
			{
				backgroundOverlay.alpha =
					Mathf.Lerp(1f, 0f, progress);
			}

			yield return null;
		}

		if (backgroundOverlay != null)
			backgroundOverlay.alpha = 0f;
	}


	// =========================================================
	// SHOW LEVEL COMPLETE
	// =========================================================

	public void ShowLevelComplete()
	{
		if (levelCompleteShown)
			return;

		if (isTransitioning)
			return;

		levelCompleteShown = true;

		if (levelCompletePanel != null)
			levelCompletePanel.SetActive(true);

		if (panelTransform != null)
			panelTransform.localScale = Vector3.zero;

		// Play level complete sound
		if (audioSource != null &&
			levelCompleteClip != null)
		{
			audioSource.PlayOneShot(levelCompleteClip);
		}

		StartCoroutine(AnimateLevelComplete());
	}


	// =========================================================
	// LEVEL COMPLETE ANIMATION
	// =========================================================

	private IEnumerator AnimateLevelComplete()
	{
		// Keep this at 1.5 seconds
		// because it is aligned with your soundtrack.
		float duration = 1.5f;

		float time = 0f;

		while (time < duration)
		{
			time += Time.deltaTime;

			float progress =
				time / duration;


			// Darken the map
			if (backgroundOverlay != null)
			{
				backgroundOverlay.alpha =
					Mathf.Lerp(
						0f,
						0.75f,
						progress
					);
			}


			// Animate panel
			if (panelTransform != null)
			{
				float scale =
					Mathf.Lerp(
						0.8f,
						1f,
						progress
					);

				panelTransform.localScale =
					new Vector3(
						scale,
						scale,
						1f
					);
			}

			yield return null;
		}


		if (backgroundOverlay != null)
			backgroundOverlay.alpha = 0.75f;

		if (panelTransform != null)
			panelTransform.localScale =
				Vector3.one;
	}


	// =========================================================
	// RESTART CURRENT LEVEL
	// =========================================================

	public void RestartLevel()
	{
		if (isTransitioning)
			return;

		isTransitioning = true;

		int currentSceneIndex =
			SceneManager.GetActiveScene().buildIndex;

		StartCoroutine(
			FadeAndLoad(currentSceneIndex)
		);
	}


	// =========================================================
	// LOAD NEXT LEVEL
	// =========================================================

	public void LoadNextLevel()
	{
		if (isTransitioning)
			return;

		int currentSceneIndex =
			SceneManager.GetActiveScene().buildIndex;

		int nextSceneIndex =
			currentSceneIndex + 1;

		string nextSceneName =
			SceneUtility.GetScenePathByBuildIndex(nextSceneIndex);

		// If the next scene is EndGame
		if (nextSceneName.Contains(endGameSceneName))
		{
			Debug.Log("Final level completed. Loading EndGame.");

			isTransitioning = true;

			StartCoroutine(FadeAndLoadEndGame());

			return;
		}

		// Otherwise load the next level
		if (nextSceneIndex <
			SceneManager.sceneCountInBuildSettings)
		{
			isTransitioning = true;

			StartCoroutine(
				FadeAndLoad(nextSceneIndex)
			);
		}
	}


	// =========================================================
	// LOAD END GAME
	// =========================================================

	private IEnumerator FadeAndLoadEndGame()
	{
		float startAlpha = 0f;

		if (backgroundOverlay != null)
			startAlpha = backgroundOverlay.alpha;

		float time = 0f;

		while (time < fadeDuration)
		{
			time += Time.deltaTime;

			float progress =
				time / fadeDuration;

			if (backgroundOverlay != null)
			{
				backgroundOverlay.alpha =
					Mathf.Lerp(
						startAlpha,
						1f,
						progress
					);
			}

			yield return null;
		}

		if (backgroundOverlay != null)
			backgroundOverlay.alpha = 1f;

		Debug.Log(
			"Loading EndGame scene."
		);

		SceneManager.LoadScene(
			endGameSceneName
		);
	}


	// =========================================================
	// FADE AND LOAD NORMAL LEVEL
	// =========================================================

	private IEnumerator FadeAndLoad(
		int sceneIndex)
	{
		float startAlpha = 0f;

		if (backgroundOverlay != null)
			startAlpha = backgroundOverlay.alpha;

		float time = 0f;

		while (time < fadeDuration)
		{
			time += Time.deltaTime;

			float progress =
				time / fadeDuration;

			if (backgroundOverlay != null)
			{
				backgroundOverlay.alpha =
					Mathf.Lerp(
						startAlpha,
						1f,
						progress
					);
			}

			yield return null;
		}

		if (backgroundOverlay != null)
			backgroundOverlay.alpha = 1f;

		SceneManager.LoadScene(
			sceneIndex
		);
	}
}