using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ChatItem : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _playerName, _playerMessage;

    public void SetChat(string name, string message)
    {
        _playerName.text = name;
        _playerMessage.text = message;
    }
}
