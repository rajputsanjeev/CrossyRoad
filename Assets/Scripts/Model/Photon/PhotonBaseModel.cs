
using ExitGames.Client.Photon;

using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using CrossyRoad;
public class PhotonBaseModel : MonoBehaviour
{
    public RoomOptions GetRoomOptionsRandom(RoomType roomType)
    {
        RoomProperties roomProperties = GetRoomProperties(roomType);

        RoomOptions options = new RoomOptions();
        options.CustomRoomProperties = new Hashtable { { "CameraPos", 0  } };
        options.PlayerTtl = 1000;
        options.EmptyRoomTtl = 0;
        options.MaxPlayers = 2;
        options.PublishUserId = true;
        return options;
    }

    public RoomProperties GetRoomProperties(RoomType roomType)
    {
        switch (roomType)
        {
            case RoomType.RANDOM:
                return RandomRoomProperties();
        }

        return null;
    }


    private RoomProperties RandomRoomProperties()
    {
        RoomProperties properties = new RoomProperties()
        {
            maxPlayers = Constants.MaxPlayersInGame,
            roomType = RoomType.RANDOM //this room won't be displayed in rooms list in home screen
        };

        return properties;
    }


    protected void SetUserPlayerProperties()
    {
        //string level = Utils.IsEmpty((BaseModel.User.leadbaby.level).ToString()) ? "0" : BaseModel.User.leadbaby.level.ToString();
        //SetPP("leadBaby", BaseModel.User.leadbaby.image);
        //SetPP("profileImage", BaseModel.User.userinfo.profile_image);
        //SetPP("databaseId", BaseModel.User.userinfo.id.ToString());
        //SetPP("Challenge", ChallengeStatus.CHALLENGE);
        //SetPP("level", level);
    }

    protected void SetPP(object key, object value)
    {
        Hashtable hash = new Hashtable();
        hash.Add(key, value);

        PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
    }
}


[System.Serializable]
public class RoomProperties
{
    public byte maxPlayers;
    public string roomName;
    public string playerLevel;
    public int istrainee;
    public RoomType roomType;

    public Hashtable ToHashTable()
    {
        Hashtable hash = new Hashtable();
        hash.Add("type", roomType.ToString());
        hash.Add("lbt", istrainee.ToString());
        hash.Add("playerLevel", playerLevel);
        return hash;
    }
    public Hashtable ToHashTableForRandom()
    {
        Hashtable hash = new Hashtable();
        hash.Add("C1", istrainee);
        hash.Add("C2", playerLevel);
        return hash;
    }

}