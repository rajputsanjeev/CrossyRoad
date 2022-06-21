using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using Photon.Pun.UtilityScripts;

namespace Crossyroad
{
    public class CountdownPanel : UIGamePanelComponent
    {
        public enum CountDownSounds { COUNTDOWN1, COUNTDOWN2, COUNTDOWN3 };

        [SerializeField] private int m_maxCountDown;
        [SerializeField] private TextMeshProUGUI m_CountdownText;
        [SerializeField] private CountdownPhotonTimer photonTimer;
        private int lastTimeCount = 0;
        private bool isTimerFinish;

        private void OnEnable()
        {
            photonTimer.OnCountdownTimerHasExpired += CounterFinish;
            photonTimer.OnCountdownUpdated += OnCountdownUpdated;
        }


        public void Play()
        {
            gameObject.SetActive(true);
            photonTimer.StartTime();
        }


        private void OnDisable()
        {
            photonTimer.OnCountdownTimerHasExpired -= CounterFinish;
            photonTimer.OnCountdownUpdated -= OnCountdownUpdated;
            photonTimer.StopTimer();
        }

        IEnumerator StartCountdown()
        {
            int time = m_maxCountDown;

            while (time > 0)
            {
                m_CountdownText.text = time.ToString();
                yield return new WaitForSeconds(2f);
                time--;
            }
        }


        private void OnCountdownUpdated(string time)
        {
            if (Utils.IsEmpty(time)) return;

            int timeCount = int.Parse(time);

            if (timeCount == 0)
                return;

            if (timeCount % 2 != 0) return;

            if (lastTimeCount == timeCount) return;

            //Debug.Log("TimeCount " + timeCount);
            m_CountdownText.text = m_maxCountDown--.ToString();
            lastTimeCount = timeCount;

            string sound = ((CountDownSounds)m_maxCountDown).ToString();
        }

        private void CounterFinish()
        {
            if (isTimerFinish)
                return;

            isTimerFinish = true;

            // Debug.Log("onCountDown Finish");
            if (gameEventListener != null)
            {
                gameEventListener.OnCountdownCompleted();

            }
            this.gameObject.SetActive(false);
        }
    }

}
