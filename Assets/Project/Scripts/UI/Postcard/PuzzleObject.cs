using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace AstroLab {

    public class PuzzleObject : MonoBehaviour {

        // NOTE FOR FINAL VERSION: if we can be sure that each slot will be different, it may make more sense to use a single "DataPayload"-like object for each star, rather than each slot having a whole DataPayload
        public DataSlot[] Slots;

        [SerializeField] public DataPayload StartingValues;
        [SerializeField] private DataPayload Solution;

        public void PopulateSlots() {

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
