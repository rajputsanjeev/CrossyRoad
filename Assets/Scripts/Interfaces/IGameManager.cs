using CrossyRoad.PlayerInstanceNamespace;
using CrossyRoad.TileController;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CrossyRoad
{
    public interface IGameManager
    {
        void OnCountdownCompleted();
        PlayerInstance GetPlayerInstance(string playerName);
        PlayerInstance GetPlayerInstance(int playerId);


    }
}

