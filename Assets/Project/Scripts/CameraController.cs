using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AstroLab
{
    [RequireComponent(typeof(Camera))]
    public class CameraController : MonoBehaviour
    {
        private Camera m_camera;

        [SerializeField] private Transform m_camRoot;

        [Space(5)]
        [Header("Look")]
        [SerializeField] private float m_lookThreshold; // margin from edge of screen (normalized)
        [SerializeField] private float m_lookRapidThreshold; // faster look threshold
        [SerializeField] private float m_lookSpeed;
        [SerializeField] private float m_lookRapidSpeed;
        [SerializeField] private Vector2 m_lookXClamp; // rotation limits in given direction (x is min X, y is max X)
        [SerializeField] private Vector2 m_lookYClamp; // rotation limits in given direction (x is min Y, y is max Y)

        [Space(5)]
        [Header("Zoom")]
        [SerializeField] private float m_zoomSpeed; // scroll wheel
        [SerializeField] private float m_zoomIncrement; // keyboard

        [SerializeField] private Vector2 m_zoomBounds; // x is min, y is max

        private float m_vertLook; // accumulated rotation vertically

        #region Unity Callbacks

        private void Start()
        {
            m_camera = GetComponent<Camera>();
            m_vertLook = m_camRoot.transform.localEulerAngles.x;
        }

        private void Update()
        {
            ProcessInputs();
        }

        #endregion // Unity Callbacks

        #region Input Processing

        private void ProcessInputs()
        {
            ProcessLook();

            ProcessZoom();
        }

        private void ProcessLook()
        {
            ProcessMouseLook();

            ProcessKeyboardLook();
        }

        private void ProcessMouseLook()
        {
            var cursorPos = m_camera.ScreenToViewportPoint(Input.mousePosition);

            // Look X
            if (cursorPos.x > 0 && cursorPos.x < m_lookThreshold)
            {
                // look left
                var newRotation = m_camRoot.localEulerAngles;

                var adjustedSpeed = (cursorPos.x > 0 && cursorPos.x < m_lookRapidThreshold) ? m_lookRapidSpeed : m_lookSpeed;
                newRotation.y = ClampAngle(newRotation.y - adjustedSpeed * Time.deltaTime, m_lookXClamp.x, m_lookXClamp.y);

                m_camRoot.localEulerAngles = newRotation;
            }
            else if (cursorPos.x < 1 && cursorPos.x > 1 - m_lookThreshold)
            {
                // look right
                var newRotation = m_camRoot.localEulerAngles;

                var adjustedSpeed = (cursorPos.x < 1 && cursorPos.x > 1 - m_lookRapidThreshold) ? m_lookRapidSpeed : m_lookSpeed;
                newRotation.y = ClampAngle(newRotation.y + adjustedSpeed * Time.deltaTime, m_lookXClamp.x, m_lookXClamp.y);

                m_camRoot.localEulerAngles = newRotation;
            }

            // Look Y
            if (cursorPos.y > 0 && cursorPos.y < m_lookThreshold)
            {
                // look down
                var adjustedSpeed = (cursorPos.y > 0 && cursorPos.y < m_lookRapidThreshold) ? m_lookRapidSpeed : m_lookSpeed;

                //m_camRoot.Rotate(adjustedSpeed * Time.deltaTime, 0, 0, Space.Self);
                m_vertLook += adjustedSpeed * Time.deltaTime;

                m_vertLook = ClampAngle(m_vertLook, m_lookYClamp.x, m_lookYClamp.y);

                var angles = m_camRoot.eulerAngles;
                angles.x = m_vertLook;
                m_camRoot.eulerAngles = angles;
            }
            else if (cursorPos.y < 1 && cursorPos.y > 1 - m_lookThreshold)
            {
                // look up
                var adjustedSpeed = (cursorPos.y < 1 && cursorPos.y > 1 - m_lookRapidThreshold) ? m_lookRapidSpeed : m_lookSpeed;
                //m_camRoot.Rotate(-adjustedSpeed * Time.deltaTime, 0, 0, Space.Self);
                m_vertLook -= adjustedSpeed * Time.deltaTime;

                m_vertLook = ClampAngle(m_vertLook, m_lookYClamp.x, m_lookYClamp.y);

                var angles = m_camRoot.eulerAngles;
                angles.x = m_vertLook;
                m_camRoot.eulerAngles = angles;
            }
        }

        private void ProcessKeyboardLook()
        {

        }

        private void ProcessZoom()
        {
            ProcessMouseZoom();

            ProcessKeyboardZoom();
        }

        private void ProcessMouseZoom()
        {
            var yScrollDelta = Input.mouseScrollDelta.y;
            if (yScrollDelta != 0)
            {
                float newZoom = m_camera.fieldOfView;

                // inverse relationship: as player scrolls updwards, fov decreases
                newZoom = Mathf.Clamp(newZoom - yScrollDelta * m_zoomSpeed, m_zoomBounds.x, m_zoomBounds.y);

                m_camera.fieldOfView = newZoom;
            }
        }

        private void ProcessKeyboardZoom()
        {
            if (Input.GetKeyDown(KeyCode.I))
            {
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

        private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
        {
            if (lfAngle < -360f)
            {
                lfAngle += 360f;
            }
            if (lfAngle > 360f)
            {
                lfAngle -= 360f;
            }

            return Mathf.Clamp(lfAngle, lfMin, lfMax);
        }
    }
}
