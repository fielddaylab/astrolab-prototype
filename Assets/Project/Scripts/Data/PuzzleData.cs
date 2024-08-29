using System;
using System.Diagnostics.Contracts;
using UnityEngine;

namespace AstroLab {
    [Serializable]
    public struct PuzzleObjectRef {
        public CelestialData RefObject;
        public DraggableFlags ShowProperties;
    }

    [CreateAssetMenu(menuName = "AstroLab/Create Puzzle Data")]

    public class PuzzleData : ScriptableObject {
        [SerializeField] private PuzzleObjectRef[] m_PuzzleObjects;
        [SerializeField] private string[] m_Clues;
    }


}