using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class ChatTextView : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI sender;

    [SerializeField]
    private TextMeshProUGUI message;

    [SerializeField]
    private Image playerImage;

    [SerializeField] private Color m_UserChatColor, m_PlayerChatColor;

    [SerializeField] private string senderName;
    public void SetData(string sender,string message)
    {
        senderName = sender;
        this.sender.text = sender;
        this.message.text =  message;
    }
}
