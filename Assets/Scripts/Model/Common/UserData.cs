using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

namespace CrossyRoad
{
    /// <summary>
    /// This class stores all User Info of local player
    /// </summary>
    public class UserData : MonoBehaviour
    {
        public static UserData Instance;
        public static string UserName { get; set; }

        private void Awake()
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }

    }
}