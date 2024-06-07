using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AstroLab
{
    [RequireComponent(typeof(Camera))]
    public class CameraController : MonoBehaviour
    {
        private Camera m_camera;
        [SerializeField] private float m_zoomSpeed; // scroll wheel
        [SerializeField] private float m_zoomIncrement; // keyboard

        [SerializeField] private Vector2 m_zoomBounds; // x is min, y is max

        #region Unity Callbacks

        private void Start()
        {
            m_camera = GetComponent<Camera>();
        }

        private void Update()
        {
            ProcessInputs();
        }

        #endregion // Unity Callbacks

        #region Input Processing

        private void ProcessInputs()
        {
            ProcessZoom();
        }

        private void ProcessZoom()
        {
            // SCROLL WHEEL
            var yScrollDelta = Input.mouseScrollDelta.y;
            if (yScrollDelta != 0)
            {
                float newZoom = m_camera.fieldOfView;

                // inverse relationship: as player scrolls updwards, fov decreases
                newZoom = Mathf.Clamp(newZoom - yScrollDelta * m_zoomSpeed, m_zoomBounds.x, m_zoomBounds.y);

                m_camera.fieldOfView = newZoom;
            }

            // KEYBOARD
            if (Input.GetKeyDown(KeyCode.I)) {
                // Zoom in
                float newZoom = m_camera.fieldOfView;

                newZoom = Mathf.Clamp(newZoom - m_zoomIncrement, m_zoomBounds.x, m_zoomBounds.y);

                m_camera.fieldOfView = newZoom;
            }
            if (Input.GetKeyDown(KeyCode.K))
            {
                // Zoom out
                float newZoom = m_camera.fieldOfView;

                newZoom = Mathf.Clamp(newZoom + m_zoomIncrement, m_zoomBounds.x, m_zoomBounds.y);

                m_camera.fieldOfView = newZoom;
            }
        }

        #endregion // Input Processing
    }
}
