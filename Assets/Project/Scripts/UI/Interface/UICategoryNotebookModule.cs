using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.EventSystems.EventTrigger;

namespace AstroLab
{
    public class UICategoryNotebookModule : UIInterfaceModule
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
        [SerializeField] private TMP_Text m_entryColorText;

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

        private int m_currConstellationEntryIndex;
        private int m_currPlanetEntryIndex;
        private int m_currMainStarEntryIndex;
        private int m_currOtherStarEntryIndex;
        private int m_currNebulaEntryIndex;
        private int m_currGalaxyEntryIndex;

        private NotebookFlags m_currCategory;

        public override void Init()
        {
            base.Init();
         
            m_closeButton.onClick.AddListener(HandleCloseClicked);
            m_identifyButton.onClick.AddListener(HandleIdentifyClicked);

            m_gridButton.onClick.AddListener(HandleGridClicked);
            m_prevButton.onClick.AddListener(HandlePrevClicked);
            m_nextButton.onClick.AddListener(HandleNextClicked);

            GameMgr.Events.Register(GameEvents.NotebookUnlocksChanged, HandleNotebookUnlocksChanged);
            GameMgr.Events.Register<NotebookFlags>(GameEvents.NotebookTabClicked, HandleNotebookTabClicked);
            GameMgr.Events.Register(GameEvents.Unfocus, HandleUnfocus);


            foreach (var entry in m_allEntries)
            {
                if ((entry.Category & NotebookFlags.Constellations) != 0) { m_constellationEntries.Add(entry); }
                if ((entry.Category & NotebookFlags.Planets) != 0) { m_planetEntries.Add(entry); }
                if ((entry.Category & NotebookFlags.MainSequenceStars) != 0) { m_mainStarEntries.Add(entry); }
                if ((entry.Category & NotebookFlags.OtherStars) != 0) { m_otherStarEntries.Add(entry); }
                if ((entry.Category & NotebookFlags.Nebulae) != 0) { m_nebulaEntries.Add(entry); }
                if ((entry.Category & NotebookFlags.Galaxies) != 0) { m_galaxyEntries.Add(entry); }
            }

            m_currConstellationEntryIndex = 0;
            m_currPlanetEntryIndex = 0;
            m_currMainStarEntryIndex = 0;
            m_currOtherStarEntryIndex = 0;
            m_currNebulaEntryIndex = 0;
            m_currGalaxyEntryIndex = 0;

            m_entryTitleText.text = string.Empty;
            m_entryColorText.text = string.Empty;
            CurrEntry = null;

            m_entryPage.SetActive(false);
            m_gridPage.SetActive(true);
            m_gridButton.gameObject.SetActive(false);
            m_prevButton.gameObject.SetActive(false);
            m_nextButton.gameObject.SetActive(false);
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
            if ((category & NotebookFlags.Constellations) != 0) { PopulateEntryPage(m_constellationEntries[m_currConstellationEntryIndex]); }
            if ((category & NotebookFlags.Planets) != 0) { PopulateEntryPage(m_planetEntries[m_currPlanetEntryIndex]); }
            if ((category & NotebookFlags.MainSequenceStars) != 0) { PopulateEntryPage(m_mainStarEntries[m_currMainStarEntryIndex]); }
            if ((category & NotebookFlags.OtherStars) != 0) { PopulateEntryPage(m_otherStarEntries[m_currOtherStarEntryIndex]); }
            if ((category & NotebookFlags.Nebulae) != 0) { PopulateEntryPage(m_nebulaEntries[m_currNebulaEntryIndex]); }
            if ((category & NotebookFlags.Galaxies) != 0) { PopulateEntryPage(m_galaxyEntries[m_currGalaxyEntryIndex]); }

            m_entryPage.SetActive(true);
            m_gridPage.SetActive(false);

            m_gridButton.gameObject.SetActive(true);
            m_prevButton.gameObject.SetActive(true);
            m_nextButton.gameObject.SetActive(true);

            m_currCategory = category;
        }

        private void HandleUnfocus()
        {
            // this.Close();
        }

        private void HandleGridClicked()
        {
            m_gridPage.SetActive(true);
            m_entryPage.SetActive(false);

            m_gridButton.gameObject.SetActive(false);
            m_prevButton.gameObject.SetActive(false);
            m_nextButton.gameObject.SetActive(false);
        }

        private void HandlePrevClicked()
        {
            SwitchPage(-1);
        }

        private void HandleNextClicked()
        {
            SwitchPage(1);
        }

        private void SwitchPage(int delta)
        {
            if ((m_currCategory & NotebookFlags.Constellations) != 0) {
                if (m_currConstellationEntryIndex + delta > -1 && m_currConstellationEntryIndex + delta < m_constellationEntries.Count)
                {
                    PopulateEntryPage(m_constellationEntries[m_currConstellationEntryIndex + delta]);
                    m_currConstellationEntryIndex += delta;
                }
            }
            if ((m_currCategory & NotebookFlags.Planets) != 0) {
                if (m_currPlanetEntryIndex + delta > -1 && m_currPlanetEntryIndex + delta < m_planetEntries.Count)
                {
                    PopulateEntryPage(m_planetEntries[m_currPlanetEntryIndex + delta]);
                    m_currPlanetEntryIndex += delta;
                }
            }
            if ((m_currCategory & NotebookFlags.MainSequenceStars) != 0) {
                if (m_currMainStarEntryIndex + delta > -1 && m_currMainStarEntryIndex + delta < m_mainStarEntries.Count)
                {
                    PopulateEntryPage(m_mainStarEntries[m_currMainStarEntryIndex + delta]);
                    m_currMainStarEntryIndex += delta;
                }
            }
            if ((m_currCategory & NotebookFlags.OtherStars) != 0) {
                if (m_currOtherStarEntryIndex + delta > -1 && m_currOtherStarEntryIndex + delta < m_otherStarEntries.Count)
                {
                    PopulateEntryPage(m_otherStarEntries[m_currOtherStarEntryIndex + delta]);
                    m_currOtherStarEntryIndex += delta;
                }
            }
            if ((m_currCategory & NotebookFlags.Nebulae) != 0) {
                if (m_currNebulaEntryIndex + delta > -1 && m_currNebulaEntryIndex + delta < m_nebulaEntries.Count)
                {
                    PopulateEntryPage(m_nebulaEntries[m_currNebulaEntryIndex + delta]);
                    m_currNebulaEntryIndex += delta;
                }
            }
            if ((m_currCategory & NotebookFlags.Galaxies) != 0) {
                if (m_currGalaxyEntryIndex + delta > -1 && m_currGalaxyEntryIndex + delta < m_galaxyEntries.Count)
                {
                    PopulateEntryPage(m_galaxyEntries[m_currGalaxyEntryIndex + delta]);
                    m_currGalaxyEntryIndex += delta;
                }
            }
        }

        #endregion // Handlers

        #region Helpers

        private void PopulateEntryPage(NotebookEntryData entryData)
        {
            CurrEntry = entryData;

            m_entryTitleText.SetText(entryData.Title);
            m_entryColorText.SetText(entryData.Color);
        }

        #endregion // Helpers
    }
}
