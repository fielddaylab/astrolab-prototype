using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AstroLab
{
    public class UINotebookModule : UIInterfaceModule
    {
        [Space(5)]
        [Header("Notebook")]
        [SerializeField] private Button m_closeButton;

        [SerializeField] private Button m_prevButton;
        [SerializeField] private Button m_nextButton;
        [SerializeField] private Button m_gridButton;
        [SerializeField] private Button m_identifyButton;

        [SerializeField] private NotebookEntryData[] m_allEntries;

        [SerializeField] private NotebookTab[] m_allTabs;

        private List<NotebookEntryData> m_constellationEntries = new List<NotebookEntryData>();
        private List<NotebookEntryData> m_planetEntries = new List<NotebookEntryData>();
        private List<NotebookEntryData> m_mainStarEntries = new List<NotebookEntryData>();
        private List<NotebookEntryData> m_otherStarEntries = new List<NotebookEntryData>();
        private List<NotebookEntryData> m_nebulaEntries = new List<NotebookEntryData>();
        private List<NotebookEntryData> m_galaxyEntries = new List<NotebookEntryData>();

        public override void Init()
        {
            base.Init();
         
            m_closeButton.onClick.AddListener(HandleCloseClicked);
            m_identifyButton.onClick.AddListener(HandleIdentifyClicked);

            GameMgr.Events.Register(GameEvents.NotebookUnlocksChanged, HandleNotebookUnlocksChanged);
        
            foreach (var entry in m_allEntries)
            {
                if ((entry.Category & NotebookFlags.Constellations) != 0) { m_constellationEntries.Add(entry); }
                if ((entry.Category & NotebookFlags.Planets) != 0) { m_planetEntries.Add(entry); }
                if ((entry.Category & NotebookFlags.MainSequenceStars) != 0) { m_mainStarEntries.Add(entry); }
                if ((entry.Category & NotebookFlags.OtherStars) != 0) { m_otherStarEntries.Add(entry); }
                if ((entry.Category & NotebookFlags.Nebulae) != 0) { m_nebulaEntries.Add(entry); }
                if ((entry.Category & NotebookFlags.Galaxies) != 0) { m_galaxyEntries.Add(entry); }
            }
        }

        public override void Open()
        {
            base.Open();

            foreach(var tab in m_allTabs)
            {
                tab.gameObject.SetActive(NotebookMgr.Instance.AreTabsUnlocked(tab.Category));
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

        private void HandleIdentifyClicked()
        {
            if (!FocusMgr.Instance.LastSelectedFocusable) { return; }

            // var currEntry = NotebookMgr.Instance.CurrEntry;
            FocusMgr.Instance.LastSelectedFocusable.CelestialObj.Identified = true;

            GameMgr.Events.Dispatch(GameEvents.CelestialObjIdentified);
        }

        private void HandleNotebookUnlocksChanged()
        {
            if (m_rootGroup.alpha == 1) { Open(); }
        }

        #endregion // Handlers
    }
}
