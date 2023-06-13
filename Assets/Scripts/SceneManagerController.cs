using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;

public class SceneManagerController : MonoBehaviour
{
    public static SceneManagerController Instance { get; private set; }
    [Header("Fade Components")]
    [SerializeField] private Image foreground;
    [Header("Fade In")]
    [SerializeField] private float fadeInTime;
    [SerializeField] private Color fadeInColor;
    [Header("Fade Out")]
    [SerializeField] private float fadeOutTime;
    [SerializeField] private Color fadeOutColor;

    private Action onFade;
    private Action onLoadScene;
    private Color imageColor;
    private string scenNameToLoad;

    private void Awake()
    {
        if (Instance != this && Instance != null)
        {
            Destroy(this.gameObject);
        }

        Instance = this;
        //DontDestroyOnLoad(this.gameObject);
    }

    private void Start()
    {
        imageColor = foreground.color;
        CallFadeOut();
        //StartCoroutine(FadeCoroutine(fadeOutTime, fadeOutColor));
    }

    public void LoadScene(string newSceneName)
    {
        scenNameToLoad = newSceneName;
        onFade = CallLoadScene;
        onLoadScene = CallFadeOut;
        CallFadeIn();
    }

    private void CallFadeOut()
    {
        StartCoroutine(FadeCoroutine(fadeOutTime, fadeOutColor));
    }

    private void CallFadeIn()
    {
        StartCoroutine(FadeCoroutine(fadeInTime, fadeInColor));
    }

    private void CallLoadScene()
    {
        StartCoroutine(LoadSceneCoroutine(scenNameToLoad));
    }

    private IEnumerator FadeCoroutine(float targetTime, Color targetColor)
    {
        float totalTime = 0f;
        while (totalTime < targetTime)
        {
            totalTime += Time.deltaTime;
            float t = totalTime / targetTime;

            yield return new WaitForSeconds(Time.deltaTime);
            foreground.color = Color.Lerp(imageColor, targetColor, t);

        }
        imageColor = targetColor;
        onFade?.Invoke();
    }

    private IEnumerator LoadSceneCoroutine(string sceneName)
    {
        yield return new WaitForSeconds(fadeInTime);

        AsyncOperation asyncLoadLevel = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);

        while (!asyncLoadLevel.isDone)
        {
            Debug.Log("Loading the Scene");
            yield return null;
        }

        onLoadScene?.Invoke();
    }
}