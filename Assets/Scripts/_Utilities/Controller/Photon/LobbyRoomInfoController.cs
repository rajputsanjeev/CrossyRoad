using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Multiplayer;

namespace ThirdPerson.Photon
{
    public class LobbyRoomInfoController : Behaviour<LobbyRoomInfoView>      
    {
        public RoomInfo roomInfo;
        public LobbyRoomInfoView lobbyRoomInfoView;

        protected override void Init()
        {
            Prefab.joinBtn.onClick.AddListener(OnJoinButtonClicked);
        }

        public void SetData(RoomInfo roomInfo)
        {
            lobbyRoomInfoView = GetComponent<LobbyRoomInfoView>();
          //  Debug.LogError("enter ");

            //if (Prefab == null) return; //if player is switching from 1 room to another

      //      Debug.LogError("enter");

            //this.roomInfo = roomInfo;
            //Prefab.lobbyNameText.text = roomInfo.Name;
            //Prefab.lobbyLimit.text = roomInfo.PlayerCount + "/" + roomInfo.MaxPlayers;

            this.roomInfo = roomInfo;
            lobbyRoomInfoView.lobbyNameText.text = roomInfo.Name;
            lobbyRoomInfoView.lobbyLimit.text = roomInfo.PlayerCount + "/" + roomInfo.MaxPlayers;

        }

        private void OnJoinButtonClicked()
        {
            PhotonController.Instance.JoinRoom(roomInfo.Name, PlayerStatus.LOBBY_ROOM);
        }
    }
}
