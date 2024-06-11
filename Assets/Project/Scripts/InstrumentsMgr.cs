using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AstroLab
{
    [Flags]
    public enum InstrumentFlags
    {
        EquatorialCoords = 0x01,
        Photometer = 0x02,
        Spectrometer = 0x04,
        Color = 0x08,
    }

    public class InstrumentsMgr : MonoBehaviour
    {
        public static InstrumentsMgr Instance;

        [SerializeField] private InstrumentFlags m_initialInstruments;

        public InstrumentFlags UnlockedInstruments;

        private void Awake()
        {
            if (Instance == null) { Instance = this; }
            else if (Instance != this) { Destroy(this.gameObject); return; }

            UnlockedInstruments = m_initialInstruments;
        }

        public bool AreInstrumentsUnlocked(InstrumentFlags instruments)
        {
            return (UnlockedInstruments & instruments) != 0;
        }
    }
}