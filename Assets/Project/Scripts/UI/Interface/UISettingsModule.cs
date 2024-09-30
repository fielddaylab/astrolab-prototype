using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AstroLab
{
    public class UISettingsModule : UIInterfaceModule
    {
        [Space(5)]
        [Header("Settings")]
        [SerializeField] private Button m_closeButton;

        [Header("Controls")]
        [SerializeField] private Toggle m_mouseControlToggle;
        [SerializeField] private Toggle m_mouseControlAutoToggle;
        [SerializeField] private Toggle m_smoothKeyboardLook;
        [SerializeField] private CameraController m_camController;

        public override void Init()
        {
            base.Init();

            m_closeButton.onClick.AddListener(HandleCloseClicked);

            m_mouseControlToggle.isOn = m_camController.EnableMouseControls;
            m_mouseControlToggle.onValueChanged.AddListener(HandleMouseControlToggleChanged);

            m_mouseControlAutoToggle.isOn = m_camController.EnableMouseAutoControls;
            m_mouseControlAutoToggle.onValueChanged.AddListener(HandleMouseAutoControlToggleChanged);

            m_smoothKeyboardLook.isOn = m_camController.EnableSmoothKeyboardControls;
            m_smoothKeyboardLook.onValueChanged.AddListener(HandleSmoothKeyboardControlToggleChanged);

            m_mouseControlToggle.isOn = true;
            m_smoothKeyboardLook.isOn = true;
        }

        public override void Open()
        {
            base.Open();
        }

        public override void Close()
        {
            base.Close();
        }

        #region Handlers

        private void HandleCloseClicked()
        {
            this.Close();
        }

        private void HandleMouseControlToggleChanged(bool newVal) {
            m_camController.EnableMouseControls = newVal;
        }

        private void HandleMouseAutoControlToggleChanged(bool newVal) {
            m_camController.EnableMouseAutoControls = newVal;
        }

        private void HandleSmoothKeyboardControlToggleChanged(bool newVal) {
            m_camController.EnableSmoothKeyboardControls = newVal;
        }
        #endregion // Handlers
    }
}
