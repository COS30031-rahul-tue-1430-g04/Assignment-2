using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
	public static GameManager Instance;

	[Header("Game Settings")]
	public int score = 0;
	public int completedAssets = 0;
	public int totalAssets = 8;
	public int currentLevel = 1;

	[Header("UI")]
	public GameObject interactionPanel;
	public GameObject feedbackPanel;
	public GameObject levelCompletePanel;

	[Header("Interaction UI")]
	public TMP_Text assetNameText;
	public TMP_Text questionText;

	[Header("Feedback UI")]
	public TMP_Text feedbackText;
	public TMP_Text explanationText;

	[Header("HUD")]
	public TMP_Text scoreText;
	public TMP_Text progressText;
	public TMP_Text levelText;
	public Slider progressBar;

	private AssetInteraction currentAsset;

	private void Awake()
	{
		Instance = this;
	}

	private void Start()
	{
		// Initial UI state
		if (interactionPanel != null)
			interactionPanel.SetActive(false);

		if (feedbackPanel != null)
			feedbackPanel.SetActive(false);

		if (levelCompletePanel != null)
			levelCompletePanel.SetActive(false);

		UpdateHUD();
	}

	// =========================================================
	// ASSET INTERACTION
	// =========================================================

	public void OpenAssetQuestion(AssetInteraction asset)
	{
		if (asset == null)
			return;

		currentAsset = asset;

		if (interactionPanel != null)
			interactionPanel.SetActive(true);

		if (assetNameText != null)
			assetNameText.text = asset.assetName;

		if (questionText != null)
		{
			questionText.text =
				"Who generally looks after this?";
		}
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
			Debug.LogWarning("No asset is currently selected.");
			return;
		}

		bool correct =
			councilAnswer == currentAsset.councilOwned;

		// Hide interaction panel
		if (interactionPanel != null)
			interactionPanel.SetActive(false);

		// Show feedback panel
		if (feedbackPanel != null)
			feedbackPanel.SetActive(true);

		if (correct)
		{
			Debug.Log("Correct!");

			score += 100;

			feedbackText.text = "CORRECT!";
		}
		else
		{
			Debug.Log("Incorrect!");

			score -= 50;

			feedbackText.text = "NOT QUITE";
		}

		// Every answered asset counts as completed
		completedAssets++;

		// Show explanation
		if (explanationText != null)
		{
			explanationText.text =
				currentAsset.explanation;
		}

		// Update HUD
		UpdateHUD();

		Debug.Log(
			"Assets completed: " +
			completedAssets +
			"/" +
			totalAssets
		);
	}

	// =========================================================
	// CONTINUE BUTTON
	// =========================================================

	public void ContinueAfterFeedback()
	{
		Debug.Log("Continue button pressed.");

		// Hide feedback
		if (feedbackPanel != null)
			feedbackPanel.SetActive(false);

		// Clear current asset
		currentAsset = null;

		// Check whether level is complete
		if (completedAssets >= totalAssets)
		{
			LevelComplete();
		}
	}

	// =========================================================
	// LEVEL COMPLETE
	// =========================================================

	private void LevelComplete()
	{
		Debug.Log("LEVEL COMPLETE!");

		if (levelCompletePanel != null)
			levelCompletePanel.SetActive(true);
	}

	// =========================================================
	// HUD
	// =========================================================

	private void UpdateHUD()
	{
		if (scoreText != null)
		{
			scoreText.text =
				"Score: " + score;
		}

		if (progressText != null)
		{
			progressText.text =
				"Assets Found: " +
				completedAssets +
				"/" +
				totalAssets;
		}

		if (levelText != null)
		{
			levelText.text =
				"Level " + currentLevel;
		}

		if (progressBar != null)
		{
			progressBar.value =
				(float)completedAssets /
				totalAssets;
		}
	}
}