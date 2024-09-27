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
        [SerializeField] private CelestialObject m_RefCelestialObject;
        [SerializeField] private PostcardPuzzle m_RefPuzzleObject; // TODO: instead of these explicit checks for celestial vs. puzzle, unify under an IReviewable interface
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

        public void PopulateCelestial(ReviewQueue queue, string guess, CelestialObject obj, float time, int pts) {
            m_Queue = queue;
            m_Timer = new Timer(time);
            m_Guess = guess;
            m_RefCelestialObject = obj;
            m_Points = pts;
            m_Image.sprite = obj.Data.Represent2D;
        }

        public void PopulatePuzzle(ReviewQueue queue, PostcardPuzzle obj, float time, int pts)
        {
            m_Queue = queue;
            m_Timer = new Timer(time);
            m_RefPuzzleObject = obj;
            m_Points = pts;
            GameConsts consts = FindObjectOfType<GameConsts>();
            m_Image.sprite = consts.PostcardIcon;
        }

        private bool CheckCorrect() {
            // TODO: instead of these explicit checks for celestial vs. puzzle, unify under an IReviewable interface
            if (m_RefCelestialObject) {
                // celestial object
                return m_Guess.Equals(m_RefCelestialObject.Data.IdentifyEntryID);
            }
            else if (m_RefPuzzleObject) {
                // postcard puzzle
                return m_RefPuzzleObject.EvaluateSolved();
            }

            return false;
        }

        private void SetComplete(bool correct) {

            if (correct) {
                if (m_RefCelestialObject) {
                    m_Background.color = Colors.CorrectColor;
                    m_RefCelestialObject.Identified = true;
                    GameMgr.Events.Dispatch(GameEvents.CelestialObjIdentified);
                    Log.Msg("Item identified! {0}", m_Guess);
                }
                else if (m_RefPuzzleObject) {
                    Log.Msg("Puzzle completed successfully!");
                    m_RefPuzzleObject.OnReviewComplete(true);
                }
            } else {
                GameConsts consts = FindObjectOfType<GameConsts>();
                if (m_RefCelestialObject)
                {
                    m_Background.color = Colors.IncorrectColor;
                    Log.Msg("Incorrect identification :( It's actually {0}", m_RefCelestialObject.Data.IdentifyEntryID);
                    m_Points = consts.IncorrectIDPenalty;
                }
                else if (m_RefPuzzleObject)
                {
                    Log.Msg("Puzzle completed unsuccessfully :(");
                    m_RefPuzzleObject.OnReviewComplete(false);
                    m_Points = consts.IncorrectIDPenalty;
                }
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

