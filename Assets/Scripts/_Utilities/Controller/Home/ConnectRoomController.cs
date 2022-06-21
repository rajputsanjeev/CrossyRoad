using CrossyRoad.Views.Home;
using UnityEngine;
using CrossyRoad.Photon;
using Photon.Pun;
using Photon.Realtime;

namespace Crossyroad
{
    public class ConnectRoomController : PhotonListener<PlayerStatus>
    {
        private ConnectRoomView m_View;
        int tryCount;

        protected override void Init()
        {
            m_View = (ConnectRoomView)Prefab;
            m_View.createBtn.onClick.AddListener(OnCreateButtonClicked);
            m_View.playBtn.onClick.AddListener(OnPlayButtonClicked);
        }
        
        public override void OnPhotonEventExecuted(PlayerStatus data)
        {
            switch (data)
            {
                case PlayerStatus.RANDOM_JOIN_FAILED:
                    Debug.Log("There is no random room");
                    SceneController.NotificationGame("There is no random room.");
                break;
                case PlayerStatus.JOIN_ROOM:
                     m_View.startGame.SetActive(PhotonNetwork.IsMasterClient);
                     UIPanelManager.Show(Panel.TYPED_LOBBY, true);
                    break;
                case PlayerStatus.OPPONENT_JOINED:
                    Debug.Log("Opponent Join ");
                    break;
            }
        }

        //public override void OnPhotonEventExecuted(PlayerStatus data, Player enterPlayer)
        //{
        //    switch (data)
        //    {
        //        case PlayerStatus.OPPONENT_JOINED:
        //            Debug.Log("Opponent Join "+enterPlayer.NickName);
        //            break;
        //    }
        //}

        private void OnCreateButtonClicked()
        {
            UIPanelManager.ShowPanel(Panel.CREATE_ROOM, true);
           
        }

        private void OnPlayButtonClicked()
        {
            PhotonController.JoinRandomRoom();
        }

      
    }
}