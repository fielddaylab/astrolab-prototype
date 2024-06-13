using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.EventSystems.EventTrigger;

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

        [SerializeField] private GameObject m_entryPage;
        [SerializeField] private TMP_Text m_entryTitleText;
        [SerializeField] private GameObject m_gridPage;

        [SerializeField] private NotebookEntryData[] m_allEntries;

        [SerializeField] private NotebookTab[] m_allTabs;

        [HideInInspector] public NotebookEntryData CurrEntry;

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
            GameMgr.Events.Register<NotebookFlags>(GameEvents.NotebookTabClicked, HandleNotebookTabClicked);

            foreach (var entry in m_allEntries)
            {
                if ((entry.Category & NotebookFlags.Constellations) != 0) { m_constellationEntries.Add(entry); }
                if ((entry.Category & NotebookFlags.Planets) != 0) { m_planetEntries.Add(entry); }
                if ((entry.Category & NotebookFlags.MainSequenceStars) != 0) { m_mainStarEntries.Add(entry); }
                if ((entry.Category & NotebookFlags.OtherStars) != 0) { m_otherStarEntries.Add(entry); }
                if ((entry.Category & NotebookFlags.Nebulae) != 0) { m_nebulaEntries.Add(entry); }
                if ((entry.Category & NotebookFlags.Galaxies) != 0) { m_galaxyEntries.Add(entry); }
            }

            m_entryTitleText.text = string.Empty;
            CurrEntry = null;
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
            if (!FocusMgr.Instance.LastSelectedFocusable || CurrEntry == null) { return; }

            if (CurrEntry.Title.Equals(FocusMgr.Instance.LastSelectedFocusable.CelestialObj.Data.IdentifyEntryID))
            {
                FocusMgr.Instance.LastSelectedFocusable.CelestialObj.Identified = true;

                GameMgr.Events.Dispatch(GameEvents.CelestialObjIdentified);
            }
        }

        private void HandleNotebookUnlocksChanged()
        {
            if (m_rootGroup.alpha == 1) { Open(); }
        }

        private void HandleNotebookTabClicked(NotebookFlags category)
        {
            if ((category & NotebookFlags.Constellations) != 0) { PopulateEntryPage(m_constellationEntries[0]); }
            if ((category & NotebookFlags.Planets) != 0) { PopulateEntryPage(m_planetEntries[0]); }
            if ((category & NotebookFlags.MainSequenceStars) != 0) { PopulateEntryPage(m_mainStarEntries[0]); }
            if ((category & NotebookFlags.OtherStars) != 0) { PopulateEntryPage(m_otherStarEntries[0]); }
            if ((category & NotebookFlags.Nebulae) != 0) { PopulateEntryPage(m_nebulaEntries[0]); }
            if ((category & NotebookFlags.Galaxies) != 0) { PopulateEntryPage(m_galaxyEntries[0]); }

            m_entryPage.SetActive(true);
            m_gridPage.SetActive(false);
        }

        #endregion // Handlers

        #region Helpers

        private void PopulateEntryPage(NotebookEntryData entryData)
        {
            CurrEntry = entryData;

            m_entryTitleText.SetText(entryData.Title);
        }

        #endregion // Helpers
    }
}
