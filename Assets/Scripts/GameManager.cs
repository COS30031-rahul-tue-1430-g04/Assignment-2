using UnityEngine;
using TMPro;
using UnityEngine.UI;

[RequireComponent(typeof(LevelTransitionManager))]
public class GameManager : MonoBehaviour
{
	public static GameManager Instance;

	// =========================================================
	// GAME SETTINGS
	// =========================================================

	[Header("Game Settings")]

	// Number of assets in the CURRENT level
	public int totalAssets = 8;

	// Current level number
	public int currentLevel = 1;


	// =========================================================
	// CURRENT LEVEL STATS
	// =========================================================

	[Header("Current Level Stats")]

	// Assets the player has interacted with
	public int assetsExplored = 0;

	// Assets answered correctly
	public int assetsAnswered = 0;


	// =========================================================
	// TOTAL GAME STATS
	// =========================================================

	[Header("Total Game Stats")]

	// Total score across the game
	public static int totalScore = 0;

	// Total assets explored across all levels
	public static int totalAssetsExplored = 0;

	// Total assets answered correctly across all levels
	public static int totalAssetsAnswered = 0;


	// =========================================================
	// UI PANELS
	// =========================================================

	[Header("UI Panels")]
	public GameObject interactionPanel;
	public GameObject feedbackPanel;
	public GameObject levelCompletePanel;


	// =========================================================
	// INTERACTION UI
	// =========================================================

	[Header("Interaction UI")]
	public TMP_Text assetNameText;
	public TMP_Text questionText;


	// =========================================================
	// FEEDBACK UI
	// =========================================================

	[Header("Feedback UI")]
	public TMP_Text feedbackText;
	public TMP_Text explanationText;


	// =========================================================
	// HUD
	// =========================================================

	[Header("HUD")]
	public TMP_Text scoreText;
	public TMP_Text progressText;
	public TMP_Text levelText;
	public Slider progressBar;


	// =========================================================
	// PRIVATE VARIABLES
	// =========================================================

	private AssetInteraction currentAsset;
	private LevelTransitionManager levelManager;


	// =========================================================
	// AWAKE
	// =========================================================

	private void Awake()
	{
		Instance = this;

		levelManager = GetComponent<LevelTransitionManager>();

		// New level starts with zero current-level stats
		assetsExplored = 0;
		assetsAnswered = 0;
	}
	// =========================================================
	// RESET GAME DATA WHEN A NEW PLAY SESSION STARTS
	// =========================================================

	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
	private static void ResetGameData()
	{
		totalScore = 0;
		totalAssetsExplored = 0;
		totalAssetsAnswered = 0;
	}


	// =========================================================
	// START
	// =========================================================

	private void Start()
	{
		// Hide interaction panel
		if (interactionPanel != null)
			interactionPanel.SetActive(false);

		// Hide feedback panel
		if (feedbackPanel != null)
			feedbackPanel.SetActive(false);

		// Hide level complete panel
		if (levelCompletePanel != null)
			levelCompletePanel.SetActive(false);

		UpdateHUD();
	}


	// =========================================================
	// OPEN ASSET QUESTION
	// =========================================================

	public void OpenAssetQuestion(AssetInteraction asset)
	{
		if (asset == null)
		{
			Debug.LogWarning("No asset was provided.");
			return;
		}

		currentAsset = asset;


		// -----------------------------------------------------
		// ASSET HAS BEEN EXPLORED
		// -----------------------------------------------------

		assetsExplored++;

		totalAssetsExplored++;


		// -----------------------------------------------------
		// UPDATE HUD
		// -----------------------------------------------------

		UpdateHUD();


		// -----------------------------------------------------
		// SHOW QUESTION PANEL
		// -----------------------------------------------------

		if (interactionPanel != null)
			interactionPanel.SetActive(true);


		// -----------------------------------------------------
		// ASSET NAME
		// -----------------------------------------------------

		if (assetNameText != null)
		{
			assetNameText.text = asset.assetName;
		}


		// -----------------------------------------------------
		// QUESTION
		// -----------------------------------------------------

		if (questionText != null)
		{
			questionText.text =
				"Who generally looks after this?";
		}


		Debug.Log(
			"Asset explored: " +
			asset.assetName
		);

		Debug.Log(
			"Assets explored this level: " +
			assetsExplored +
			"/" +
			totalAssets
		);

		Debug.Log(
			"Total assets explored: " +
			totalAssetsExplored
		);
	}


	// =========================================================
	// COUNCIL BUTTON
	// =========================================================

	public void ChooseCouncil()
	{
		CheckAnswer(true);
	}


	// =========================================================
	// PRIVATE BUTTON
	// =========================================================

	public void ChoosePrivate()
	{
		CheckAnswer(false);
	}


