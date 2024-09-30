using UnityEngine;

namespace AstroLab
{
    [RequireComponent(typeof(Camera))]
    public class CameraController : MonoBehaviour
    {
        private Camera m_camera;

        [SerializeField] private Transform m_camRoot;
        [SerializeField] private Transform m_playerRoot;
        public bool EnableMouseControls = true;
        public bool EnableMouseAutoControls = false;
        public bool EnableSmoothKeyboardControls = true;

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
        [SerializeField] private float m_smoothLookIncrement;
        [SerializeField] private Vector2 m_lookXClamp; // rotation limits in given direction (x is min X, y is max X)
        [SerializeField] private Vector2 m_lookYClamp; // rotation limits in given direction (x is min Y, y is max Y)
        [SerializeField] private float m_lookDragMod;

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

        private bool m_mouseDragLookActive;
        private Vector3 m_prevMousePos;

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

            GameMgr.Events.Register(GameEvents.UnfocusDown, HandleUnfocusDown);
            GameMgr.Events.Register(GameEvents.UnfocusUp, HandleUnfocusUp);

            m_mouseDragLookActive = false;
            EnableMouseControls = true;
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
            }
        }

        public void TryLook(Vector3 lookPos)
        {
            m_camRoot.LookAt(lookPos, m_playerRoot.up);
            var angles = this.transform.localEulerAngles;
            angles.x = 0;
            this.transform.localEulerAngles = angles;
        }

        #region Input Processing

        private void ProcessInputs()
        {
            ProcessLook();

            ProcessZoom();
        }

        private void ProcessLook()
        {
            if (EnableMouseAutoControls)
            {
                ProcessMouseAutoLook();
            }
            if (EnableMouseControls)
            {
                ProcessMouseDragLook();
            }

            if (EnableSmoothKeyboardControls) {
                ProcessKeyboardLookSmooth();
            } else {
                ProcessKeyboardLookDiscrete();
            }
        }

        private void ProcessMouseAutoLook()
        {
            var cursorPos = m_camera.ScreenToViewportPoint(Input.mousePosition);

            // Look X
            if (cursorPos.x < m_lookThreshold)
            {
                // look left
                var adjustedSpeed = (cursorPos.x < m_lookRapidThreshold) ? m_lookRapidSpeed : m_lookSpeed;
                AdjustHorizLook(-adjustedSpeed * Time.deltaTime);
            }
            else if (cursorPos.x > 1 - m_lookThreshold)
            {
                // look right
                var adjustedSpeed = (cursorPos.x > 1 - m_lookRapidThreshold) ? m_lookRapidSpeed : m_lookSpeed;
                AdjustHorizLook(adjustedSpeed * Time.deltaTime);
            }

            // Look Y
            if (cursorPos.y < m_lookThreshold)
            {
                // look down
                var adjustedSpeed = (cursorPos.y < m_lookRapidThreshold) ? m_lookRapidSpeed : m_lookSpeed;
                AdjustVertLook(adjustedSpeed * Time.deltaTime);
            }
            else if (cursorPos.y > 1 - m_lookThreshold)
            {
                // look up
                var adjustedSpeed = (cursorPos.y > 1 - m_lookRapidThreshold) ? m_lookRapidSpeed : m_lookSpeed;
                AdjustVertLook(-adjustedSpeed * Time.deltaTime);
            }
        }

        private void ProcessMouseDragLook()
        {
            if (m_mouseDragLookActive)
            {
                var cursorPos = m_camera.ScreenToViewportPoint(Input.mousePosition);
                var deltaPos = m_prevMousePos - cursorPos;

                // look horizontal
                AdjustHorizLook(deltaPos.x * m_lookDragMod);

                // look vertical
                AdjustVertLook(-deltaPos.y * m_lookDragMod);

                m_prevMousePos = cursorPos;
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

        private void ProcessKeyboardLookDiscrete()
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

        private void ProcessKeyboardLookSmooth() {
            if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W)) {
                // look up
                AdjustVertLook(-m_smoothLookIncrement);
            } else if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S)) {
                // look down
                AdjustVertLook(m_smoothLookIncrement);
            }
            if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A)) {
                // look left
                AdjustHorizLook(-m_smoothLookIncrement);
            } else if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D)) {
                // look right
                AdjustHorizLook(m_smoothLookIncrement);
            }
        }

        private void ProcessZoom()
        {
            if (EnableMouseControls)
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

        #region Handlers

        private void HandleUnfocusDown()
        {
            m_mouseDragLookActive = true;
            m_prevMousePos = m_camera.ScreenToViewportPoint(Input.mousePosition);
        }

        private void HandleUnfocusUp()
        {
            m_mouseDragLookActive = false;
            m_prevMousePos = Vector3.zero;
        }

        #endregion // Handlers
    }
}
