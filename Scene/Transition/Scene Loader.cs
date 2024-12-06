using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [Header("事件监听")]
    public SceneLoadEventSO loadEventSO;
    public GameSceneSO firstLoadScene;
    public Transform PlayerTrans;
    public float fadeDuration;

    private GameSceneSO currentLoadedScene;
    private GameSceneSO sceneToLoad;
    private Vector3 positionToGo;
    private bool fadeScreen;

    private void Awake()
    {
        currentLoadedScene = firstLoadScene;
        currentLoadedScene.sceneReference.LoadSceneAsync(LoadSceneMode.Additive);
        //Addressables.LoadSceneAsync(firstLoadScene.sceneReference, LoadSceneMode.Additive);
    }

    private void OnEnable()
    {
        loadEventSO.LoadRequestEvent += OnLoadRequestEvent;
    }
    private void OnDisable()
    {
        loadEventSO.LoadRequestEvent -= OnLoadRequestEvent;
    }
    private void OnLoadRequestEvent(GameSceneSO locationToLoad, Vector3 posToGO, bool FadeScreen)
    {
        sceneToLoad = locationToLoad;
        positionToGo = posToGO;
        this.fadeScreen = FadeScreen;

        if (currentLoadedScene != null) 
        {
            StartCoroutine(UnLoadPreviousScene());
        }
        //Debug.Log("事件触发");
    }

    private IEnumerator UnLoadPreviousScene()
    {
        PlayerTrans.position = positionToGo;
        yield return new WaitForSeconds(fadeDuration);
        currentLoadedScene.sceneReference.UnLoadScene();
        LoadNewScene();
    }

    private void LoadNewScene() 
    {
        sceneToLoad.sceneReference.LoadSceneAsync(LoadSceneMode.Additive, true);
    }
}
