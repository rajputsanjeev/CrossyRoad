using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Crossyroad 
{
    [CreateAssetMenu(menuName = "Canera/CameraSetting", fileName = "data")]
    public class CameraSetting : ScriptableObject
    {
        public float minZ = 0.0f;
        public float speedIncrementZ = 1.5f;
        public float speedOffsetZ = 4.0f;
    }

}

