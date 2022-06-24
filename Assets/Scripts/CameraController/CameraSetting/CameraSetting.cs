using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CrossyRoad
{
    [CreateAssetMenu(menuName = "Canera/CameraSetting", fileName = "data")]
    public class CameraSetting : ScriptableObject
    {
        public float minZ = 0.0f;
        public float speedIncrementZ = 1.5f;
        public float speedOffsetZ = 1f;
        public float speedOffsetZPlayerMove = 10.0f;
        public float distance = 50f;
        public float deathZone = 20f;
    }

}

