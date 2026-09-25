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

	[Header("UI")]
	public GameObject interactionPanel;
	public GameObject feedbackPanel;
	public GameObject levelCompletePanel;

	public TMP_Text assetNameText;
	public TMP_Text questionText;

	public TMP_Text feedbackText;
	public TMP_Text explanationText;

	public TMP_Text scoreText;
	public TMP_Text progressText;

	public Slider progressBar;

	private AssetInteraction currentAsset;

	private void Awake()
	{
		Instance = this;
	}

	private void Start()
	{
		interactionPanel.SetActive(false);
		/*feedbackPanel.SetActive(false);
		levelCompletePanel.SetActive(false);*/

		UpdateHUD();
	}

	public void OpenAssetQuestion(AssetInteraction asset)
	{
		currentAsset = asset;

		interactionPanel.SetActive(true);

		assetNameText.text = asset.assetName;

		questionText.text =
			"Who generally looks after this?";
	}

	public void ChooseCouncil()
	{
		CheckAnswer(true);
	}

	public void ChoosePrivate()
	{
		CheckAnswer(false);
	}

	private void CheckAnswer(bool councilAnswer)
	{
		bool correct =
			councilAnswer == currentAsset.councilOwned;

		interactionPanel.SetActive(false);
		
		feedbackPanel.SetActive(true);

		if (correct)
		{
			Debug.Log("correct");
			score += 100;
			completedAssets++;

			feedbackText.text = "✓ Correct!";
		}
		else
		{
			Debug.Log("incorrect");
			score -= 50;

			completedAssets++;

			feedbackText.text = "✗ Incorrect";
		}

		explanationText.text =
			currentAsset.explanation;

		UpdateHUD();
	}

	public void Continue()
	{
		feedbackPanel.SetActive(false);

		if (completedAssets >= totalAssets)
		{
			LevelComplete();
		}
	}

	private void UpdateHUD()
	{
		scoreText.text = "Score: " + score;

		progressText.text =
			"Assets Found: " +
			completedAssets +
			"/" +
			totalAssets;

		if (progressBar != null)
		{
			progressBar.value =
				(float)completedAssets /
				totalAssets;
		}
	}

	public void closeFeedbackPanel()
	{
		feedbackPanel.SetActive(false);
	}
	
	
	
	
	private void LevelComplete()
	{
		levelCompletePanel.SetActive(true);
	}
}