using CrossyRoad;
using UnityEngine;

namespace CrossyRoard
{
    public static class MyEventArgs
    { 
        public static class UIEvents
        {
            public static MyEvent<int> updateScore = new MyEvent<int>();
            public static MyEvent<PlayerStatus> OnPlayerDie = new MyEvent<PlayerStatus>();
            public static MyEvent<bool> IsMoveAble = new MyEvent<bool>();
            public static MyEvent<Transform, bool> cameraTransforms = new MyEvent<Transform , bool>();
            public static MyEvent<bool> initTileManager = new MyEvent<bool>();
            public static MyEvent<Transform> PlayerMove = new MyEvent<Transform>();
            public static MyEvent<bool> StartGame = new MyEvent<bool>();
            public static MyEvent<bool> transferMasterPlayerTransform = new MyEvent<bool>();
        }

    }
}