using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Crossyroad {
    [CreateAssetMenu(menuName = "Player/setting", fileName = "data")]
    public class PlayerSetting : ScriptableObject
    {
        public float timeForMove = 0.2f;
        public float jumpHeight = 1.0f;

        public int minX = -4;
        public int maxX = 4;

        public float leftRotation = -45.0f;
        public float rightRotation = 90.0f;
        public float elapsedTime = 0f;
        public float xJumpDistance = 1.5f;
    }


}

