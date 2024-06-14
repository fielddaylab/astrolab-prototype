using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AstroLab
{
    public class UIInteractionCanvas : MonoBehaviour
    {
        #region Inspector

        [Header("Main Buttons")]
        [SerializeField] private Button m_settingsButton;
        [SerializeField] private Button m_notebookButton;
        [SerializeField] private Button m_techButton;
        [SerializeField] private Button m_instrumentButton;

        [Space(5)]
        [Header("Orientation Sliders")]
        [SerializeField] private OrientSlider m_magSlider;
        [SerializeField] private OrientSlider m_ascensionSlider;
        [SerializeField] private OrientSlider m_declinationSlider;

        [Space(5)]
        [Header("Modules")]
        [SerializeField] private UIInterfaceModuleHub m_hub;

        #endregion // Inspector

        #region Unity Callbacks

        private void Awake()
        {
            m_settingsButton.onClick.AddListener(HandleSettingsClicked);
            m_notebookButton.onClick.AddListener(HandleNotebookClicked);
            m_techButton.onClick.AddListener(HandleTechClicked);
            m_instrumentButton.onClick.AddListener(HandleInstrumentsClicked);

        }

        private void Start()
        {
            GetComponent<Canvas>().worldCamera = GameMgr.Instance.InterfaceCamera;
            GameMgr.Events.Register<UIFocusable>(GameEvents.FocusableClicked, HandleFocusableClicked);
        }

        #endregion // Unity Callbacks

        #region Handlers

        private void HandleSettingsClicked()
        {
            m_hub.OpenUI(UIID.Settings);
        }

        private void HandleNotebookClicked()
        {
            m_hub.OpenUI(UIID.Notebook);
        }

        private void HandleTechClicked()
        {
            m_hub.OpenUI(UIID.Tech);
        }

        private void HandleInstrumentsClicked()
        {
            m_hub.OpenUI(UIID.Instruments);
        }

        private void HandleFocusableClicked(UIFocusable focusable)
        {
            FocusMgr.Instance.LastSelectedFocusable = focusable;
            m_hub.OpenUI(UIID.Instruments);
        }

        #endregion // Handlers
    }
}
