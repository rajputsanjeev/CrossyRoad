using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CrossyRoad.PlayerInstanceNamespace
{
    public class PlayerInstance
    {
        public int playerID;                            // unique player ID (bots have their own player ID's different from the host's/MasterClient's)
        public string playerName;
        public int score;
        public Player punPlayer { get; protected set; }
        public bool isMine { get; protected set; }
        public bool isMasterClient;

        public PlayerInstance(int playerID , string playerName , bool isMine ,int score , bool isMasterClient ,Player player) 
        {
            this.playerID = playerID;   
            this.playerName = playerName;
            this.score = score;
            this.punPlayer = player;
            this.isMine = isMine;   
            this.isMasterClient = isMasterClient;
        }

        public void AddScore()
        {
            this.score = this.score + 1;
            UpdateStats();
        }

        public void SetStats(int score)
        {
             this.score = score;
        }

        public void UpdateStats()
        {
            ExitGames.Client.Photon.Hashtable h = new ExitGames.Client.Photon.Hashtable();
            if (score != 0) h.Add("Score", score);
            punPlayer.SetCustomProperties(h);
        }
    }
}
