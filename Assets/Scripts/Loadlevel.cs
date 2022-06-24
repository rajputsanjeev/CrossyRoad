using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CrossyRoard
{
    public class Loadlevel : MonoBehaviour
    {

        public void LoadLevel(string levelName)
        {
            SceneController.LoadScene(levelName);
        }
    }
}

