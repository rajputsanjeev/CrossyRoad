using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Constants
{
    #region Scenes
    public static string InitScene = "Init";
    public static string ServerScene = "Server";
    public static string HomeScene = "Home";
    public static string LoginScene = "Login";
    public static string GameScene = "GameScene";
    #endregion

    #region Screens
    public static string LobbyPanelScreen = "LobbyPanel";
    #endregion

    #region Photon
    public static byte MaxPlayersInGame = 2; //to test single device 2 player game : make this as 1
    public const int MAX_PLAYERS_IN_LOBBY = 8;
    public static string RANKED_LOBBY_NAME = "RankedLobby";
    public const int MAX_RANDOM_ROOM_JOIN_ATTEMPT = 50;
    #endregion

    #region Audio
    public const string BUTTON_CLICK_CLIP = "ButtonClick";
    public const string HOME_BG = "Home";

    public const string GAME_BG = "Game";
    public const string FLOOR_OPENING = "FloorOpening";
    public const string FIREWORKS = "FireWorks";
    public const string TOSS_SPIN = "TossSpin";

    public const float MAX_PARTICLE_VOLUME = 0.3f;
    #endregion

    #region Limits
    public static int MAX_STAMINA = 100;
    public static int CONNECTION_RETRIES_LIMIT = 4;
    public static int MAX_ROUNDS_IN_GAME = 3;
    #endregion

    #region GamePlayValues
    public const int TEAM_BABY_SLOT = 3;
    #endregion
  
    #region BuildSettings
    public static bool IsTestBuild = false;
    public static bool IsAITest = false; //to test single device 2 player game:  make this as true
    #endregion


    public const string EDITOR_BASE_URL = "https://www.blockbabies.world/staging/elp/api.php";
    public const string LIVE_BASE_URL = "https://www.blockbabies.world/elp/api.php";
   
    #region PlayerPrefs
    public const string SFX = "SFX";
    public const string MUSIC = "MUSIC";
    public const string PP_VERSION = "Version";

    public static string PP_PLAYER_INFO = "PlayerInfo";

    public static string PP_IS_AUTHENTICATED = "IsAuth";
    #endregion



}
