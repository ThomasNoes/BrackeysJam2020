using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

[RequireComponent(typeof(Timer))]
public class LevelManager : MonoBehaviour
{
    [Header("Assignables")]
    [SerializeField] private GameObject endLevelScreen;
    [SerializeField] private Image blackScreen;
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private int menuSceneBuildIndex = 0;

    [Header("Properties")]
    [SerializeField] private float BlackFadeTime = 2f;

    private Timer timer;
    private float percentCleaned;

    private void Start()
    {
        Fade(0, BlackFadeTime);
        timer = GetComponent<Timer>();
        timer.TimerReachedZero += DisplayEndLevelScreen;
    }

    private void FixedUpdate()
    {
        timer.UpdateText(timerText, 0); //update timer text displaying 0 decimals
    }

    private void DisplayEndLevelScreen()
    {
        endLevelScreen.SetActive(true);
    }

    public void LoadNextLevel()
    {
        Fade(1, BlackFadeTime);

        int currentBuildingIndex = SceneManager.GetActiveScene().buildIndex;
        if (currentBuildingIndex < SceneManager.sceneCountInBuildSettings - 1)
        {
            StartCoroutine(LoadSceneAfterDelay(BlackFadeTime + .1f, currentBuildingIndex +1));
        }
        else
        {
            StartCoroutine(LoadSceneAfterDelay(BlackFadeTime + .1f, 0));

        }
    }

    public void RestartLevel() //re-loads level
    {
        Fade(1, BlackFadeTime);
        StartCoroutine(LoadSceneAfterDelay(BlackFadeTime + .1f, SceneManager.GetActiveScene().buildIndex));
    }

    public void GoToMainMenu()
    {
        Fade(1, BlackFadeTime);
        StartCoroutine(LoadSceneAfterDelay(BlackFadeTime + .1f, menuSceneBuildIndex));
    }
    private void Fade(float alphaTargetValue, float seconds)
    {
        blackScreen.CrossFadeAlpha(alphaTargetValue, seconds, true);
    }

    IEnumerator LoadSceneAfterDelay(float delay, int sceneBuildIndex)
    {
        yield return new WaitForSeconds(delay);

        SceneManager.LoadScene(sceneBuildIndex);
    }
}
