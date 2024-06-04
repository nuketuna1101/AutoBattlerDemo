using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MySceneManager : Singleton<MySceneManager>
{
    /// <summary>
    /// �� ��ȯ �� ���̵� �ƿ� ȿ���� �ֱ� ���� SceneManager�� ��ü.
    /// ���İ� �̿��� ȿ��
    /// </summary>
    public Image imgFade;
    private const float fadeDuration = 1.0f;

    public void LoadMyScene(string sceneName)
    {
        StartCoroutine(LoadSceneFadeOut(sceneName));
    }
    private void Awake()
    {
        StartCoroutine(FadeIn());  // �� ���� �� ���̵� ��
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
