using Ediiie.Photon;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CrossyRoad.Photon
{
    /// <summary>
    /// This class will update lobby view as player joins/leaves lobby.
    /// </summary>
    public class LobbyRoomView : UIPanelComponent
    {
        public TextMeshProUGUI lobbyName;
        public GameObject lobbyPlayerInfoPrefab;
        public GameObject lobbyPlayerInfoPrefabSelf;
        public Transform parentContainer;
        public Button exitBtn; //exit button of lobby screen, when clicked opens leavePrompt
        public Button leaveButton; //leave prompt leave button
        public ChatView chatView;
        public Button startGame;
    }
}