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
        [SerializeField] private bool m_enableMouseControls;

        [SerializeField] private RenderTexture m_renderTex;
        [SerializeField] private Vector2Int m_defaultRenderTexDims;
        [SerializeField] private Canvas m_focusCanvas;

        [Space(5)]
        [Header("Look")]
        [SerializeField] private float m_lookThreshold; // margin from edge of screen (normalized)
        [SerializeField] private float m_lookRapidThreshold; // faster look threshold
        [SerializeField] private float m_lookSpeed;
        [SerializeField] private float m_lookRapidSpeed;
        [SerializeField] private float m_lookIncrement;
        [SerializeField] private Vector2 m_lookXClamp; // rotation limits in given direction (x is min X, y is max X)
        [SerializeField] private Vector2 m_lookYClamp; // rotation limits in given direction (x is min Y, y is max Y)

        [Space(5)]
        [Header("Zoom")]
        [SerializeField] private float m_zoomSpeed; // scroll wheel
        [SerializeField] private float m_zoomIncrement; // keyboard

        [SerializeField] private Vector2 m_zoomBounds; // x is min, y is max

        [Space(5)]
        [Header("Skyboxes")]
        [SerializeField] private Material m_fallbackMat;

        private float m_vertLook; // accumulated rotation vertically
        private float m_horizLook; // accumulated rotation horizontally

        #region Unity Callbacks

        private void Start()
        {
            m_camera = GetComponent<Camera>();
            m_vertLook = m_camRoot.transform.localEulerAngles.x;

            if (!GetComponent<Skybox>().material)
            {
                GetComponent<Skybox>().material = m_fallbackMat;
            }

            m_renderTex.Release();
            m_renderTex.width = m_defaultRenderTexDims.x;
            m_renderTex.height = m_defaultRenderTexDims.y;
            m_renderTex.Create();
        }

        private void Update()
        {
            UpdateRenderTexture();

            ProcessInputs();
        }

        #endregion // Unity Callbacks

        private void UpdateRenderTexture()
        {
            if (m_renderTex.width != Camera.main.pixelWidth || m_renderTex.height != Camera.main.pixelHeight)
            {
                Debug.Log("Updating render texture!");
                m_renderTex.Release();
                m_renderTex.width = Camera.main.pixelWidth;
                m_renderTex.height = Camera.main.pixelHeight;
                m_renderTex.Create();

                // m_focusCanvas.scaleFactor = Camera.main.pixelWidth / m_defaultRenderTexDims.x;
            }
        }

        #region Input Processing

        private void ProcessInputs()
        {
            ProcessLook();

            ProcessZoom();
        }

        private void ProcessLook()
        {
            if (m_enableMouseControls)
            {
                ProcessMouseLook();
            }

            ProcessKeyboardLook();
        }

        private void ProcessMouseLook()
        {
            var cursorPos = m_camera.ScreenToViewportPoint(Input.mousePosition);

            // Look X
            if (/*cursorPos.x > 0 && */cursorPos.x < m_lookThreshold)
            {
                // look left
                var adjustedSpeed = (/*cursorPos.x > 0 && */cursorPos.x < m_lookRapidThreshold) ? m_lookRapidSpeed : m_lookSpeed;
                AdjustHorizLook(-adjustedSpeed * Time.deltaTime);
            }
            else if (/*cursorPos.x < 1 && */cursorPos.x > 1 - m_lookThreshold)
            {
                // look right
                var adjustedSpeed = (/*cursorPos.x < 1 && */cursorPos.x > 1 - m_lookRapidThreshold) ? m_lookRapidSpeed : m_lookSpeed;
                AdjustHorizLook(adjustedSpeed * Time.deltaTime);
            }

            // Look Y
            if (/*cursorPos.y > 0 && */cursorPos.y < m_lookThreshold)
            {
                // look down
                var adjustedSpeed = (/*cursorPos.y > 0 && */cursorPos.y < m_lookRapidThreshold) ? m_lookRapidSpeed : m_lookSpeed;
                AdjustVertLook(adjustedSpeed * Time.deltaTime);
            }
            else if (/*cursorPos.y < 1 && */cursorPos.y > 1 - m_lookThreshold)
            {
                // look up
                var adjustedSpeed = (/*cursorPos.y < 1 && */cursorPos.y > 1 - m_lookRapidThreshold) ? m_lookRapidSpeed : m_lookSpeed;
                AdjustVertLook(-adjustedSpeed * Time.deltaTime);
            }
        }

        private void AdjustVertLook(float adjustment)
        {
            m_vertLook += adjustment;

            m_vertLook = ClampAngle(m_vertLook, m_lookYClamp.x, m_lookYClamp.y);

            var angles = m_camRoot.localEulerAngles;
            angles.x = m_vertLook;
            m_camRoot.localEulerAngles = angles;
        }

        private void AdjustHorizLook(float adjustment)
        {
            m_horizLook += adjustment;

            m_horizLook = ClampAngle(m_horizLook, m_lookXClamp.x, m_lookXClamp.y);

            var angles = m_camRoot.localEulerAngles;
            angles.y = m_horizLook;
            m_camRoot.localEulerAngles = angles;
        }

        private void ProcessKeyboardLook()
        {
            if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
            {
                // look up
                AdjustVertLook(-m_lookIncrement);
            }
            if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
            {
                // look down
                AdjustVertLook(m_lookIncrement);
            }
            if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
            {
                // look left
                AdjustHorizLook(-m_lookIncrement);
            }
            if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
            {
                // look right
                AdjustHorizLook(m_lookIncrement);
            }
        }

        private void ProcessZoom()
        {
            if (m_enableMouseControls)
            {
                ProcessMouseZoom();
            }

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
