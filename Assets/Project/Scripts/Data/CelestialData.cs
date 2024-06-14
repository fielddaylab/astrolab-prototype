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
        [SerializeField] private string m_constellation;
        [SerializeField] private bool m_useRadianRADecl;
        [SerializeField] private float m_rightAscensionRadian;
        [SerializeField] private float m_declinationRadian;
        [SerializeField] private string m_identifyEntryID;

        [Header("Color")]
        [SerializeField] private string m_color;

        [Header("Visuals")]
        [SerializeField] private Sprite m_represent2D;
        [SerializeField] private bool m_useOverrideMat;
        [SerializeField] private Material m_overrideMat;
        [SerializeField] private bool m_useOverrideCOPrefab;
        [SerializeField] private GameObject m_overrideCOPrefab;
        [SerializeField] private bool m_useOverrideFocusVisual;
        [SerializeField] private GameObject m_overrideFocusVisual;


        public string Name { get { return m_name; } }
        public Vector3 RA { get { return m_rightAscension; } }
        public Vector3 Decl { get { return m_declination; } }
        public string Constellation { get { return m_constellation; } }
        public bool UseRadianRADecl { get { return m_useRadianRADecl; } }
        public float RARad { get { return m_rightAscensionRadian; } }
        public float DeclRad { get { return m_declinationRadian; } }
        public Sprite Represent2D { get { return m_represent2D; } }
        public bool UseOverrideMat { get { return m_useOverrideMat; } }
        public Material OverrideMat { get { return m_overrideMat; } }
        public bool UserOverrideCOPrefab { get { return m_useOverrideCOPrefab; } }
        public GameObject OverrideCOPrefab { get { return m_overrideCOPrefab; } }
        public bool UseOverrideFocusVisual { get { return m_useOverrideFocusVisual; } }
        public GameObject OverrideFocusVisual { get { return m_overrideFocusVisual; } }
        public string IdentifyEntryID { get { return m_identifyEntryID; } }
        public string Color { get { return m_color; } }
    }
}