using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace CrossyRoad.Photon
{
    //placed over prefab used for available room info; shown in home scene
    public class LobbyRoomInfoView : View
    {
        public TextMeshProUGUI lobbyNameText;
        public TextMeshProUGUI lobbyLimit;
        public Button joinBtn;
    }
}
