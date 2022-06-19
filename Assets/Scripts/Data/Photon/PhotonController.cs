using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;
using CrossyRoad;

namespace Multiplayer
{
    public class PhotonController : PhotonBaseController
    {
        public int playerCount;

        public override void OnConnectedToMaster()
        {
            photonModel.SetUserDataForLobby();
            PhotonNetwork.JoinLobby();
        }

        public override void OnJoinedLobby()
        {
           // Debug.Log("Joined Lobby..........." + photonModel.playerStatus + "," + photonModel.isReconnected);
          //  Debug.Log("Player count  " + PhotonNetwork.CountOfPlayers);
            //1. leave and join in game play
            //2. join lobby from room
            if (photonModel.IsListenerAllowedInLobby)
            {
                photonModel.isReconnected = false;
                CallPhotonListner(photonModel.playerStatus);
            }
        }

        public override void OnJoinedRoom()
        {
            Debug.Log("Join Room");
            photonModel.SetPlayerProperty("PositionCount", PhotonNetwork.CurrentRoom.PlayerCount);
            CallPhotonListner(PlayerStatus.JOIN_ROOM);
            
        }
        public override void OnMasterClientSwitched(Player newMasterClient)
        {
            CallPhotonListner(PlayerStatus.MASTER_CLIENT_SWITCH);
        }
        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
             Debug.Log("Opponint join");
            photonModel.opponentPlayer = newPlayer;
            CallPhotonListner(PlayerStatus.OPPONENT_JOINED);
        }

        public override void OnCreateRoomFailed(short returnCode, string message)
        {
            Debug.Log("on create room failed: " + message);

            if (photonModel.playerStatus == PlayerStatus.LOBBY_ROOM || photonModel.playerStatus == PlayerStatus.CREATE_CHALLENGE)
            {
                CallPhotonListner(PlayerStatus.CREATE_ROOM_FAILED);
            }
        }

        public override void OnJoinRoomFailed(short returnCode, string message)
        {
           Debug.Log("on join room failed : " + returnCode + " , " + message);

            if (photonModel.playerStatus == PlayerStatus.JOIN_CHALLENGE)
            {
                photonModel.playerStatus = PlayerStatus.JOIN_CHALLENGE_FAILED;
            }
            else
            {
                photonModel.playerStatus = PlayerStatus.PHOTON_LOBBY_FAILED;
                if(message.Contains("already joined"))
                {
                    SceneController.NotificationGame("You are already inside the lobby.");
                }
            }

            CallPhotonListner(photonModel.playerStatus);
        }

        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            Debug.Log("join random room failed : " + returnCode + " , " + message);

            CallPhotonListner(PlayerStatus.RANDOM_JOIN_FAILED);
        }

        public override void OnLeftLobby()
        {
        }

        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            photonModel.opponentPlayer = otherPlayer;
            CallPhotonListner(PlayerStatus.OPPONENT_LEFT);
        }

        public override void OnRoomListUpdate(List<RoomInfo> roomList)
        {
           // Debug.Log("room list updated");
            CallPhotonListner(roomList);
        }
        public override void OnLobbyStatisticsUpdate(List<TypedLobbyInfo> lobbyStatistics)
        {
            string countPlayersOnline;
            countPlayersOnline = lobbyStatistics[0].PlayerCount + " Players Online";
            playerCount = PhotonNetwork.CountOfPlayers;
        }

        public override void OnLeftRoom()
        {
            if (!photonModel.isReconnected)
            {
                if (photonModel.playerStatus == PlayerStatus.NONE)
                {
                    photonModel.playerStatus = PlayerStatus.LEAVE_ROOM;
                }

                CallPhotonListner(photonModel.playerStatus);
            }
        }

     
        public override void OnDisconnected(DisconnectCause cause)
        {
            Debug.Log("==========disconnected from server" + cause.ToString());
            
            if (photonModel.isReconnected)
            {
                switch (photonModel.playerStatus)
                {
                    case PlayerStatus.JOIN_CHALLENGE:
                    case PlayerStatus.CREATE_CHALLENGE:
                    case PlayerStatus.PHOTON_LOBBY:
                    case PlayerStatus.LOBBY_ROOM:
                    case PlayerStatus.RANDOM_JOIN:
                    case PlayerStatus.RANDOM_JOIN_RECONNECT:
                        if (photonModel.connectionRetriesCnt > Constants.CONNECTION_RETRIES_LIMIT)
                        {
                            photonModel.connectionRetriesCnt = 0;
                            SceneController.NotificationGame("Error connecting photon server! Try again!!");
                            return;
                        }

                        Debug.Log("connecting");

                        ConnectToPhoton(photonModel.playerStatus, true);
                        photonModel.connectionRetriesCnt++;
                        break;
                }

                return;
            }
            ShowLoading(false);

            if (photonModel.playerStatus == PlayerStatus.RELOAD_SCENE)
            {
                photonModel.playerStatus = PlayerStatus.NONE;
                SceneController.ReloadCurrentScene();
            }
            else if (cause == DisconnectCause.ServerTimeout)
            {
                SceneController.NotificationGame("You got disconnected from server.");
            }
            else if (cause == DisconnectCause.ExceptionOnConnect)
            {
                photonModel.connectionRetriesCnt = 0;
            }
            else
            {
                SceneController.LoadScene(Constants.HomeScene);
            }
        }
    }
}