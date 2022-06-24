using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using CrossyRoad;
using System.Linq;

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

        GameUtil.IsMultiplayer = true;
        username = tMP_InputField.text;
        UserData.UserName = username;
        UserData.UserID = RandomStringGenerator(4);
        SceneController.LoadScene(sceneProperties.sceneName);
    }


    string RandomStringGenerator(int lenght)
    {
        string characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
        string generated_string = "";

        for (int i = 0; i < lenght; i++)
            generated_string += characters[Random.Range(0, lenght)];

        return generated_string;
    }
}
