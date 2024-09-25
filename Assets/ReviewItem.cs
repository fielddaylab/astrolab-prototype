using BeauUtil.Debugger;
using BeauUtil.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AstroLab {
    public class ReviewItem : MonoBehaviour {

        [Header("Config")]
        [SerializeField] private Timer m_Timer;
        [SerializeField] private int m_Points;
        [SerializeField] private string m_Guess;
        [SerializeField] private CelestialObject m_RefObject;
        [Space(4)]

        [Header("Components")]
        [SerializeField] private ReviewQueue m_Queue;
        [SerializeField] private Button m_Button;
        [SerializeField] private EllipseGraphic m_TimeDial;
        [SerializeField] private Image m_Image;
        [SerializeField] private Graphic m_Background;

        void Start() {
            m_Button.interactable = false;
            m_Button.onClick.AddListener(HandleButtonPressed);
        }

        void Update() {
            if (m_Timer.Advance(Time.deltaTime)) {
                // complete item
                SetComplete(CheckCorrect());
                m_Timer.Paused = true;
            }
            m_TimeDial.ArcFill = (1 - m_Timer.GetProgress());
        }

        public void Populate(ReviewQueue queue, string guess, CelestialObject obj, float time, int pts) {
            m_Queue = queue;
            m_Timer = new Timer(time);
            m_Guess = guess;
            m_RefObject = obj;
            m_Points = pts;
            m_Image.sprite = obj.Data.Represent2D;
        }

        private bool CheckCorrect() {
            return m_Guess.Equals(m_RefObject.Data.IdentifyEntryID);
        }

        private void SetComplete(bool correct) {
            GameConsts consts = FindObjectOfType<GameConsts>();

            if (correct) {
                m_Background.color = consts.CorrectColor;
                m_RefObject.Identified = true;
                GameMgr.Events.Dispatch(GameEvents.CelestialObjIdentified);
                Log.Msg("Item identified! {0}", m_Guess);
            } else {
                m_Background.color = consts.IncorrectColor;
                Log.Msg("Incorrect identification :( It's actually {0}", m_RefObject.Data.IdentifyEntryID);
                m_Points = consts.IncorrectIDPenalty;
            }
            m_Button.interactable = true;
        }

        private void HandleButtonPressed() {
            PointsMgr.Instance.AddXP(m_Points);
            m_Queue.Items.Remove(this);
            Destroy(gameObject);
        }
    }
}

