using CrossyRoad.PlayerInstanceNamespace;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace CrossyRoad.TileController
{
    public class ScoreboardItem : MonoBehaviour
    {
        [HideInInspector] public PlayerInstance represented;

        public TextMeshProUGUI nameText;
        public TextMeshProUGUI scoreText;

		void Start()
		{
			// Stats:
			nameText.text = represented.playerName;
		}
	}

}
