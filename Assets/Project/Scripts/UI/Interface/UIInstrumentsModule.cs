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
        [SerializeField] private DataSource m_nameData;

        [SerializeField] private CameraController m_camController;

        [Header("Coordinate Input Group")]
        [SerializeField] private GameObject m_coordinateInputGroup;
        [SerializeField] private TMP_InputField m_coordRAHrsInput;
        [SerializeField] private TMP_InputField m_coordRAMinsInput;
        [SerializeField] private TMP_InputField m_coordRASecsInput;
        [SerializeField] private TMP_InputField m_coordDeclDegreesInput;
        [SerializeField] private TMP_InputField m_coordDeclMinsInput;
        [SerializeField] private TMP_InputField m_coordDeclSecsInput;
        [SerializeField] private Button m_coordSubmitButton;

        [Header("Coordinate Group")]
        [SerializeField] private GameObject m_coordinatesGroup;
        [SerializeField] private TMP_Text m_coordRAReadoutText;
        [SerializeField] private TMP_Text m_coordDeclReadoutText;
        [SerializeField] private DataSource m_fullCoords;
        [SerializeField] private TMP_Text m_coordConstellationReadoutText;

        [Header("Photometer Group")]
        [SerializeField] private GameObject m_photometerGroup;
        [SerializeField] private DataSource m_photometerData;

        [Header("Spectrometer Group")]
        [SerializeField] private GameObject m_spectrometerGroup;
        [SerializeField] private DataSource m_spectrumData;

        [Header("Color Group")]
        [SerializeField] private GameObject m_colorGroup;
        [SerializeField] private TMP_Text m_colorText;
        [SerializeField] private DataSource m_colorData;

        private static string UNKNOWN_OBJ_TEXT = "Unknown Object";

        public override void Init()
        {
            base.Init();
            m_closeButton.onClick.AddListener(HandleCloseClicked);
            m_coordSubmitButton.onClick.AddListener(HandleCoordSubmitClicked);

            GameMgr.Events.Register(GameEvents.InstrumentUnlocksChanged, HandleInstrumentUnlocksChanged);
            GameMgr.Events.Register(GameEvents.CelestialObjIdentified, HandleCelestialObjIdentified);
            GameMgr.Events.Register(GameEvents.UnfocusDown, HandleUnfocus);
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

                // Display coordinate input UI
                DisplayCoordInputGroup();
                return;
            }

            m_coordinateInputGroup.SetActive(false);

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

        private void HandleUnfocus()
        {
            bool wasOpen = m_rootGroup.alpha == 1;

            this.Close();

            if (wasOpen) { this.Open(); }
        }

        private void HandleCoordSubmitClicked() 
        {
            var ra = new Vector3(
                m_coordRAHrsInput.text.Equals(string.Empty) ? 0 : float.Parse(m_coordRAHrsInput.text),
                m_coordRAMinsInput.text.Equals(string.Empty) ? 0 : float.Parse(m_coordRAMinsInput.text),
                m_coordRASecsInput.text.Equals(string.Empty) ? 0 : float.Parse(m_coordRASecsInput.text)
                );

            var decl = new Vector3(
            m_coordDeclDegreesInput.text.Equals(string.Empty) ? 0 : float.Parse(m_coordDeclDegreesInput.text),
            m_coordDeclMinsInput.text.Equals(string.Empty) ? 0 : float.Parse(m_coordDeclMinsInput.text),
            m_coordDeclSecsInput.text.Equals(string.Empty) ? 0 : float.Parse(m_coordDeclSecsInput.text)
            );
            
            int skyboxDist = FindObjectOfType<GameConsts>().SkyboxDist;

            float raDegrees = (float)CoordinateUtility.RAToDegrees((int)ra.x, (int)ra.y, ra.z);
            float declDegrees = (float)CoordinateUtility.DeclensionToDecimalDegrees((int)decl.x, (int)decl.y, decl.z);
            var pos = CoordinateUtility.RAscDeclDegreesToCartesianCoordinates(raDegrees, declDegrees) * skyboxDist;
            m_camController.TryLook(pos);
        }

        #endregion // Handlers

        private void DisplayName()
        {
            if (FocusMgr.Instance.LastSelectedFocusable)
            {
                if (FocusMgr.Instance.LastSelectedFocusable.CelestialObj.Identified)
                {
                    string name = FocusMgr.Instance.LastSelectedFocusable.CelestialObj.Data.Name;
                    m_objName.SetText(name);
                    m_nameData.SetPayload(new DataPayload(name));
                    m_nameData.gameObject.SetActive(true);
                } else
                {
                    m_objName.SetText(UNKNOWN_OBJ_TEXT);
                    m_nameData.SetPayload(new DataPayload(UNKNOWN_OBJ_TEXT));
                    m_nameData.gameObject.SetActive(true);
                }
            }
            else
            {
                m_objName.text = string.Empty;
                m_nameData.gameObject.SetActive(false);
            }
        }

        private void DisplayCoordInputGroup()
        {
            m_coordinateInputGroup.SetActive(true);
        }

        private void DisplayEquatorialCoords()
        {
            var currData = FocusMgr.Instance.LastSelectedFocusable.CelestialObj.Data;

            EqCoordinates finalCoords;
            Vector3 finalRA;
            Vector3 finalDecl;
            if (currData.UseRadianRADecl)
            {
                var raDegrees = CoordinateUtility.RadianToDegree(currData.RARad);
                finalRA = CoordinateUtility.DegreesToRA(raDegrees);
                var declDegrees = CoordinateUtility.RadianToDegree(currData.DeclRad);
                finalDecl = CoordinateUtility.DecimalDegreesToDegrees(declDegrees);
                finalCoords = CoordinateUtility.RadiansToCoordinates(currData.RARad, currData.DeclRad);
            }
            else
            {
                finalRA = currData.RA;
                finalDecl = currData.Decl;
                finalCoords = new EqCoordinates(currData.RA, currData.Decl);
            }

            m_coordRAReadoutText.SetText(
                finalRA.x + "h "
                + finalRA.y + "m "
                + finalRA.z.ToString("F1") + "s");

            m_coordDeclReadoutText.SetText(
                "+" + finalDecl.x + "\u00B0 "
                + finalDecl.y + "' "
                + finalDecl.z.ToString("F1") + "''");

            m_coordConstellationReadoutText.SetText(currData.Constellation);
            m_fullCoords.SetPayload(new DataPayload(finalCoords));

            m_coordinatesGroup.SetActive(true);
        }

        private void DisplayPhotometer()
        {
            var currData = FocusMgr.Instance.LastSelectedFocusable.CelestialObj.Data;

            m_photometerData.SetPayload(new DataPayload(currData.Magnitude));
            m_photometerGroup.SetActive(true);
        }

        private void DisplaySpectrometer()
        {
            var currData = FocusMgr.Instance.LastSelectedFocusable.CelestialObj.Data;

            m_spectrometerGroup.SetActive(true);
            m_spectrumData.SetPayload(new DataPayload(currData.Spectrum));
        }

        private void DisplayColor()
        {
            var currData = FocusMgr.Instance.LastSelectedFocusable.CelestialObj.Data;

            m_colorGroup.SetActive(true);

            m_colorText.SetText(currData.Color);
            m_colorData.SetPayload(new DataPayload(currData.OverrideMat.color));
        }

        private void HandleInstrumentUnlocksChanged()
        {
            if (m_rootGroup.alpha == 1) { Open(); }
        }

        private void HandleCelestialObjIdentified()
        {
            if (m_rootGroup.alpha == 1) { Open(); }
        }
    }
}

