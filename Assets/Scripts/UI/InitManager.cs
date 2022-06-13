
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CogniHab.Init
{
    public class InitManager : MonoBehaviour
    {
        [SerializeField] private SceneProperties sceneProperties;

        public void OnEnable()
        {
            SceneController.LoadScene(sceneProperties);
        }
    }
}