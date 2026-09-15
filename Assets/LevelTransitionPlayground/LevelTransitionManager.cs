using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class LevelTransitionManager : MonoBehaviour
{
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

    void Update()
    {
        if (Keyboard.current.lKey.wasPressedThisFrame)
        {
            LoadNextLevel();
        }
    }
}

//when pressing 'L', the current build index is determined and +1 is added to transition to next level
//"pressing L" is a placeholder for later when progression bar is added (and it reaches 100%)

//add "cooler" level transition animations
//Fade-in for new level
//things like score, progession, buttons for "next level" or "retry level" come later