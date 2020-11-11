using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public GameObject loadingScreen;
    public Slider progressBar;

    List<AsyncOperation> scenesLoading = new List<AsyncOperation>();
    float totalSceneProgress;

    private void Awake()
    {
        Instance = this;

        SceneManager.LoadSceneAsync((int)SceneIndexes.TITLE_SCREEN, LoadSceneMode.Additive);
    }


    public void LoadScene()
    {
        loadingScreen.gameObject.SetActive(true);
        scenesLoading.Add(SceneManager.UnloadSceneAsync((int)SceneIndexes.TITLE_SCREEN));
        scenesLoading.Add(SceneManager.LoadSceneAsync((int)SceneIndexes.MAP, LoadSceneMode.Additive));

        StartCoroutine(GetSceneLoadProgress());
    }

    private IEnumerator GetSceneLoadProgress()
    {
        for (int i = 0; i < scenesLoading.Count; i++)
        {
            while (!scenesLoading[i].isDone)
            {
                totalSceneProgress = 0;

                foreach (AsyncOperation operation in scenesLoading)
                {
                    totalSceneProgress += operation.progress;
                }

                totalSceneProgress = (totalSceneProgress / scenesLoading.Count) * 100.0f;

                progressBar.value = Mathf.RoundToInt(totalSceneProgress);

                yield return null;
            }
        }

        SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex((int)SceneIndexes.MAP));
        loadingScreen.gameObject.SetActive(false);
    }

    public void RestartMatch()
    {
        Time.timeScale = 1f;
        loadingScreen.gameObject.SetActive(true);
        SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex((int)SceneIndexes.MANAGER));
        scenesLoading.Add(SceneManager.UnloadSceneAsync((int)SceneIndexes.MAP));
        scenesLoading.Add(SceneManager.LoadSceneAsync((int)SceneIndexes.MAP, LoadSceneMode.Additive));

        StartCoroutine(GetSceneLoadProgress());
    }

    public void BackInTitleScreen()
    {
        Time.timeScale = 1f;
        scenesLoading.Add(SceneManager.UnloadSceneAsync((int)SceneIndexes.MAP));
        scenesLoading.Add(SceneManager.LoadSceneAsync((int)SceneIndexes.TITLE_SCREEN, LoadSceneMode.Additive));
    }
}