	// =========================================================
	// CHECK ANSWER
	// =========================================================

	private void CheckAnswer(bool councilAnswer)
	{
		if (currentAsset == null)
		{
			Debug.LogWarning(
				"No asset is currently selected."
			);

			return;
		}


		// Check whether answer is correct
		bool correct =
			councilAnswer == currentAsset.councilOwned;


		// -----------------------------------------------------
		// HIDE QUESTION PANEL
		// -----------------------------------------------------

		if (interactionPanel != null)
			interactionPanel.SetActive(false);


		// -----------------------------------------------------
		// SHOW FEEDBACK PANEL
		// -----------------------------------------------------

		if (feedbackPanel != null)
			feedbackPanel.SetActive(true);


		// =====================================================
		// CORRECT ANSWER
		// =====================================================

		if (correct)
		{
			Debug.Log("Correct!");

			// Add 100 points
			totalScore += 100;


			// Increase answered count
			assetsAnswered++;

			totalAssetsAnswered++;


			// Feedback
			if (feedbackText != null)
			{
				feedbackText.text = "CORRECT!";
			}
		}


		// =====================================================
		// WRONG ANSWER
		// =====================================================

		else
		{
			Debug.Log("Incorrect!");

			// Remove 50 points
			totalScore -= 50;


			// IMPORTANT:
			// Do NOT increase assetsAnswered.


			if (feedbackText != null)
			{
				feedbackText.text = "NOT QUITE";
			}
		}


		// -----------------------------------------------------
		// SHOW EXPLANATION
		// -----------------------------------------------------

		if (explanationText != null)
		{
			explanationText.text =
				currentAsset.explanation;
		}


		// -----------------------------------------------------
		// UPDATE HUD
		// -----------------------------------------------------

		UpdateHUD();


		// -----------------------------------------------------
		// DEBUG
		// -----------------------------------------------------

		Debug.Log(
			"Assets Explored: " +
			assetsExplored +
			"/" +
			totalAssets
		);

		Debug.Log(
			"Assets Answered Correctly: " +
			assetsAnswered +
			"/" +
			totalAssets
		);

		Debug.Log(
			"Total Score: " +
			totalScore
		);
	}


	// =========================================================
	// CONTINUE AFTER FEEDBACK
	// =========================================================

	public void ContinueAfterFeedback()
	{
		Debug.Log(
			"Continue button pressed."
		);


		// Hide feedback
		if (feedbackPanel != null)
			feedbackPanel.SetActive(false);


		// Clear current asset
		currentAsset = null;


		// -----------------------------------------------------
		// CHECK IF ALL ASSETS HAVE BEEN EXPLORED
		// -----------------------------------------------------

		if (assetsExplored >= totalAssets)
		{
			Debug.Log(
				"ALL ASSETS IN LEVEL " +
				currentLevel +
				" HAVE BEEN EXPLORED."
			);


			if (levelManager != null)
			{
				levelManager.ShowLevelComplete();
			}
			else
			{
				Debug.LogError(
					"LevelTransitionManager is missing!"
				);
			}
		}
	}


	// =========================================================
	// UPDATE HUD
	// =========================================================

	private void UpdateHUD()
	{
		// -----------------------------------------------------
		// SCORE
		// -----------------------------------------------------

		if (scoreText != null)
		{
			scoreText.text =
				"Score: " +
				totalScore;
		}


		// -----------------------------------------------------
		// EXPLORED / ANSWERED
		// -----------------------------------------------------

		if (progressText != null)
		{
			progressText.text =
				"Assets Explored: " +
				assetsExplored +
				"/" +
				totalAssets +
				"\n" +
				"Assets Answered: " +
				assetsAnswered +
				"/" +
				totalAssets;
		}


		// -----------------------------------------------------
		// LEVEL
		// -----------------------------------------------------

		if (levelText != null)
		{
			levelText.text =
				"Level " +
				currentLevel;
		}


		// -----------------------------------------------------
		// PROGRESS BAR
		// -----------------------------------------------------

		if (progressBar != null)
		{
			if (totalAssets > 0)
			{
				progressBar.value =
					(float)assetsExplored /
					totalAssets;
			}
		}
	}


	// =========================================================
	// GETTERS
	// =========================================================

	public int GetAssetsExplored()
	{
		return assetsExplored;
	}


	public int GetAssetsAnswered()
	{
		return assetsAnswered;
	}


	public int GetTotalAssetsExplored()
	{
		return totalAssetsExplored;
	}


	public int GetTotalAssetsAnswered()
	{
		return totalAssetsAnswered;
	}


	public int GetTotalScore()
	{
		return totalScore;
	}


	public int GetCurrentLevel()
	{
		return currentLevel;
	}
}