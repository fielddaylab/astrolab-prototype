using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AstroLab
{
    [CreateAssetMenu(menuName = "AstroLab/Create Celestial Object Data")]
    public class CelestialData : ScriptableObject
    {
        [SerializeField] private string m_name;
        [SerializeField] private Vector3 m_rightAscension;
        [SerializeField] private Vector3 m_declination;
        [SerializeField] private bool m_useRadianRADecl;
        [SerializeField] private float m_rightAscensionRadian;
        [SerializeField] private float m_declinationRadian;
        [SerializeField] private bool m_useOverrideFocusVisual;
        [SerializeField] private GameObject m_overrideFocusVisual;

        public string Name { get { return m_name; } }
        public Vector3 RA { get { return m_rightAscension; } }
        public Vector3 Decl { get { return m_declination; } }
        public bool UseRadianRADecl { get { return m_useRadianRADecl; } }
        public float RARad { get { return m_rightAscensionRadian; } }
        public float DeclRad { get { return m_declinationRadian; } }
        public bool UseOverrideFocusVisual { get { return m_useOverrideFocusVisual; } }
        public GameObject OverrideFocusVisual { get { return m_overrideFocusVisual; } }
    }
}