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

        [Header("Coordinate Group")]
        [SerializeField] private GameObject m_coordinatesGroup;
        [SerializeField] private TMP_Text m_coordRAReadoutText;
        [SerializeField] private TMP_Text m_coordDeclReadoutText;
        [SerializeField] private TMP_Text m_coordConstellationReadoutText;


        [Header("Photometer Group")]
        [SerializeField] private GameObject m_photometerGroup;

        [Header("Spectrometer Group")]
        [SerializeField] private GameObject m_spectrometerGroup;

        [Header("Color Group")]
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

            if (!FocusMgr.Instance.LastSelectedFocusable) {
                m_coordinatesGroup.SetActive(false);
                m_photometerGroup.SetActive(false);
                m_spectrometerGroup.SetActive(false);
                m_colorGroup.SetActive(false);
                return;
            } 

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
                    m_objName.SetText(FocusMgr.Instance.LastSelectedFocusable.CelestialObj.Data.Name);
                }
                else
                {
                    m_objName.SetText(UNKNOWN_OBJ_TEXT);
                }
            }
            else
            {
                m_objName.text = string.Empty;
            }
        }

        private void DisplayEquatorialCoords()
        {
            var currData = FocusMgr.Instance.LastSelectedFocusable.CelestialObj.Data;

            Vector3 finalRA;
            Vector3 finalDecl;
            if (currData.UseRadianRADecl)
            {
                var raDegrees = CoordinateUtility.RadianToDegree(currData.RARad);
                finalRA = CoordinateUtility.DegreesToRA(raDegrees);
                var declDegrees = CoordinateUtility.RadianToDegree(currData.DeclRad);
                finalDecl = CoordinateUtility.DecimalDegreesToDegrees(declDegrees);
            }
            else
            {
                finalRA = currData.RA;
                finalDecl = currData.Decl;
            }

            m_coordRAReadoutText.SetText(
                finalRA.x.ToString("F2") + "h "
                + finalRA.y.ToString("F2") + "m "
                + finalRA.z.ToString("F2") + "s");

            m_coordDeclReadoutText.SetText(
                "+" + finalDecl.x.ToString("F2") + "\u00B0 "
                + finalDecl.y.ToString("F2") + "' "
                + finalDecl.z.ToString("F2") + "''");

            m_coordConstellationReadoutText.SetText("[Not Implemented]");

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

