using ThirdPerson;
using ExitGames.Client.Photon;

using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class PhotonModel : PhotonBaseModel
{
    public static string GameRoomName;
    public string lobbyRoomName;
    public PlayerStatus playerStatus;
    public bool isReconnected;
    public int connectionRetriesCnt;
    public const string playerLevel = "C0";
    public const string playerExperience = "C1";
    public Player opponentPlayer;
    public SceneProperties sceneProperties;
    public TypedLobby sqlLobby = new TypedLobby(Constants.RANKED_LOBBY_NAME, LobbyType.SqlLobby);


    public bool IsListenerAllowedInLobby
    {
        get
        {
            //Debug.Log(playerStatus);
            bool isAllowed = isReconnected || playerStatus == PlayerStatus.PHOTON_LOBBY || playerStatus == PlayerStatus.LOBBY_ROOM || playerStatus == PlayerStatus.SHUFFLED;
            bool isAllowedIfNot = playerStatus != PlayerStatus.RANDOM_JOIN;

            return isAllowed && isAllowedIfNot;
        }
    }

    public void ResetPlayerStatus()
    {
        playerStatus = PlayerStatus.NONE;
    }

    public void SetPlayerProperty(object key, object value)
    {
        SetPP(key, value);
    }

    public void SetUserDataForLobby()
    {
        PhotonNetwork.LocalPlayer.NickName = UserData.UserName; // + Random.Range(0,10000);
        PhotonNetwork.GameVersion = "1";

        SetUserPlayerProperties();
    }

    public void SetPlayerStatus(RoomType roomType)
    {
        switch (roomType)
        {
            case RoomType.LOBBY:
                playerStatus = PlayerStatus.LOBBY_ROOM;
                break;
            case RoomType.CHALLENGE:
                playerStatus = PlayerStatus.CREATE_CHALLENGE;
                break;
            case RoomType.RANDOM:
                playerStatus = PlayerStatus.RANDOM_JOIN;
                break;
        }
    }

    //player who comes second will get opponent player
    public void SetOpponentPlayer()
    {
        //only 1 player exist in room and which is my player so no need to loop in player list
        if(PhotonNetwork.CurrentRoom.PlayerCount == 1)
        {
            return;
        }

        foreach (var player in PhotonNetwork.PlayerList)
        {
            if (player != PhotonNetwork.LocalPlayer)
            {
                opponentPlayer = player;
                break;
            }
        }
    }
}