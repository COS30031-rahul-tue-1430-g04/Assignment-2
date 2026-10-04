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
	public int score = 0;
	public int completedAssets = 0;
	public int totalAssets = 8;
	public int currentLevel = 1;


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

		// Update HUD
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

		// Show interaction panel
		if (interactionPanel != null)
		{
			interactionPanel.SetActive(true);
		}

		// Set asset name
		if (assetNameText != null)
		{
			assetNameText.text = asset.assetName;
		}

		// Set question
		if (questionText != null)
		{
			questionText.text =
				"Who generally looks after this?";
		}

		Debug.Log("Question opened for: " + asset.assetName);
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
		// Make sure an asset is selected
		if (currentAsset == null)
		{
			Debug.LogWarning("No asset is currently selected.");
			return;
		}

		// Check answer
		bool correct =
			councilAnswer == currentAsset.councilOwned;


		// -----------------------------------------------------
		// Hide question panel
		// -----------------------------------------------------

		if (interactionPanel != null)
		{
			interactionPanel.SetActive(false);
		}


		// -----------------------------------------------------
		// Show feedback panel
		// -----------------------------------------------------

		if (feedbackPanel != null)
		{
			feedbackPanel.SetActive(true);
		}


		// -----------------------------------------------------
		// CORRECT ANSWER
		// -----------------------------------------------------

		if (correct)
		{
			Debug.Log("Correct!");

			score += 100;

			if (feedbackText != null)
			{
				feedbackText.text = "CORRECT!";
			}
		}


		// -----------------------------------------------------
		// WRONG ANSWER
		// -----------------------------------------------------

		else
		{
			Debug.Log("Incorrect!");

			score -= 50;

			if (feedbackText != null)
			{
				feedbackText.text = "NOT QUITE";
			}
		}


		// -----------------------------------------------------
		// EVERY ANSWER COUNTS AS COMPLETED
		// -----------------------------------------------------

		completedAssets++;


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
			"Assets completed: " +
			completedAssets +
			"/" +
			totalAssets
		);

		Debug.Log(
			"Current score: " +
			score
		);
	}


	// =========================================================
	// CONTINUE AFTER FEEDBACK
	// =========================================================

	public void ContinueAfterFeedback()
	{
		Debug.Log("Continue button pressed.");


		// Hide feedback panel
		if (feedbackPanel != null)
		{
			feedbackPanel.SetActive(false);
		}


		// Clear current asset
		currentAsset = null;


		// -----------------------------------------------------
		// CHECK IF LEVEL IS COMPLETE
		// -----------------------------------------------------

		if (completedAssets >= totalAssets)
		{
			Debug.Log("LEVEL COMPLETE!");

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
		// Score
		if (scoreText != null)
		{
			scoreText.text =
				"Score: " + score;
		}


		// Progress text
		if (progressText != null)
		{
			progressText.text =
				"Assets Found: " +
				completedAssets +
				"/" +
				totalAssets;
		}


		// Level
		if (levelText != null)
		{
			levelText.text =
				"Level " +
				currentLevel;
		}


		// Progress bar
		if (progressBar != null)
		{
			if (totalAssets > 0)
			{
				progressBar.value =
					(float)completedAssets /
					totalAssets;
			}
		}
	}
}