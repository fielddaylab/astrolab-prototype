using BeauUtil.Debugger;
using UnityEngine;
using UnityEngine.UI;

namespace AstroLab {

    public class PostcardPuzzle : MonoBehaviour {

        [SerializeField] private PuzzleData PuzzleData;

        [SerializeField] private PuzzleObject[] PuzzleObjects;
        [SerializeField] private Button m_FinishButton;
        public Button FinishButton { get { return m_FinishButton; }}

        private void Start() {
            m_FinishButton.onClick.AddListener(OnFinishClicked);
            m_FinishButton.interactable = false;
        }

        [ContextMenu("Populate Solutions from Refs")]
        public void SetObjSolutionFromRefs() {
            for (int i =0; i < PuzzleObjects.Length; i++) {
                PuzzleObjects[i].RefToSolutionVals();
            }
        }

        [ContextMenu("Populate Starting Vals")]
        public void PopulateStartingVals() {
            for (int i = 0; i < PuzzleObjects.Length; i++) {
                PuzzleObjects[i].PopulateStartingValues();
            }
        }

        public bool EvaluateSolved() {
            for (int i = 0; i < PuzzleObjects.Length; i++) {
                if (!PuzzleObjects[i].EvaluateSolved()) {
                    return false;
                }
            }
            m_FinishButton.interactable = true;
            return true;
        }

        public void OnFinishClicked() {
            Log.Warn("[PostcardPuzzle] Clicked Finish! TODO: submit puzzle");
        }

    }
}
