using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class PhotonDisconnect : MonoBehaviour
{
    public Button disconnectBtn;

    private void OnEnable()
    {
        disconnectBtn.onClick.AddListener(Disconnect);
    }

    private void OnDisable()
    {
        disconnectBtn.onClick.RemoveListener(Disconnect);
    }

    void Disconnect()
    {
        PhotonNetwork.Disconnect();
    }

}
