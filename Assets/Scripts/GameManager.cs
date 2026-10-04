using UnityEngine;
using TMPro;
using UnityEngine.UI;

[RequireComponent(typeof(LevelTransitionManager))]
public class GameManager : MonoBehaviour
{
	public static GameManager Instance;

	[Header("Game Settings")]
	[HideInInspector]
	public int totalAssets = 0;

	public int currentLevel = 1;

	[Header("Current Level Stats")]
	public int assetsExplored = 0;
	public int assetsAnswered = 0;

	[Header("Total Game Stats")]
	public static int totalScore = 0;
	public static int totalAssetsExplored = 0;
	public static int totalAssetsAnswered = 0;

	[Header("UI Panels")]
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

	[Header("Player")]
	public PlayerController playerController;

	private AssetInteraction currentAsset;
	private LevelTransitionManager levelManager;

	// Reset statistics when a completely new Unity Play session starts
	[RuntimeInitializeOnLoadMethod(
		RuntimeInitializeLoadType.SubsystemRegistration
	)]
	private static void ResetGameData()
	{
		totalScore = 0;
		totalAssetsExplored = 0;
		totalAssetsAnswered = 0;
	}

	private void Awake()
	{
		Instance = this;

		levelManager =
			GetComponent<LevelTransitionManager>();

		assetsExplored = 0;
		assetsAnswered = 0;

		CountAssets();

		// Automatically find player if one wasn't assigned
		if (playerController == null)
		{
			playerController =
				FindFirstObjectByType<PlayerController>();
		}
	}

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

	private void Start()
	{
		if (interactionPanel != null)
			interactionPanel.SetActive(false);

		if (feedbackPanel != null)
			feedbackPanel.SetActive(false);

		if (levelCompletePanel != null)
			levelCompletePanel.SetActive(false);

		// Make sure player can move when level starts
		if (playerController != null)
		{
			playerController.EnableMovement();
		}

		UpdateHUD();
	}

	// ---------------------------------------------------------
	// ASSET INTERACTION
	// ---------------------------------------------------------

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

		currentAsset = asset;

		// STOP PLAYER MOVEMENT
		if (playerController != null)
		{
			playerController.DisableMovement();
		}

		// Asset has been explored
		assetsExplored++;
		totalAssetsExplored++;

		UpdateHUD();

		if (interactionPanel != null)
			interactionPanel.SetActive(true);

		if (assetNameText != null)
		{
			assetNameText.text =
				asset.assetName;
		}

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

	// ---------------------------------------------------------
	// ANSWERS
	// ---------------------------------------------------------

	public void ChooseCouncil()
	{
		CheckAnswer(true);
	}

	public void ChoosePrivate()
	{
		CheckAnswer(false);
	}

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

		bool correct =
			councilAnswer ==
			currentAsset.councilOwned;

		// Close question panel
		if (interactionPanel != null)
		{
			interactionPanel.SetActive(false);
		}

		// Show feedback panel
		if (feedbackPanel != null)
		{
			feedbackPanel.SetActive(true);
		}

		if (correct)
		{
			Debug.Log("Correct!");

			// Correct answer = +100
			totalScore += 100;

			// ONLY correct answers increase answered count
			assetsAnswered++;
			totalAssetsAnswered++;

			if (feedbackText != null)
			{
				feedbackText.text =
					"CORRECT!";
			}
		}
		else
		{
			Debug.Log("Incorrect!");

			// Wrong answer = -50
			totalScore -= 50;

			// Do NOT increase answered count

			if (feedbackText != null)
			{
				feedbackText.text =
					"NOT QUITE";
			}
		}

		if (explanationText != null)
		{
			explanationText.text =
				currentAsset.explanation;
		}

		// Player remains frozen here
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

	// ---------------------------------------------------------
	// CONTINUE AFTER FEEDBACK
	// ---------------------------------------------------------

	public void ContinueAfterFeedback()
	{
		Debug.Log(
			"Continue button pressed."
		);

		if (feedbackPanel != null)
		{
			feedbackPanel.SetActive(false);
		}

		// Allow player to move again
		if (playerController != null)
		{
			playerController.EnableMovement();
		}

		currentAsset = null;

		// Check if all assets have been explored
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

	// ---------------------------------------------------------
	// HUD
	// ---------------------------------------------------------

	private void UpdateHUD()
	{
		if (scoreText != null)
		{
			scoreText.text =
				"Score: " +
				totalScore;
		}

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

		if (levelText != null)
		{
			levelText.text =
				"Level " +
				currentLevel;
		}

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

	// ---------------------------------------------------------
	// GETTERS
	// ---------------------------------------------------------

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