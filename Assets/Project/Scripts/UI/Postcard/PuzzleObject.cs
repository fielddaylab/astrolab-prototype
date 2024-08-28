using UnityEngine;

namespace AstroLab {

    public class PuzzleObject : MonoBehaviour {

        // NOTE FOR FINAL VERSION: if we can be sure that each slot will be different, it may make more sense to use a single "DataPayload"-like object for each star, rather than each slot having a whole DataPayload
        public DataSlot[] Slots;

        [SerializeField] public DataPayload StartingValues;
        [SerializeField] private DataPayload Solution;

        [SerializeField] private CelestialData RefObject;

        [ContextMenu("Populate Starting Values")]
        public void PopulateStartingValues() {
            for (int i = 0; i < Slots.Length; i++) {
                switch (Slots[i].SlotType) {
                    case DraggableFlags.Name:
                        Slots[i].SetData(new DataPayload(StartingValues.Name), true);
                        break;
                    case DraggableFlags.Coords:
                        Slots[i].SetData(new DataPayload(StartingValues.Coordinates), true);
                        break;
                    case DraggableFlags.Color:
                        Slots[i].SetData(new DataPayload(StartingValues.Color), true);
                        break;
                    case DraggableFlags.Magnitude:
                        Slots[i].SetData(new DataPayload(StartingValues.Magnitude), true);
                        break;
                    case DraggableFlags.Spectrum:
                        Slots[i].SetData(new DataPayload(StartingValues.Spectrum), true);
                        break;
                    default:
                        break;
                }
            }
        }

        [ContextMenu("Populate Starting Values From Ref")]
        private void RefToStartingVals() {
            if (RefObject == null) return;
            DataPayload refVals = new DataPayload() { };
            refVals.Name = RefObject.Name;
            refVals.Coordinates = new EqCoordinates(RefObject.RA, RefObject.Decl);
            refVals.Color = RefObject.OverrideMat.color;
            refVals.Magnitude = RefObject.Magnitude;
            //newVals.Spectrum = RefObject.Spectrum;
            StartingValues = refVals;
            PopulateStartingValues();
        }

        [ContextMenu("Populate Solution Values From Ref")]
        public void RefToSolutionVals() {
            if (RefObject == null) return;
            DataPayload refVals = new DataPayload() { };
            refVals.Name = RefObject.Name;
            refVals.Coordinates = new EqCoordinates(RefObject.RA, RefObject.Decl);
            refVals.Color = RefObject.OverrideMat.color;
            refVals.Magnitude = RefObject.Magnitude;
            //newVals.Spectrum = RefObject.Spectrum;
            Solution = refVals;
        }

        public bool EvaluateSolved() {
            // iterate through slots: if any don't match the solution, return false
            for (int i = 0; i < Slots.Length; i++) {
                switch (Slots[i].SlotType) {
                    case DraggableFlags.Name:
                        if (Slots[i].FilledData.Name != Solution.Name) {
                            return false;
                        }
                        break;
                    case DraggableFlags.Coords:
                        if (!Slots[i].FilledData.Coordinates.Equals(Solution.Coordinates)) {
                            return false;
                        }
                        break;
                    case DraggableFlags.Color:
                        if (!Slots[i].FilledData.Color.Equals(Solution.Color)) {
                            return false;
                        }
                        break;
                    case DraggableFlags.Magnitude:
                        if (!Slots[i].FilledData.Magnitude.Equals(Solution.Magnitude)) {
                            return false;
                        }
                        break;
                    case DraggableFlags.Spectrum:
                        if (!Slots[i].FilledData.Spectrum.Equals(Solution.Spectrum)) {
                            return false;
                        }
                        break;
                    default:
                        break;
                }
            }
            return true;
        }
    }
}
