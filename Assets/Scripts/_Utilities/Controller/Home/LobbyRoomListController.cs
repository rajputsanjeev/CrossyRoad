using CrossyRoad;
using CrossyRoad.Photon;
using CrossyRoad.Views.Home;
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;

namespace CrossyRoad.Controller.Home
{
    public class LobbyRoomListController : PhotonListener<List<RoomInfo>>
    {
        private Dictionary<string, LobbyRoomInfoController> m_lobbyRooms;
        private LobbyRoomListView m_View;
        private List<GameObject> allRoom = new List<GameObject>();
        protected override void Init()
        {
            m_lobbyRooms = new Dictionary<string, LobbyRoomInfoController>();
            m_View = (LobbyRoomListView)Prefab;
        }

        protected override void OnEnable()
        {
            base.OnEnable();
          
           // if (PhotonController.PlayerStatus != PlayerStatus.SHUFFLED)
            {
                PhotonController.ConnectToPhoton(PlayerStatus.PHOTON_LOBBY, false); //connect to photon lobby
            }
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            DestroyAllRooms();
        }

        public override void OnPhotonEventExecuted(List<RoomInfo> data)
        {
            if (PhotonNetwork.InLobby)
            {
                ShowRoomListInLobby(data);
            }
        }
     
        private void ShowRoomListInLobby(List<RoomInfo> roomList)
        {
            if (!this.gameObject.activeSelf)
            {
                return;
            }

            Debug.Log("Room Lsit Update "+roomList.Count);

            for (int i = 0; i < roomList.Count; i++)
            {
                if (roomList[i].CustomProperties.ContainsKey("type") && !roomList[i].CustomProperties["type"].Equals(RoomType.LOBBY.ToString()))
                {
                    return;
                }

                //create new room, update existing rooms, remove old rooms
                string roomName = roomList[i].Name;
                int playerCount = roomList[i].PlayerCount;

                if (playerCount == 0)
                {
                    RemoveRoom(roomName);
                }
                else
                {
                    if (!m_lobbyRooms.ContainsKey(roomName))
                    {
                        AddRoom(roomName);
                    }

                    m_lobbyRooms[roomName].SetData(roomList[i]);
                }

                if (roomList[i].RemovedFromList)
                {
                  //  Debug.Log("room name : " + roomName + " , player count : " + playerCount);
                }
            }
        }

        private void DestroyAllRooms()
        {
            List<string> roomName = new List<string>();

            foreach (KeyValuePair<string, LobbyRoomInfoController> item in m_lobbyRooms)
            {
                roomName.Add(item.Key);
            }

            for (int i = 0; i < roomName.Count; i++)
            {
                RemoveRoom(roomName[i]);
            }
        }

        private void AddRoom(string roomName)
        {
            GameObject go = Instantiate(m_View.lobbyRoomPrefab, m_View.parent);
            this.m_lobbyRooms.Add(roomName, go.GetComponent<LobbyRoomInfoController>());
            go.SetActive(true);
        }

        private void RemoveRoom(string name)
        {
            Debug.Log("roomInfo.name ----remove" + name);
            if (!m_lobbyRooms.ContainsKey(name))
            {
                return;
            }

            GameObject go = m_lobbyRooms[name].gameObject;
            m_lobbyRooms.Remove(name);
            Destroy(go);
        }
    }
}
