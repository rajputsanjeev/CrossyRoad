using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using TMPro;
public class SceneController : MonoBehaviour
{
    
    private static SceneController m_Instance;
    public static Action<string> OnNewLevelLoaded;
    public static Action<string> OnUnloadSceneCompleted;
    [SerializeField] private GameObject quitScreen;
    private static GameObject quitScreenOriginal;

    [SerializeField] private GameObject logoutScreen;
    private static GameObject logoutScreenOriginal;

    [SerializeField] private GameObject notificationScreen;
    private static GameObject notificationScreenOriginal;

    [SerializeField] private TextMeshProUGUI notificationText;
    private static TextMeshProUGUI notificationTextOriginal;


    [SerializeField] private GameObject loadingCanvas;
    [SerializeField] private List<AssetReferenceInfo> scenesAssetReference;

    public static string CurrentScene;

    
    private void Awake()
    {
        
        m_Instance = this;
        quitScreenOriginal = quitScreen;
        logoutScreenOriginal = logoutScreen;
        notificationScreenOriginal = notificationScreen;
        notificationTextOriginal = notificationText;
        Debug.Log(quitScreenOriginal);
        SceneManager.sceneLoaded += OnLevelLoaded;
    }

    private void Start()
    {
        quitScreenOriginal = quitScreen;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnLevelLoaded;
    }


    public void ShowLoading(bool bol)
    {
        loadingCanvas.SetActive(bol);
    }
    private void OnLevelLoaded(Scene scene, LoadSceneMode mode)
    {
        Time.timeScale = 1f;
        SceneManager.SetActiveScene(scene);
        //AudioManager.PlayAudio(scene.name);
        OnNewLevelLoaded?.Invoke(scene.name);
        CurrentScene = scene.name;
    }

    public static void LoadScene(string sceneName)
    {
        LoadSceneAsync(sceneName);
    }

    public static void LoadSceneAsync(string sceneName)
    {
#if ADDRESSABLES
        m_Instance.LoadAddressableAsync(sceneName, LoadSceneMode.Single);
#else
        LoadScene(new SceneProperties(sceneName));
#endif
    }

    public static void LoadScene(SceneProperties sceneProperties, Action callback = null)
    {
        string sceneName = sceneProperties.sceneName;

#if ADDRESSABLES
        m_Instance.LoadAddressableAsync(sceneName, sceneProperties.loadSceneMode);
#else
        Debug.Log(sceneProperties.isAsync);
        Debug.Log(sceneProperties.sceneName);

        if (sceneProperties.isAsync)
        {
            m_Instance.StartCoroutine(m_Instance.LoadSceneAsync(sceneName, sceneProperties, callback));
        }
        else
        {
            SceneManager.LoadScene(sceneName, sceneProperties.loadSceneMode);
        }
#endif
    }

    private IEnumerator LoadSceneAsync(string sceneName, SceneProperties sceneProperties, Action callback)
    {
        //Debug.Log("active Loading ");

        yield return StartCoroutine(ShowLoading(sceneProperties));

        //Debug.Log("called again");       
#if ADDRESSABLES
        AsyncOperationHandle op = LoadAddressableAsync(sceneName, sceneProperties.loadSceneMode);
#else
        AsyncOperation op = SceneManager.LoadSceneAsync(sceneName, sceneProperties.loadSceneMode);
#endif

        float dummyProgress = 0.1f;

#if ADDRESSABLES
        while (!op.IsDone)
        {
            Debug.Log("percent completed : " + op.PercentComplete);
            float progress = (op.PercentComplete < 1) ? dummyProgress : op.PercentComplete;
#else
        while (op.progress < 1)
        {
            //Debug.Log("percent completed : " + op.progress);
            float progress = (op.progress < 1) ? dummyProgress : op.progress;
#endif
            dummyProgress += 0.1f;
            dummyProgress = Mathf.Clamp(dummyProgress, 0f, 0.9f);

            //loadingView?.SetLoading(progress);
            yield return null;
        }

        //loadingView?.SetLoading(1f);
        yield return new WaitForSecondsRealtime(1f);
        
        if (loadingCanvas != null)
        {
            loadingCanvas.SetActive(false);
        }

        callback?.Invoke();
    }

#if ADDRESSABLES

    private AsyncOperationHandle LoadAddressableAsync(string sceneName, LoadSceneMode loadSceneMode)
    {
        Debug.Log("scene name : " + sceneName);

        AssetReferenceInfo sceneReferenceInfo = scenesAssetReference.Find(x => x.sceneName == sceneName);
        AsyncOperationHandle<SceneInstance> op = Addressables.LoadSceneAsync(sceneReferenceInfo.assetReference, loadSceneMode);
        op.Completed += SceneLoadCompleted;
        return op;
    }

#endif

    private IEnumerator ShowLoading(SceneProperties sceneProperties)
    {
        if (sceneProperties.showLoading)
        {
            Debug.Log("loadingCanvas.activeSelf " + loadingCanvas.activeSelf);
            if (!loadingCanvas.activeSelf)
            {
                loadingCanvas?.SetActive(true);
            }
            yield return new WaitForSecondsRealtime(1f);
        }
    }

    private IEnumerator WaitForUnloadScene(string sceneName)
    {
        AsyncOperation sceneUnload = SceneManager.UnloadSceneAsync(sceneName);
        while (sceneUnload != null && !sceneUnload.isDone)
        {
            yield return null;
        }

        OnUnloadSceneCompleted?.Invoke(sceneName);
    }

    public static void ReloadCurrentScene()
    {
        SceneProperties sceneProperties = new SceneProperties()
        {
            isAsync = true,
            loadSceneMode = LoadSceneMode.Single,
            sceneName = CurrentScene,
            showLoading = true
        };

        LoadScene(sceneProperties);
    }

    #region PC Build Changes
    public static  void QuitGame()
    {
        quitScreenOriginal.SetActive(true);
    }

    public static void LogoutGame()
    {
        logoutScreenOriginal.SetActive(true);
    }
    public static void NotificationGame(string str)
    {
        notificationScreenOriginal.SetActive(true);
        notificationTextOriginal.text = str;
    }

    public void YesClicked()
    {
        Application.Quit();
    }

    public void NoClicked()
    {
        quitScreen.SetActive(false);
    }

    public void YeslogoutClicked()
    {
        PlayerPrefs.DeleteAll();
        logoutScreen.SetActive(false);
        SceneController.LoadScene(Constants.LoginScene);
    }

    public void NoLogoutClicked()
    {
        logoutScreen.SetActive(false);
    }
    public void OkClicked()
    {
        notificationScreenOriginal.SetActive(false);
    }

    #endregion
}

[System.Serializable]
public class SceneProperties
{
    public string sceneName;
    public LoadSceneMode loadSceneMode;
    public bool isAsync;
    public bool showLoading;

    public SceneProperties()
    {
    }

    public SceneProperties(string name)
    {
        sceneName = name;
        loadSceneMode = LoadSceneMode.Single;
        isAsync = true;
        showLoading = true;
    }
}


[System.Serializable]
public struct AssetReferenceInfo
{
    public string sceneName;
    //public AssetReference assetReference;
}