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

	// Automatically calculated from AssetInteraction objects
	[HideInInspector]
	public int totalAssets = 0;

	public int currentLevel = 1;


	// =========================================================
	// CURRENT LEVEL STATS
	// =========================================================

	[Header("Current Level Stats")]

	// Assets the player has explored in this level
	public int assetsExplored = 0;

	// Assets answered correctly in this level
	public int assetsAnswered = 0;


	// =========================================================
	// TOTAL GAME STATS
	// =========================================================

	[Header("Total Game Stats")]

	// Total score across all levels
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
	// RESET STATIC DATA WHEN NEW PLAY SESSION STARTS
	// =========================================================

	[RuntimeInitializeOnLoadMethod(
		RuntimeInitializeLoadType.SubsystemRegistration)]
	private static void ResetGameData()
	{
		totalScore = 0;
		totalAssetsExplored = 0;
		totalAssetsAnswered = 0;
	}


	// =========================================================
	// AWAKE
	// =========================================================

	private void Awake()
	{
		Instance = this;

		levelManager =
			GetComponent<LevelTransitionManager>();

		// Reset current-level statistics
		assetsExplored = 0;
		assetsAnswered = 0;

		// Automatically count assets in this scene
		CountAssets();
	}


	// =========================================================
	// COUNT ASSETS
	// =========================================================

	private void CountAssets()
	{
		AssetInteraction[] assets =
			FindObjectsByType<AssetInteraction>(
				FindObjectsSortMode.None
			);

		totalAssets = assets.Length;

		Debug.Log(
			"Total assets found in Level " +
			currentLevel +
			": " +
			totalAssets
		);
	}


	// =========================================================
	// START
	// =========================================================

	private void Start()
	{
		if (interactionPanel != null)
			interactionPanel.SetActive(false);

		if (feedbackPanel != null)
			feedbackPanel.SetActive(false);

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
			Debug.LogWarning(
				"No asset was provided."
			);

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
			assetNameText.text =
				asset.assetName;
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
			"Assets explored: " +
			assetsExplored +
			"/" +
			totalAssets
		);
	}


	// =========================================================
	// ANSWER BUTTONS
	// =========================================================

	public void ChooseCouncil()
	{
		CheckAnswer(true);
	}


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


		bool correct =
			councilAnswer ==
			currentAsset.councilOwned;


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

			// Increase correctly answered count
			assetsAnswered++;
			totalAssetsAnswered++;

			if (feedbackText != null)
			{
				feedbackText.text =
					"CORRECT!";
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
			// Assets Answered does NOT increase.

			if (feedbackText != null)
			{
				feedbackText.text =
					"NOT QUITE";
			}
		}


		// -----------------------------------------------------
		// EXPLANATION
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


		Debug.Log(
			"Assets Explored: " +
			assetsExplored +
			"/" +
			totalAssets
		);

		Debug.Log(
			"Assets Answered: " +
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


		if (feedbackPanel != null)
			feedbackPanel.SetActive(false);


		currentAsset = null;


		// -----------------------------------------------------
		// LEVEL COMPLETE
		// -----------------------------------------------------

		// The player cannot retry, so level completion is based
		// on every asset having been explored/attempted.

		if (assetsExplored >= totalAssets)
		{
			Debug.Log(
				"ALL " +
				totalAssets +
				" ASSETS HAVE BEEN EXPLORED!"
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
		// ASSETS EXPLORED + ANSWERED
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
			else
			{
				progressBar.value = 0f;
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


	public int GetTotalAssets()
	{
		return totalAssets;
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