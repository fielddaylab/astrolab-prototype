using BeauUtil;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace AstroLab
{
    public class UIDocumentsModule : UIInterfaceModule
    {
        public static UIDocumentsModule Instance;


        public float ModuleScale;
        public float MainScreenScale;

        public Postcard[] PostcardsToSpawn;

        [SerializeField] private Button m_CloseButton;
        [SerializeField] private RectTransform m_SwapZone;

        [SerializeField] private Graphic m_IconBackground;
        [SerializeField] private TMP_Text m_IconNum;

        [NonSerialized] public int NewPostcards;


        private void Awake() {
            if (Instance == null) {
                Instance = this;
            } else if (this != Instance) {
                Destroy(this.gameObject);
                return;
            }

            SpawnPostcard(0);
            SpawnPostcard(1);
        }

        public override void Init()
        {
            base.Init();
            m_CloseButton.onClick.AddListener(HandleCloseClicked);
            m_IconNum.SetText(NewPostcards.ToStringLookup());
        }

        public override void Open() {
            base.Open();

        }

        public override void Close()
        {
            base.Close();
        }

        public void SpawnPostcard(int index) {
            Postcard newCard = Instantiate(PostcardsToSpawn[index], this.transform);
            newCard.Initialize();
            newCard.transform.localPosition = new Vector3(UnityEngine.Random.Range(-300, 300), UnityEngine.Random.Range(-100, 100), 0);
            ChangeNewPostcardNum(+1);
        }

        public void ChangeNewPostcardNum(int delta) {
            NewPostcards = Math.Max(NewPostcards + delta, 0);
            m_IconNum.SetText(NewPostcards.ToStringLookup());
            m_IconBackground.gameObject.SetActive(NewPostcards > 0);
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

        #endregion // Handlers

    }
}

