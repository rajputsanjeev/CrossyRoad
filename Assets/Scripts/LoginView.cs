using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using ThirdPerson;

public class LoginView : MonoBehaviour
{
    public static string username;
    public SceneProperties sceneProperties;
    public void SetName(TMP_InputField tMP_InputField)
    {
        if (tMP_InputField == null)
            return;

        if (string.IsNullOrEmpty(tMP_InputField.text))
            return;

        username = tMP_InputField.text;
        UserData.UserName = username;
        SceneController.LoadScene(sceneProperties.sceneName);
    }
}
