using AstroLab;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace AstroLab {
    public class InstrumentUpgrade : MonoBehaviour {

        [SerializeField] private InstrumentFlags m_Instrument;
        [SerializeField] private int m_RequiredXP;
        [SerializeField] private Button m_Button;

        private bool Acquired = false;

        void Start() {
            m_Button.onClick.AddListener(HandleClicked);
            GameMgr.Events.Register(GameEvents.UISwitched, HandleOpened);
        }

        public void HandleClicked() {
            if (PointsMgr.Instance.CurrentXP >= m_RequiredXP) {
                InstrumentsMgr.Instance.UnlockedInstruments |= m_Instrument;
                GameMgr.Events.Dispatch(GameEvents.InstrumentUnlocksChanged);
                Acquired = true;
                m_Button.interactable = false;
                m_Button.targetGraphic.color = Colors.CompletedColor;
            }
        }

        public void HandleOpened() {
            m_Button.interactable = !Acquired && PointsMgr.Instance.CurrentXP >= m_RequiredXP;
        }
        
    }
}