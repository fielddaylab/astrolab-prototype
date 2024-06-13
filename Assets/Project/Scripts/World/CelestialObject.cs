using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AstroLab
{
    public class CelestialObject : MonoBehaviour
    {
        private CelestialData m_data;

        public CelestialData Data { get { return m_data; } }

        public Renderer MeshRenderer;
        public MeshFilter MeshFilter;

        public bool Identified;

        public void Populate(CelestialData data, bool setInitialPos = true)
        {
            m_data = data;

            if (setInitialPos) { SetToInitialPos(); }
        }

        public void SetToInitialPos()
        {
            if (Data.UseRadianRADecl)
            {
                SpaceSpawner.Instance.PositionObject(this.gameObject, m_data.RARad, m_data.DeclRad);
            }
            else
            {
                SpaceSpawner.Instance.PositionObject(this.gameObject, m_data.RA, m_data.Decl);
            }
        }
    }
}