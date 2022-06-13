using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneAsync : MonoBehaviour
{
    [SerializeField] private string sceneToLoad;
    [SerializeField] private float waitBeforeLoad;

    private void Start()
    {
        StartCoroutine(LoadScene());
    }

    IEnumerator LoadScene()
    {
        yield return new WaitForSeconds(waitBeforeLoad);
        SceneManager.LoadSceneAsync(sceneToLoad);
    }
}
