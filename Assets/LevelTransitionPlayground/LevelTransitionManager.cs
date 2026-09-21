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

    private bool levelCompleteShown = false;
    private bool isTransitioning = false;

    void Start()
    {
        levelCompletePanel.SetActive(false);

        // Scene starts dark and fades in
        backgroundOverlay.alpha = 1f;
        StartCoroutine(FadeInFromBlack());
    }

    void Update()
    {
        if (Keyboard.current.lKey.wasPressedThisFrame &&
            !levelCompleteShown &&
            !isTransitioning)
        {
            ShowLevelComplete();
        }
    }

    private IEnumerator FadeInFromBlack()
    {
        float time = 0f;

        while (time < fadeDuration)
        {
            time += Time.deltaTime;

            float progress = time / fadeDuration;

            backgroundOverlay.alpha = Mathf.Lerp(1f, 0f, progress);

            yield return null;
        }

        backgroundOverlay.alpha = 0f;
    }

    public void ShowLevelComplete()
    {
        if (levelCompleteShown)
            return;

        levelCompleteShown = true;

        levelCompletePanel.SetActive(true);

        panelTransform.localScale = Vector3.zero;

        if (audioSource != null && levelCompleteClip != null)
        {
            audioSource.PlayOneShot(levelCompleteClip);
        }

        StartCoroutine(AnimateLevelComplete());
    }

    private IEnumerator AnimateLevelComplete()
    {
        
        float duration = 1.5f; //don't change - alignes perfectly with soundtrack
        float time = 0f;

        while (time < duration)
        {
            time += Time.deltaTime;

            float progress = time / duration;

            // darkens the map
            backgroundOverlay.alpha =
                Mathf.Lerp(0f, 0.75f, progress);

            float scale =
                Mathf.Lerp(0.8f, 1f, progress);

            panelTransform.localScale =
                new Vector3(scale, scale, 1f);

            yield return null;
        }

        backgroundOverlay.alpha = 0.75f;
        panelTransform.localScale = Vector3.one;
    }

    public void RestartLevel()
    {
        if (isTransitioning)
            return;

        isTransitioning = true;

        StartCoroutine(FadeAndLoad(
            SceneManager.GetActiveScene().buildIndex
        ));
    }

    public void LoadNextLevel()
    {
        if (isTransitioning)
            return;

        int currentSceneIndex =
            SceneManager.GetActiveScene().buildIndex;

        int nextSceneIndex =
            currentSceneIndex + 1;

        if (nextSceneIndex <
            SceneManager.sceneCountInBuildSettings)
        {
            isTransitioning = true;

            StartCoroutine(FadeAndLoad(nextSceneIndex));
        }
        else
        {
            UnityEngine.Debug.Log(
                "No more levels available."
            );
        }
    }

    private IEnumerator FadeAndLoad(int sceneIndex)
    {
        float startAlpha = backgroundOverlay.alpha;
        float time = 0f;

        while (time < fadeDuration)
        {
            time += Time.deltaTime;

            float progress = time / fadeDuration;

            backgroundOverlay.alpha =
                Mathf.Lerp(startAlpha, 1f, progress);

            yield return null;
        }

        backgroundOverlay.alpha = 1f;

        SceneManager.LoadScene(sceneIndex);
    }
}

/*
level transition complete
some minor UI changes:
- colours are horrible
- sounds for buttonclicking, level start (maybe along with fade-in), level end (maybe along with fade out),...
