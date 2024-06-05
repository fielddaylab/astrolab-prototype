using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AstroLab {

    public class SpaceSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject m_sampleObj;

        [SerializeField] private float m_skyboxDist = 1000;

        [Header("Degrees")]
        [SerializeField] private Vector3 m_sampleRA;
        [SerializeField] private Vector3 m_sampleDeclination;

        [Header("Radians")]
        [SerializeField] private float m_sampleRARadians;
        [SerializeField] private float m_sampleDeclinationRadians;

#if UNITY_EDITOR

        [ContextMenu("Place Obj At Location (Right Ascension + Declension degrees)")]
        private void SampleLocation()
        {
            float raDegrees = (float)CoordinateUtility.RAToDegrees((int)m_sampleRA.x, (int)m_sampleRA.y, m_sampleRA.z);
            float declDegrees = (float)CoordinateUtility.DeclensionToDecimalDegrees((int)m_sampleDeclination.x, (int)m_sampleDeclination.y, m_sampleDeclination.z);
            var pos = CoordinateUtility.RAscDeclDegreesToCartesianCoordinates(raDegrees, declDegrees);
            m_sampleObj.transform.position = pos * m_skyboxDist;
        }

        [ContextMenu("Place Obj At Location (Right Ascension + Declension radians)")]
        private void SampleLocationRadians()
        {
            var pos = CoordinateUtility.RAscDeclRadiansToCartesianCoordinates(m_sampleRARadians, m_sampleDeclinationRadians);
            m_sampleObj.transform.position = pos * m_skyboxDist;
        }

#endif // UNITY_EDITOR
    }
}