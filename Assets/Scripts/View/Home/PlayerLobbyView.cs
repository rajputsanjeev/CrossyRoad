using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;


namespace CrossyRoad
{
    /// <summary>
    /// This class will be responsible to reveal player's information's listed inside a lobby
    /// and to send Challenge Request of Battle
    /// </summary>
    public class PlayerLobbyView : MonoBehaviour
    {
        public Button challengeBtn;
        [SerializeField] public string playerName;
        [SerializeField] public string id;
        [SerializeField] private TextMeshProUGUI playerNameText;
        [SerializeField] public TextMeshProUGUI chatnumberText;
        [SerializeField] public int chatCount;
     
        public Player photonPlayer;

        public string ProfilePic { get; private set; }

        public void SetData(Player player)
        {
            photonPlayer = player;
            playerNameText.text = player.NickName;
            playerName = player.NickName; ;
            id = player.UserId;

            if (!photonPlayer.IsLocal)
            {
                this.GetComponent<PrivateChat>().photonPlayerID = photonPlayer.NickName;
            }

            Show(true);
        }


        public void Reset()
        {
            playerNameText.text = string.Empty;
            this.gameObject.SetActive(false);
        }

        public void Show(bool on)
        {
            this.gameObject.SetActive(on);
        }
    }
}
