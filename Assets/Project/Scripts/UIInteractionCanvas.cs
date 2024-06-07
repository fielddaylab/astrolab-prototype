using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AstroLab
{
    public class UIInteractionCanvas : MonoBehaviour
    {
        #region Inspector

        [Header("Buttons")]
        [SerializeField] private Button m_settingsButton;
        [SerializeField] private Button m_notebookButton;
        [SerializeField] private Button m_techButton;
        [SerializeField] private Button m_instrumentButton;

        [Space(5)]
        [Header("Orientation Sliders")]
        [SerializeField] private OrientSlider m_magSlider;
        [SerializeField] private OrientSlider m_ascensionSlider;
        [SerializeField] private OrientSlider m_declinationSlider;

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
        }

        #endregion // Unity Callbacks

        #region Handlers

        private void HandleSettingsClicked()
        {

        }

        private void HandleNotebookClicked()
        {

        }

        private void HandleTechClicked()
        {

        }

        private void HandleInstrumentsClicked()
        {

        }

        #endregion // Handlers
    }
}
