using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Crossyroad;

namespace CrossyRoad.Lobby
{
    public class CreateLobbyRoomView : UIPanelComponent
    {
        [SerializeField] private TMP_InputField roomName;
        [SerializeField] private TextMeshProUGUI placeHolderText;
        [SerializeField] private Button m_CreateButton;

        private void OnEnable()
        {
            m_CreateButton.onClick.AddListener(OnClick);
        }

        private void OnDisable()
        {
            m_CreateButton.onClick.RemoveListener(OnClick);
        }

        private void OnClick()
        {
            if (string.IsNullOrEmpty(roomName.text.Trim()))
            {
                placeHolderText.color = Color.red;
                roomName.text = "";
                return;
            }

            PhotonController.Instance.CreateRoom(roomName.text, RoomType.LOBBY);
        }

    }
}
