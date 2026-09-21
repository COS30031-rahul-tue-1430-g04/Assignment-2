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

    private bool levelCompleteShown = false;

    void Start()
    {
        levelCompletePanel.SetActive(false);
        backgroundOverlay.alpha = 0f;
    }

    void Update()
    {
        if (Keyboard.current.lKey.wasPressedThisFrame && !levelCompleteShown)
        {
            ShowLevelComplete();
        }
    }

    public void ShowLevelComplete()
    {
        levelCompleteShown = true;

        levelCompletePanel.SetActive(true);

        panelTransform.localScale = Vector3.zero;

        StartCoroutine(AnimateLevelComplete());
    }

    private IEnumerator AnimateLevelComplete()
    {
        float duration = 0.5f;
        float time = 0f;

        while (time < duration)
        {
            time += Time.deltaTime;

            float progress = time / duration;

            backgroundOverlay.alpha = Mathf.Lerp(0f, 0.75f, progress);

            float scale = Mathf.Lerp(0.8f, 1f, progress);
            panelTransform.localScale = new Vector3(scale, scale, 1f);

            yield return null;
        }

        backgroundOverlay.alpha = 0.75f;
        panelTransform.localScale = Vector3.one;
    }

    public void RestartLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        SceneManager.LoadScene(currentSceneIndex);
    }

    public void LoadNextLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;

        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            UnityEngine.Debug.Log("No more levels available.");
        }
    }
}