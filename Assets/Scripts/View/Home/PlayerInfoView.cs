using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerInfoView : MonoBehaviour
{
    public TextMeshProUGUI playerNameText;
    public Image leadCardImage;
    
    public void SetData(string playerName)
    {
        playerNameText.text = playerName;
    }

    public void SetData(Sprite leadCardSprite)
    {
        leadCardImage.sprite = leadCardSprite;
    }

    public void SetData(string playerName, Sprite leadCardSprite)
    {
        SetData(playerName);
        SetData(leadCardSprite);
    }

    public void Reset()
    {
        playerNameText.text = "";
        leadCardImage.sprite = null;
        Show(false);
    }

    public void Show(bool on)
    {
        playerNameText.gameObject.SetActive(on);
        leadCardImage.gameObject.SetActive(on);
    }
}
