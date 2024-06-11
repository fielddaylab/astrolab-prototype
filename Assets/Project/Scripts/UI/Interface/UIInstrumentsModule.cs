using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace AstroLab
{
    public class UIInstrumentsModule : UIInterfaceModule
    {
        [Space(5)]
        [Header("Instruments")]
        [SerializeField] private Button m_closeButton;

        [SerializeField] private TMP_Text m_objName;

        [Header("Groups")]
        [SerializeField] private GameObject m_coordinatesGroup;
        [SerializeField] private GameObject m_photometerGroup;
        [SerializeField] private GameObject m_spectrometerGroup;
        [SerializeField] private GameObject m_colorGroup;

        private static string UNKNOWN_OBJ_TEXT = "Unknown Object";

        public override void Init()
        {
            base.Init();
            m_closeButton.onClick.AddListener(HandleCloseClicked);
        }

        public override void Open()
        {
            base.Open();

            DisplayName();

            if (InstrumentsMgr.Instance.AreInstrumentsUnlocked(InstrumentFlags.EquatorialCoords))
            {
                DisplayEquatorialCoords();
            }
            else
            {
                m_coordinatesGroup.SetActive(false);
            }

            if (InstrumentsMgr.Instance.AreInstrumentsUnlocked(InstrumentFlags.Photometer))
            {
                DisplayPhotometer();
            }
            else
            {
                m_photometerGroup.SetActive(false);
            }

            if (InstrumentsMgr.Instance.AreInstrumentsUnlocked(InstrumentFlags.Spectrometer))
            {
                DisplaySpectrometer();
            }
            else
            {
                m_spectrometerGroup.SetActive(false);
            }

            if (InstrumentsMgr.Instance.AreInstrumentsUnlocked(InstrumentFlags.Color))
            {
                DisplayColor();
            }
            else
            {
                m_colorGroup.SetActive(false);
            }
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

        #endregion // Handlers

        private void DisplayName()
        {
            if (FocusMgr.Instance.LastSelectedFocusable)
            {
                if (FocusMgr.Instance.LastSelectedFocusable.CelestialObj.Identified)
                {
                    m_objName.text = FocusMgr.Instance.LastSelectedFocusable.CelestialObj.Data.Name;
                }
                else
                {
                    m_objName.text = UNKNOWN_OBJ_TEXT;
                }
            }
            else
            {
                m_objName.text = string.Empty;
            }
        }

        private void DisplayEquatorialCoords()
        {
            m_coordinatesGroup.SetActive(true);
        }

        private void DisplayPhotometer()
        {
            m_photometerGroup.SetActive(true);
        }

        private void DisplaySpectrometer()
        {
            m_spectrometerGroup.SetActive(true);
        }

        private void DisplayColor()
        {
            m_colorGroup.SetActive(true);
        }
    }
}

