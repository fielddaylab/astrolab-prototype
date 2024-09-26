using BeauUtil.Debugger;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace AstroLab {

    public class PostcardPuzzle : MonoBehaviour {

        [SerializeField] private PuzzleData PuzzleData;

        [SerializeField] private PuzzleObject[] PuzzleObjects;
        [SerializeField] private TMP_Text[] Clues;
        [SerializeField] private Button m_FinishButton;
        public Button FinishButton { get { return m_FinishButton; }}

        private void Start() {
            m_FinishButton.onClick.AddListener(OnFinishClicked);
            m_FinishButton.interactable = false;

            GameMgr.Events.Register(GameEvents.DataSlotFilled, OnSlotFilled);

            if (PuzzleData != null) {
                PopulateFromPuzzleData();
            }
        }

        [ContextMenu("Populate From Puzzle Data")]
        public void PopulateFromPuzzleData() {
            for (int i = 0; i < PuzzleObjects.Length; i++) {
                PuzzleObjects[i].PuzzleRefPopulate(PuzzleData.PuzzleRefs[i]);
                if (i < Clues.Length && PuzzleData.Clues[i] != null) {
                    Clues[i].SetText(PuzzleData.Clues[i]);
                }
            }
        }

        [ContextMenu("Populate Solutions from Refs")]
        public void SetObjSolutionFromRefs() {
            for (int i = 0; i < PuzzleObjects.Length; i++) {
                PuzzleObjects[i].RefToSolutionVals();
            }
        }

        [ContextMenu("Populate Starting Vals")]
        public void PopulateStartingVals() {
            for (int i = 0; i < PuzzleObjects.Length; i++) {
                PuzzleObjects[i].PopulateStartingValues();
            }
        }

        private void OnSlotFilled() {
            bool allFilled = EvaluateFilled();
            m_FinishButton.interactable = allFilled;
            m_FinishButton.gameObject.SetActive(allFilled);
        }

        public bool EvaluateSolved() {
            for (int i = 0; i < PuzzleObjects.Length; i++) {
                if (!PuzzleObjects[i].EvaluateSolved()) {
                    return false;
                }
            }
            return true;
        }

        public bool EvaluateFilled() {
            for (int i = 0; i < PuzzleObjects.Length; i++) {
                if (!PuzzleObjects[i].EvaluateFilled()) {
                    return false;
                }
            }
            return true;
        }

        public void OnFinishClicked() {
            Log.Warn("[PostcardPuzzle] Clicked Finish!");

            ReviewQueue.Instance.AddNewPuzzleItem(this, 7, 5);

            if (EvaluateSolved()) {
                // Destroy(this);
                Log.Warn("[PostcardPuzzle] CORRECT! :D");
                return;
            }
            Log.Warn("[PostcardPuzzle] INCORRECT D:");
        }

        public void OnReviewComplete(bool correct)
        {
            if (correct)
            {
                Destroy(this.gameObject);
            }
        }
    }
}
