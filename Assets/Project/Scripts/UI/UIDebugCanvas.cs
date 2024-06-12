using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AstroLab
{
    public class UIDebugCanvas : MonoBehaviour
    {
        [SerializeField] private Button m_openButton;
        [SerializeField] private Button m_closeButton;

        [SerializeField] private GameObject m_mainPanel;

        [Header("Instruments")]
        [SerializeField] private Toggle m_equatorialToggle;
        [SerializeField] private Toggle m_photometerToggle;
        [SerializeField] private Toggle m_spectrometerToggle;
        [SerializeField] private Toggle m_colorToggle;

        [Header("Notebook")]
        [SerializeField] private Toggle m_constellationToggle;
        [SerializeField] private Toggle m_planetsToggle;
        [SerializeField] private Toggle m_mainStarsToggle;
        [SerializeField] private Toggle m_otherStarsToggle;
        [SerializeField] private Toggle m_nebulaeToggle;
        [SerializeField] private Toggle m_galaxiesToggle;

        private void Start()
        {
            m_openButton.onClick.AddListener(HandleOpenClicked);
            m_closeButton.onClick.AddListener(HandleCloseClicked);

            m_equatorialToggle.isOn = InstrumentsMgr.Instance.AreInstrumentsUnlocked(InstrumentFlags.EquatorialCoords);
            m_photometerToggle.isOn = InstrumentsMgr.Instance.AreInstrumentsUnlocked(InstrumentFlags.Photometer);
            m_spectrometerToggle.isOn = InstrumentsMgr.Instance.AreInstrumentsUnlocked(InstrumentFlags.Spectrometer);
            m_colorToggle.isOn = InstrumentsMgr.Instance.AreInstrumentsUnlocked(InstrumentFlags.Color);

            m_constellationToggle.isOn = NotebookMgr.Instance.AreTabsUnlocked(NotebookFlags.Constellations);
            m_planetsToggle.isOn = NotebookMgr.Instance.AreTabsUnlocked(NotebookFlags.Planets);
            m_mainStarsToggle.isOn = NotebookMgr.Instance.AreTabsUnlocked(NotebookFlags.MainSequenceStars);
            m_otherStarsToggle.isOn = NotebookMgr.Instance.AreTabsUnlocked(NotebookFlags.OtherStars);
            m_nebulaeToggle.isOn = NotebookMgr.Instance.AreTabsUnlocked(NotebookFlags.Nebulae);
            m_galaxiesToggle.isOn = NotebookMgr.Instance.AreTabsUnlocked(NotebookFlags.Galaxies);

            m_equatorialToggle.onValueChanged.AddListener(HandleEquatorialToggleChanged);
            m_photometerToggle.onValueChanged.AddListener(HandlePhotometerToggleChanged);
            m_spectrometerToggle.onValueChanged.AddListener(HandleSpectrometerToggleChanged);
            m_colorToggle.onValueChanged.AddListener(HandleColorToggleChanged);

            m_constellationToggle.onValueChanged.AddListener(HandleConstellationsToggleChanged);
            m_planetsToggle.onValueChanged.AddListener(HandlePlanetsToggleChanged);
            m_mainStarsToggle.onValueChanged.AddListener(HandleMainStarsToggleChanged);
            m_otherStarsToggle.onValueChanged.AddListener(HandleOtherStarsToggleChanged);
            m_nebulaeToggle.onValueChanged.AddListener(HandleNebulaeToggleChanged);
            m_galaxiesToggle.onValueChanged.AddListener(HandleGalaxiesToggleChanged);
        }

        private void HandleOpenClicked()
        {
            m_mainPanel.SetActive(true);
        }

        private void HandleCloseClicked()
        {
            m_mainPanel.SetActive(false);
        }

        #region Instruments

        private void HandleEquatorialToggleChanged(bool newVal)
        {
            if (newVal) { InstrumentsMgr.Instance.UnlockedInstruments |= InstrumentFlags.EquatorialCoords; }
            else { InstrumentsMgr.Instance.UnlockedInstruments &= ~InstrumentFlags.EquatorialCoords; }
        
            GameMgr.Events.Dispatch(GameEvents.InstrumentUnlocksChanged);
        }

        private void HandlePhotometerToggleChanged(bool newVal)
        {
            if (newVal) { InstrumentsMgr.Instance.UnlockedInstruments |= InstrumentFlags.Photometer; }
            else { InstrumentsMgr.Instance.UnlockedInstruments &= ~InstrumentFlags.Photometer; }
         
            GameMgr.Events.Dispatch(GameEvents.InstrumentUnlocksChanged);
        }

        private void HandleSpectrometerToggleChanged(bool newVal)
        {
            if (newVal) { InstrumentsMgr.Instance.UnlockedInstruments |= InstrumentFlags.Spectrometer; }
            else { InstrumentsMgr.Instance.UnlockedInstruments &= ~InstrumentFlags.Spectrometer; }
        
            GameMgr.Events.Dispatch(GameEvents.InstrumentUnlocksChanged);
        }

        private void HandleColorToggleChanged(bool newVal)
        {
            if (newVal) { InstrumentsMgr.Instance.UnlockedInstruments |= InstrumentFlags.Color; }
            else { InstrumentsMgr.Instance.UnlockedInstruments &= ~InstrumentFlags.Color; }
        
            GameMgr.Events.Dispatch(GameEvents.InstrumentUnlocksChanged);
        }

        #endregion // Instruments

        #region Notebook

        private void HandleConstellationsToggleChanged(bool newVal)
        {
            if (newVal) { NotebookMgr.Instance.UnlockedTabs |= NotebookFlags.Constellations; }
            else { NotebookMgr.Instance.UnlockedTabs &= ~NotebookFlags.Constellations; }

            GameMgr.Events.Dispatch(GameEvents.NotebookUnlocksChanged);
        }

        private void HandlePlanetsToggleChanged(bool newVal)
        {
            if (newVal) { NotebookMgr.Instance.UnlockedTabs |= NotebookFlags.Planets; }
            else { NotebookMgr.Instance.UnlockedTabs &= ~NotebookFlags.Planets; }

            GameMgr.Events.Dispatch(GameEvents.NotebookUnlocksChanged);
        }

        private void HandleMainStarsToggleChanged(bool newVal)
        {
            if (newVal) { NotebookMgr.Instance.UnlockedTabs |= NotebookFlags.MainSequenceStars; }
            else { NotebookMgr.Instance.UnlockedTabs &= ~NotebookFlags.MainSequenceStars; }

            GameMgr.Events.Dispatch(GameEvents.NotebookUnlocksChanged);
        }

        private void HandleOtherStarsToggleChanged(bool newVal)
        {
            if (newVal) { NotebookMgr.Instance.UnlockedTabs |= NotebookFlags.OtherStars; }
            else { NotebookMgr.Instance.UnlockedTabs &= ~NotebookFlags.OtherStars; }

            GameMgr.Events.Dispatch(GameEvents.NotebookUnlocksChanged);
        }

        private void HandleNebulaeToggleChanged(bool newVal)
        {
            if (newVal) { NotebookMgr.Instance.UnlockedTabs |= NotebookFlags.Nebulae; }
            else { NotebookMgr.Instance.UnlockedTabs &= ~NotebookFlags.Nebulae; }

            GameMgr.Events.Dispatch(GameEvents.NotebookUnlocksChanged);
        }

        private void HandleGalaxiesToggleChanged(bool newVal)
        {
            if (newVal) { NotebookMgr.Instance.UnlockedTabs |= NotebookFlags.Galaxies; }
            else { NotebookMgr.Instance.UnlockedTabs &= ~NotebookFlags.Galaxies; }

            GameMgr.Events.Dispatch(GameEvents.NotebookUnlocksChanged);
        }

        #endregion // Notebook
    }
}