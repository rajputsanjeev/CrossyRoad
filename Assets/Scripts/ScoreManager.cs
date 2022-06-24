
using UnityEngine.UI;
using CrossyRoad.IGameListner;
using TMPro;
using CrossyRoard;
using UnityEngine;

namespace CrossyRoad.ScoreManagerNamespace
{
    public class ScoreManager : GameListner
    {
        public TextMeshProUGUI scoreText;
        public int score;

        public void UpdateScore(int score)
        {
            Debug.Log("Score " + score);
            this.score += score;
            scoreText.text = this.score.ToString();
        }
    }
}

