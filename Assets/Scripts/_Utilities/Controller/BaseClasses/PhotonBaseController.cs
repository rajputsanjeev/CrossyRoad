using CrossyRoad;


using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Multiplayer
{
    public abstract class PhotonBaseController : MonoBehaviourPunCallbacks
    {
        public static PhotonBaseController Instance;
        protected PhotonModel photonModel;



        public Player OpponentPlayer => photonModel.opponentPlayer;

        public bool IsReconnect
        {
            set
            {
                photonModel.isReconnected = value;
            }
        }

        public PlayerStatus PlayerStatus
        {
            set
            {
                photonModel.playerStatus = value;
            }

            get
            {
                return photonModel.playerStatus;
            }
        }

        protected virtual void Awake()
        {
            Instance = this;
            Debug.Log(Instance);
            photonModel = this.GetComponent<PhotonModel>();
            PhotonNetwork.AutomaticallySyncScene = true;

        }

        private void OnDestroy()
        {
            Debug.Log("destroyed");
        }

        protected void ShowLoading(bool on = true)
        {
          
        }

        protected void CallPhotonListner<T>(T data)
        {
            if (typeof(T) != typeof(List<RoomInfo>))
            {
                ShowLoading(false);
            }

            if (PhotonListener<T>.Instance == null)
            {
                return;
            }

            PhotonListener<T>.Instance.OnPhotonEventExecuted(data);
        }

        public void ConnectToPhoton(PlayerStatus playerStatus, bool isReconnect)
        {
            photonModel.playerStatus = playerStatus;
            Debug.Log("connect to photon : " + playerStatus);

            photonModel.isReconnected = isReconnect;
            
            bool success = PhotonNetwork.ConnectUsingSettings();

            if (!success)
            {
                Debug.Log("disconnect");
                photonModel.isReconnected = true;
                PhotonNetwork.Disconnect();
                ShowLoading();
            }
        }

        public void CreateRoom(string roomName, RoomType type)
        {
            bool success;
            success = PhotonNetwork.CreateRoom(roomName,photonModel.GetRoomOptionsRandom(type));
            CheckSuccess(success);
            Debug.Log("create lobby room");
            Debug.Log("success : " + success);
        }

        public void LeaveRoomToJoinChallenge(PlayerStatus playerStatus)
        {
            photonModel.playerStatus = playerStatus;
            photonModel.isReconnected = false;
            bool success = PhotonNetwork.LeaveRoom();
            CheckSuccess(success);
            Debug.Log("Leave room");
        }

        public void JoinRoom(string roomName, PlayerStatus playerStatus)
        {
            ShowLoading();
            photonModel.playerStatus = playerStatus;
            bool success = PhotonNetwork.JoinRoom(roomName);
            Debug.Log(success + ", roomname : " + roomName);
            CheckSuccess(success);
        }

        public void JoinOrCreate(string roomName, RoomType type , PlayerStatus playerStatus)
        {
            ShowLoading();
            photonModel.playerStatus = playerStatus;
            bool success = PhotonNetwork.JoinOrCreateRoom(roomName , photonModel.GetRoomOptionsRandom(type) , typedLobby:default);
            Debug.Log(success + ", roomname : " + roomName);
            CheckSuccess(success);
        }

        public void JoinRandomRoom(int joinAttempt = 0)
        {
            Debug.Log("Jon random Room");
            ShowLoading();
            PhotonNetwork.JoinRandomRoom();
        }

        private void CheckSuccess(bool success)
        {
            photonModel.isReconnected = false;

            if (!success)
            {
                photonModel.isReconnected = true;
                ConnectToPhoton(photonModel.playerStatus, true);
            }
        }

        public void LeaveRoom()
        {
            Debug.Log("leave room");

            photonModel.playerStatus = PlayerStatus.LEAVE_ROOM;
            bool success = PhotonNetwork.LeaveRoom();
            Debug.Log("success  " + success);
            if (success == false)
            {
                PhotonNetwork.Disconnect();
                CallPhotonListner(photonModel.playerStatus);
            }
        }
    }
}