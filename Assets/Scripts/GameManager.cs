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

	[HideInInspector]
	public int totalAssets = 0;

	public int currentLevel = 1;


	// =========================================================
	// CURRENT LEVEL STATS
	// =========================================================

	[Header("Current Level Stats")]

	public int assetsExplored = 0;

	public int assetsAnswered = 0;


	// =========================================================
	// TOTAL GAME STATS
	// These persist between levels
	// =========================================================

	[Header("Total Game Stats")]

	public static int totalScore = 0;

	public static int totalAssetsExplored = 0;

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

	[Tooltip("The Image component that provides the visible background of the feedback panel.")]
	public Image feedbackBackground;

	[Tooltip("Background colour shown when the answer is correct.")]
	public Color correctBackgroundColor =
		new Color(0.75f, 0.90f, 0.75f);

	[Tooltip("Background colour shown when the answer is incorrect.")]
	public Color incorrectBackgroundColor =
		new Color(0.95f, 0.75f, 0.75f);


	// =========================================================
	// HUD
	// =========================================================

	[Header("HUD")]

	public TMP_Text scoreText;

	public TMP_Text progressText;

	public TMP_Text levelText;

	public Slider progressBar;


	// =========================================================
	// PLAYER
	// =========================================================

	[Header("Player")]

	public PlayerController playerController;


	// =========================================================
	// PRIVATE VARIABLES
	// =========================================================

	private AssetInteraction currentAsset;

	private LevelTransitionManager levelManager;


	// =========================================================
	// RESET GAME DATA
	// Runs when a new Unity Play session starts
	// =========================================================

	[RuntimeInitializeOnLoadMethod(
		RuntimeInitializeLoadType.SubsystemRegistration
	)]

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

		// Automatically count assets in this level
		CountAssets();

		// Automatically find player if not assigned
		if (playerController == null)
		{
			playerController =
				FindFirstObjectByType<PlayerController>();
		}
	}


	// =========================================================
	// COUNT ASSETS
	// Automatically counts AssetInteraction components
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
		// Hide interaction panel
		if (interactionPanel != null)
		{
			interactionPanel.SetActive(false);
		}

		// Hide feedback panel
		if (feedbackPanel != null)
		{
			feedbackPanel.SetActive(false);
		}

		// Hide level complete panel
		if (levelCompletePanel != null)
		{
			levelCompletePanel.SetActive(false);
		}

		// Make sure player can move at the beginning
		if (playerController != null)
		{
			playerController.EnableMovement();
		}

		// Update HUD
		UpdateHUD();
	}


	// =========================================================
	// OPEN ASSET QUESTION
	// Called when player interacts with an asset
	// =========================================================

	public void OpenAssetQuestion(
		AssetInteraction asset
	)
	{
		if (asset == null)
		{
			Debug.LogWarning(
				"No asset was provided."
			);

			return;
		}

		// Store current asset
		currentAsset = asset;


		// -----------------------------------------------------
		// STOP PLAYER MOVEMENT
		// -----------------------------------------------------

		if (playerController != null)
		{
			playerController.DisableMovement();
		}


		// -----------------------------------------------------
		// UPDATE EXPLORED COUNTERS
		// -----------------------------------------------------

		assetsExplored++;

		totalAssetsExplored++;


		// -----------------------------------------------------
		// UPDATE HUD
		// -----------------------------------------------------

		UpdateHUD();


		// -----------------------------------------------------
		// SHOW INTERACTION PANEL
		// -----------------------------------------------------

		if (interactionPanel != null)
		{
			interactionPanel.SetActive(true);
		}


		// -----------------------------------------------------
		// DISPLAY ASSET NAME
		// -----------------------------------------------------

		if (assetNameText != null)
		{
			assetNameText.text =
				asset.assetName;
		}


		// -----------------------------------------------------
		// DISPLAY QUESTION
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
	// COUNCIL ANSWER
	// =========================================================

	public void ChooseCouncil()
	{
		CheckAnswer(true);
	}


	// =========================================================
	// PRIVATE ANSWER
	// =========================================================

	public void ChoosePrivate()
	{
		CheckAnswer(false);
	}


	// =========================================================
	// CHECK ANSWER
	// =========================================================

	private void CheckAnswer(
		bool councilAnswer
	)
	{
		if (currentAsset == null)
		{
			Debug.LogWarning(
				"No asset is currently selected."
			);

			return;
		}


		// Determine whether answer is correct
		bool correct =
			councilAnswer ==
			currentAsset.councilOwned;


		// -----------------------------------------------------
		// CLOSE QUESTION PANEL
		// -----------------------------------------------------

		if (interactionPanel != null)
		{
			interactionPanel.SetActive(false);
		}


		// -----------------------------------------------------
		// SHOW FEEDBACK PANEL
		// -----------------------------------------------------

		if (feedbackPanel != null)
		{
			feedbackPanel.SetActive(true);
		}


		// =====================================================
		// CORRECT ANSWER
		// =====================================================

		if (correct)
		{
			Debug.Log("Correct!");


			// Add 100 points
			totalScore += 100;


			// Correct answers count
			assetsAnswered++;

			totalAssetsAnswered++;


			// Feedback text
			if (feedbackText != null)
			{
				feedbackText.text =
					"CORRECT!";
			}


			// Green background
			if (feedbackBackground != null)
			{
				feedbackBackground.color =
					correctBackgroundColor;
			}
		}


		// =====================================================
		// INCORRECT ANSWER
		// =====================================================

		else
		{
			Debug.Log("Incorrect!");


			// Remove 50 points
			totalScore -= 50;


			// DO NOT increase assetsAnswered
			// because the answer was incorrect


			// Feedback text
			if (feedbackText != null)
			{
				feedbackText.text =
					"NOT QUITE";
			}


			// Red background
			if (feedbackBackground != null)
			{
				feedbackBackground.color =
					incorrectBackgroundColor;
			}
		}


		// -----------------------------------------------------
		// DISPLAY EXPLANATION
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
		// DEBUG INFORMATION
		// -----------------------------------------------------

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


		// -----------------------------------------------------
		// HIDE FEEDBACK PANEL
		// -----------------------------------------------------

		if (feedbackPanel != null)
		{
			feedbackPanel.SetActive(false);
		}


		// -----------------------------------------------------
		// ENABLE PLAYER MOVEMENT
		// -----------------------------------------------------

		if (playerController != null)
		{
			playerController.EnableMovement();
		}


		// Clear current asset
		currentAsset = null;


		// -----------------------------------------------------
		// CHECK LEVEL COMPLETION
		// -----------------------------------------------------

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
		// PROGRESS TEXT
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
		// LEVEL TEXT
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