using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AstroLab {

    public class SpaceSpawner : MonoBehaviour
    {
        public static SpaceSpawner Instance;

        [SerializeField] private GameObject m_sampleObj;
        [SerializeField] private GameObject m_celestialObjPrefab;

        [SerializeField] private CelestialData[] m_initialSpawns;
        [SerializeField] private Transform m_spawnRoot;

        [SerializeField] private GameObject m_defaultFocusPrefab;
        [SerializeField] private Transform m_focusRoot;

        [Header("Degrees")]
        [SerializeField] private Vector3 m_sampleRA;
        [SerializeField] private Vector3 m_sampleDeclination;

        [Header("Radians")]
        [SerializeField] private float m_sampleRARadians;
        [SerializeField] private float m_sampleDeclinationRadians;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else if (this != Instance)
            {
                Destroy(this.gameObject);
                return;
            }

            foreach(var data in m_initialSpawns)
            {
                GameObject adjustedPrefab = m_celestialObjPrefab;
                // Apply prefab overrides
                if (data.UserOverrideCOPrefab)
                {
                    adjustedPrefab = data.OverrideCOPrefab;
                }

                // create object
                var newObj = Instantiate(adjustedPrefab, m_spawnRoot).GetComponent<CelestialObject>();
                newObj.Populate(data);
                newObj.gameObject.name = "CO: " + data.Name;
                 
                // Apply material overrides
                if (data.UseOverrideMat)
                {
                    var mats = newObj.MeshRenderer.sharedMaterials;
                    mats[0] = data.OverrideMat;
                    newObj.MeshRenderer.sharedMaterials = mats;
                }

                // create focus
                UIFocusable newFocusable = newObj.GetComponent<UIFocusable>();
                if (newFocusable)
                {
                    newFocusable.Init(newObj);

                    CreateFocusForFocusable(data, newFocusable);
                }
            }
        }

        private void CreateFocusForFocusable(CelestialData data, UIFocusable newFocusable)
        {
            UIFocus newFocus = null;
            if (data.UseOverrideFocusVisual && data.OverrideFocusVisual != null)
            {
                newFocus = Instantiate(data.OverrideFocusVisual, m_focusRoot).GetComponent<UIFocus>();
            }
            else
            {
                newFocus = Instantiate(m_defaultFocusPrefab, m_focusRoot).GetComponent<UIFocus>();
            }

            if (newFocus)
            {
                newFocus.Init(newFocusable);
            }
        }


        public void PositionObject(GameObject toPosition, Vector3 ra, Vector3 decl)
        {
            int skyboxDist = FindObjectOfType<GameConsts>().SkyboxDist;

            float raDegrees = (float)CoordinateUtility.RAToDegrees((int)ra.x, (int)ra.y, ra.z);
            float declDegrees = (float)CoordinateUtility.DeclensionToDecimalDegrees((int)decl.x, (int)decl.y, decl.z);
            var pos = CoordinateUtility.RAscDeclDegreesToCartesianCoordinates(raDegrees, declDegrees);
            toPosition.transform.position = pos * skyboxDist;
        }

        public void PositionObject(GameObject toPosition, float raRad, float declRad)
        {
            int skyboxDist = FindObjectOfType<GameConsts>().SkyboxDist;

            var pos = CoordinateUtility.RAscDeclRadiansToCartesianCoordinates(raRad, declRad);
            toPosition.transform.position = pos * skyboxDist;
        }

#if UNITY_EDITOR

        [ContextMenu("(Degrees) Place Obj At Location (Right Ascension + Declension)")]
        private void SampleLocation()
        {
            int skyboxDist = FindObjectOfType<GameConsts>().SkyboxDist;

            float raDegrees = (float)CoordinateUtility.RAToDegrees((int)m_sampleRA.x, (int)m_sampleRA.y, m_sampleRA.z);
            float declDegrees = (float)CoordinateUtility.DeclensionToDecimalDegrees((int)m_sampleDeclination.x, (int)m_sampleDeclination.y, m_sampleDeclination.z);
            var pos = CoordinateUtility.RAscDeclDegreesToCartesianCoordinates(raDegrees, declDegrees);
            m_sampleObj.transform.position = pos * skyboxDist;
        }

        [ContextMenu("(Radians) Place Obj At Location (Right Ascension + Declension)")]
        private void SampleLocationRadians()
        {
            int skyboxDist = FindObjectOfType<GameConsts>().SkyboxDist;

            var pos = CoordinateUtility.RAscDeclRadiansToCartesianCoordinates(m_sampleRARadians, m_sampleDeclinationRadians);
            m_sampleObj.transform.position = pos * skyboxDist;
        }

#endif // UNITY_EDITOR
    }
}