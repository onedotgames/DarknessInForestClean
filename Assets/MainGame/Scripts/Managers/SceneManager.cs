using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.SceneManagement;
using System;
using DG.Tweening;

public class SceneManager : CustomBehaviour
{
    public VideoPlayer VideoPlayer;
    public GameObject LoadingScreen;
    public Slider LoadingBarFill;

    private void Awake()
    {
        VideoPlayer.loopPointReached += EndReached;
    }

    void EndReached(UnityEngine.Video.VideoPlayer vp)
    {
        vp.enabled = false;
        LoadScene(1);
    }

    public void LoadScene(int SceneId)
    {
        StartCoroutine(LoadSceneAsync(SceneId));
    }

    IEnumerator LoadSceneAsync(int sceneId)
    {
        //AsyncOperation operation = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneId);
        //LoadingScreen.SetActive(true);
        //if (!operation.isDone)
        //{
        //    float progressValue = Mathf.Clamp01(operation.progress / 1f);

        //    //LoadingBarFill.value = progressValue;
        //    LoadingBarFill.DOValue(1f, 3f).;
        //    yield return null;
        //}
        LoadingScreen.SetActive(true);
        LoadingBarFill.DOValue(1f, 3f).OnComplete(() =>
         {
             AsyncOperation operation = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneId);
         });
        yield return null;
    }
}
