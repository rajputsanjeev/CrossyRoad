using ThirdPerson;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class PrivateChat : MonoBehaviour
{
    public Button privateChatBtn;
    public string photonPlayerID;
    [SerializeField] public TextMeshProUGUI chatNumberText;
    public GameObject image;
    [SerializeField] public int chatNumber;
    public static Action<string , TextMeshProUGUI , int> OnChatButtonClicked;

    private void OnEnable()
    {
        privateChatBtn.onClick.AddListener(SendPrivateChat);
    }

    private void OnDisable()
    {
        privateChatBtn.onClick.RemoveListener(SendPrivateChat);
    }

    private void SendPrivateChat()
    {
        OnChatButtonClicked?.Invoke(photonPlayerID , chatNumberText , chatNumber);
    }
}
