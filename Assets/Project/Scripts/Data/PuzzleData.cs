using System.Diagnostics.Contracts;
using UnityEngine;

namespace AstroLab {
    [CreateAssetMenu(menuName = "AstroLab/Create Puzzle Data")]

    public class PuzzleData : ScriptableObject {
        [SerializeField] private DataPayload[] m_PuzzleObjects;
        [SerializeField] private DataPayload[] m_Solution;

        public DataPayload[] PuzzleObjects { get { return m_PuzzleObjects; } }
        public DataPayload[] Solution { get { return m_Solution; } }

    }
}