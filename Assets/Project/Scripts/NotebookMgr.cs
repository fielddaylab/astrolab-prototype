using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AstroLab
{
    [Flags]
    public enum NotebookFlags
    {
        Constellations = 0x01,
        Planets = 0x02,
        MainSequenceStars = 0x04,
        OtherStars = 0x08,
        Nebulae = 0x10,
        Galaxies = 0x20
    }

    public class NotebookMgr : MonoBehaviour
    {
        public static NotebookMgr Instance;

        [SerializeField] private NotebookFlags m_initialTabs;

        public NotebookFlags UnlockedTabs;

        private void Awake()
        {
            if (Instance == null) { Instance = this; }
            else if (Instance != this) { Destroy(this.gameObject); return; }

            UnlockedTabs = m_initialTabs;
        }

        public bool AreTabsUnlocked(NotebookFlags tabs)
        {
            return (UnlockedTabs & tabs) != 0;
        }
    }
}

