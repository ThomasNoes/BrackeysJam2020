using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using Assets.Scripts.Interaction.Vacuum;
using System.Linq;
using System;

[RequireComponent(typeof(Timer))]
public class LevelManager : MonoBehaviour
{
    [Header("Assignables")]
    [SerializeField] private GameObject endLevelScreen;
    [SerializeField] private Image blackScreen;
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private TextMeshProUGUI eatPercentText;
    [SerializeField] private Slider completionSlider;
    [SerializeField] private int menuSceneBuildIndex = 0;

    [Header("Properties")]
    [SerializeField] private float blackFadeTime = 2f;
    [Range(1, 100)]
    [SerializeField] private float bronzeLevel = 30f;
    [Range(1, 100)]
    [SerializeField] private float silverLevel = 50f;
    [Range(1, 100)]
    [SerializeField] private float goldLevel = 70f;
    [Range(0, 10)]
    [SerializeField] private float sliderAnimationTime = 3f;



    public event Action levelEnd;
    public event Action loadStarted;
    private Timer timer;

    // eatables and associated
    public event Action allPercentEaten;
    private VacuumSource vacuumSource;
    private int objectsEaten;
    private List<IEatable> eatables = new List<IEatable>();

    private void Start()
    {
        initialize();
        Fade(0, blackFadeTime);
        if(vacuumSource != null)
            vacuumSource.eatEvent += OnEatObject;

        timer.TimerReachedZero += DisplayEndLevelScreen;
        allPercentEaten += DisplayEndLevelScreen;
    }

    private void initialize()
    {
        vacuumSource = FindObjectOfType<VacuumSource>();
        eatables = FindAllEatables();
        timer = GetComponent<Timer>();
        objectsEaten = 0;
        UpdatePercentEaten(eatPercentText, 0);
    }
    private void FixedUpdate()
    {
        timer.UpdateText(timerText, 0); //update timer text displaying 0 decimals
    }

    private void DisplayEndLevelScreen()
    {
        timer.PauseTimer();
        endLevelScreen.SetActive(true);
        if(levelEnd != null)
            levelEnd.Invoke();
        UpdateSlider(completionSlider, sliderAnimationTime);
    }

    public void LoadNextLevel()
    {
        Fade(1, blackFadeTime);

        int currentBuildingIndex = SceneManager.GetActiveScene().buildIndex;
        if (currentBuildingIndex < SceneManager.sceneCountInBuildSettings - 1)
        {
            StartCoroutine(LoadSceneAfterDelay(blackFadeTime + .1f, currentBuildingIndex +1));
        }
        else
        {
            StartCoroutine(LoadSceneAfterDelay(blackFadeTime + .1f, 0));

        }
    }

    public void RestartLevel() //re-loads level
    {
        Fade(1, blackFadeTime);
        StartCoroutine(LoadSceneAfterDelay(blackFadeTime + .1f, SceneManager.GetActiveScene().buildIndex));
    }

    public void GoToMainMenu()
    {
        Fade(1, blackFadeTime);
        StartCoroutine(LoadSceneAfterDelay(blackFadeTime + .1f, menuSceneBuildIndex));
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
        float percent = (float)objectsEaten / eatables.Count * 100;

        if(percent == 100)
        {
            allPercentEaten.Invoke();
        }
        return percent;
    }

    private void OnEatObject()
    {
        objectsEaten++;
        float percent = CalculatePercentageEatablesEaten();
        UpdatePercentEaten(eatPercentText, 0);
    }
    private void UpdatePercentEaten(Text tex, int decimalCount)
    {
        tex.text = CalculatePercentageEatablesEaten().ToString("F" + decimalCount) + @"%";
    }

    private void UpdatePercentEaten(TextMeshProUGUI tex, int decimalCount)
    {
        tex.text = CalculatePercentageEatablesEaten().ToString("F" + decimalCount) + @"%";
    }

    IEnumerator LoadSceneAfterDelay(float delay, int sceneBuildIndex)
    {
        if(loadStarted != null)
        {
            loadStarted.Invoke();
        }
        yield return new WaitForSeconds(delay);

        SceneManager.LoadScene(sceneBuildIndex, LoadSceneMode.Single);
    }

    private void UpdateSlider(Slider slider, float animationTime)
    {
        slider.value = 0;
        StartCoroutine(AnimateSliderUpdate(slider, animationTime, CalculatePercentageEatablesEaten()));
    }

    IEnumerator AnimateSliderUpdate(Slider slide, float seconds, float desiredPercent)
    {
        float lerpTime = 0f;
        float lerpValue = 0f;
        float targetValue = slide.maxValue / 100 * desiredPercent;

        while(slide.value != targetValue)
        {
            //Debug.Log("Animating");
            slide.value = Mathf.Lerp(slide.value, targetValue, lerpValue);
            lerpTime += Time.deltaTime;
            lerpValue = lerpTime / seconds;
            yield return new WaitForSeconds(0);
        }
    }

    private void OnDisable()
    {
        if (vacuumSource != null)
            vacuumSource.eatEvent -= OnEatObject;

        timer.TimerReachedZero -= DisplayEndLevelScreen;
        allPercentEaten -= DisplayEndLevelScreen;
    }
}
