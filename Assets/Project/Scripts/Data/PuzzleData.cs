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
        [SerializeField] private PuzzleObjectRef[] m_PuzzleRefs;
        [SerializeField] private string[] m_Clues;


        public PuzzleObjectRef[] PuzzleRefs { get { return m_PuzzleRefs; } }
        public string[] Clues { get { return m_Clues; } }
    }


}