using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MySceneManager : Singleton<MySceneManager>
{
    /// <summary>
    /// 씬 전환 시 페이드 아웃 효과를 주기 위해 SceneManager를 대체.
    /// 알파값 이용한 효과
    /// </summary>
    public Image imgFade;
    private const float fadeDuration = 1.0f;

    public void LoadMyScene(string sceneName)
    {
        StartCoroutine(LoadSceneFadeOut(sceneName));
    }
    private void Awake()
    {
        StartCoroutine(FadeIn());  // 씬 시작 시 페이드 인
    }

    private IEnumerator FadeIn()
    {
        float elapsedTime = 0f;
        Color color = imgFade.color;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            color.a = 1f - Mathf.Clamp01(elapsedTime / fadeDuration);
            imgFade.color = color;
            yield return null;
        }
    }

    private IEnumerator LoadSceneFadeOut(string sceneName)
    {
        float elapsedTime = 0f;
        Color color = imgFade.color;
        while (elapsedTime < fadeDuration)
        {
            yield return null;
            elapsedTime += Time.deltaTime;
            color.a = Mathf.Clamp01(elapsedTime / fadeDuration);
            imgFade.color = color;
        }
        SceneManager.LoadScene(sceneName);
    }
}
