using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AstroLab {

    public class SpaceSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject m_sampleObj;

        [Header("Degrees")]
        [SerializeField] private Vector3 m_sampleRA;
        [SerializeField] private Vector3 m_sampleDeclination;

        [Header("Radians")]
        [SerializeField] private float m_sampleRARadians;
        [SerializeField] private float m_sampleDeclinationRadians;

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

            var pos = CoordinateUtility.RadiansToCartesianCoordinates(m_sampleRARadians, m_sampleDeclinationRadians);
            m_sampleObj.transform.position = pos * skyboxDist;
        }

#endif // UNITY_EDITOR
    }
}