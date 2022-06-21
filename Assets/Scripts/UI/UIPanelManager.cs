using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace Crossyroad
{
    public class UIPanelManager : MonoBehaviour
    {
        [SerializeField] private List<UIPanelComponent> m_PanelsList;

        [Tooltip("Add a panel here if it will be on the scene from starting by default")]
        [SerializeField] private UIPanelComponent m_LastActivePanel;

        private UIPanelComponent m_LastActiveScreen;

        public static UIPanelManager Instance;
        public List<UIPanelComponent> PanelList => m_PanelsList;
        public Panel CurrentPanel => m_LastActivePanel.panelType;
        public Panel CurrentScreen => m_LastActiveScreen.panelType;

        public UIPanelComponent GetPanelFromType(Panel type)
        {
            return m_PanelsList.Find(x => x.panelType == type);
        }

        public int PanelCount => m_PanelsList.Count;

        private void Awake()
        {
            Instance = this;
            m_LastActiveScreen = m_LastActivePanel;
        }

        private void OnEnable()
        {
            if (m_LastActivePanel != null)
            {
                // Show(m_LastActivePanel.panelType);
            }
        }

        public void ShowPanel(Panel type, bool bol)
        {
            UIPanelComponent desiredPanel = m_PanelsList.Find(x => x.panelType == type);
            desiredPanel.Show(bol);
        }
        public void Show(Panel type, bool isPopUp = false)
        {
            if (type == Panel.HOME && m_LastActivePanel.panelType == Panel.HOME)
            {
                return;
            }

            UIPanelComponent desiredPanel = m_PanelsList.Find(x => x.panelType == type);
            if (desiredPanel != null)
            {
                Sequence sequence;

                Debug.Log("false4");
                sequence = DOTween.Sequence();
                foreach (var item in m_PanelsList)
                {
                    Debug.Log("false5");
                    sequence.AppendCallback(() => item.Show(false));
                }
                sequence.AppendCallback(() => desiredPanel.Show(true));
                sequence.Play();
                m_LastActivePanel = desiredPanel;
                m_LastActiveScreen = desiredPanel;

                if (isPopUp)
                {
                    desiredPanel.Show(true);
                    if (!desiredPanel.panelType.Equals(Panel.SETTINGS))
                    {
                        m_LastActivePanel = desiredPanel;
                    }
                }
            }

            if (m_LastActivePanel.panelType == type) return;
        }
    }

}
