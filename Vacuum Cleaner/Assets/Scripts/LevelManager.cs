using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using Assets.Scripts.Interaction.Vacuum;
using System.Linq;

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

    private List<IEatable> eatables = new List<IEatable>();
    private float percentCleaned;

    private void Start()
    {
        Fade(0, BlackFadeTime);
        timer = GetComponent<Timer>();
        timer.TimerReachedZero += DisplayEndLevelScreen;

        eatables = FindAllEatables();
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

    private List<IEatable> FindAllEatables()
    {
        List<IEatable> objects = new List<IEatable>();
        
        var eatableObjects = FindObjectsOfType<MonoBehaviour>().OfType<IEatable>();
        foreach (IEatable eatable in eatableObjects)
        {
            objects.Add(eatable);
        }
        return objects;
    }

    private float CalculatePercentageEatablesEaten()
    {
        float percent = 0; // some eat count / eatables.Count * 100
        return percent;
    }
    private void UpdatePercentEaten(Text tex, int decimalCount)
    {
        tex.text = CalculatePercentageEatablesEaten().ToString("F" + decimalCount);
    }

    private void UpdatePercentEaten(TextMeshProUGUI tex, int decimalCount)
    {
        tex.text = CalculatePercentageEatablesEaten().ToString("F" + decimalCount);
    }

    IEnumerator LoadSceneAfterDelay(float delay, int sceneBuildIndex)
    {
        yield return new WaitForSeconds(delay);

        SceneManager.LoadScene(sceneBuildIndex);
    }
}
