using CrossyRoad.Photon;
using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;

namespace Crossyroad { 
    /// <summary>
    /// This class will update lobby view as player joins/leaves lobby.
    /// Also, will manage challenges using ChallengeController base class.
    /// </summary>
    public class LobbyRoomController : PhotonListener<PlayerStatus>
    {
        public List<PlayerLobbyView> playerLobbyViews;
        private LobbyRoomView m_LobbyView;
        protected string roomName;

        protected override void Init()
        {
            base.Init();
            roomName = "";
            m_LobbyView = (LobbyRoomView)Prefab;
            m_LobbyView.leaveButton.onClick.AddListener(ExitLobby);
            m_LobbyView.startGame.onClick.AddListener(StartGame);
            m_LobbyView.lobbyName.text = PhotonNetwork.CurrentRoom.Name;
            GeneratePoolList();
            ShowPlayersInLobby();
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            ShowPlayersInLobby();
            m_LobbyView.chatView.Connect();
        }

        public override void OnPhotonEventExecuted(PlayerStatus data)
        {
            switch (data)
            {
                case PlayerStatus.LEAVE_ROOM:
                    UIPanelManager.Show(Panel.HOME);
                    m_LobbyView.chatView.OnLeftRoom();
                    break;

                case PlayerStatus.OPPONENT_LEFT:
                case PlayerStatus.OPPONENT_JOINED:
                    ShowPlayersInLobby();
                    m_LobbyView.chatView.OnOpponentChanged(data, PhotonController.OpponentPlayer.NickName);
                    break;
                case PlayerStatus.MASTER_CLIENT_SWITCH:
                    m_LobbyView.startGame.gameObject.SetActive(PhotonNetwork.IsMasterClient);
                    break;
            }
        }

        private void GeneratePoolList()
        {
            if (playerLobbyViews.Count > 0) return; //already items were instantiated before

            playerLobbyViews = new List<PlayerLobbyView>();
            for (int i = 0; i < Constants.MAX_PLAYERS_IN_LOBBY; i++)
            {
                GameObject playerObj = null;
                if (i == 0) //adding self prefab in index 0
                {
                    playerObj = Instantiate(m_LobbyView.lobbyPlayerInfoPrefabSelf, m_LobbyView.parentContainer);
                }
                else
                {
                    playerObj = Instantiate(m_LobbyView.lobbyPlayerInfoPrefab, m_LobbyView.parentContainer);
                }

                playerLobbyViews.Add(playerObj.GetComponent<PlayerLobbyView>());
                playerObj.SetActive(false);
            }
        }

        private void ShowPlayersInLobby()
        {
            ResetPlayersList();
            int j = 1;
            for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
            {

                if (!PhotonNetwork.PlayerList[i].IsLocal)
                {
                    playerLobbyViews[j].SetData(PhotonNetwork.PlayerList[i]);
                    j++;
                }
                else
                {
                    playerLobbyViews[0].SetData(PhotonNetwork.PlayerList[i]);
                }
            }
        }


        private void ResetPlayersList()
        {
            for (int i = 0; i < playerLobbyViews.Count; i++)
            {
                playerLobbyViews[i].Reset();
            }
        }

        public void StartGame()
        {
            PhotonNetwork.LoadLevel(3);
        }
        #region PhotonEvents
        private void ExitLobby()
        {
            PhotonController.LeaveRoom();
        }
        #endregion
    }
}
