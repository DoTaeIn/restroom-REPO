using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class LoadingSceneManager : MonoBehaviour
{
    public string nextSceneName = "Final"; // 다음 씬 이름
    public Slider loadingSlider;         // 슬라이더 UI 
    public Text loadingText;             // 로딩 퍼센트 텍스트

    void Start()
    {
        StartCoroutine(LoadSceneAsync());
    }

    IEnumerator LoadSceneAsync()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(nextSceneName);
        operation.allowSceneActivation = false;

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);

            if (loadingSlider != null)
                loadingSlider.value = progress;

            if (loadingText != null)
                loadingText.text = $"Loading... {Mathf.RoundToInt(progress * 100)}%";

            // 로딩 90% 도달하면 자동 전환
            if (operation.progress >= 0.9f)
            {
                yield return new WaitForSeconds(0.5f); // 연출용 대기
                operation.allowSceneActivation = true;
            }

            yield return null;
        }
    }
}