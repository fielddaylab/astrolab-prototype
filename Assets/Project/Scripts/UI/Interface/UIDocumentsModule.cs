using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace AstroLab
{
    public class UIDocumentsModule : UIInterfaceModule
    {

        public HashSet<Draggable> Postcards;

        public float ModuleScale;
        public float MainScreenScale;

        [SerializeField] private Button m_CloseButton;
        [SerializeField] private RectTransform m_SwapZone;
 
        public override void Init()
        {
            base.Init();
            m_CloseButton.onClick.AddListener(HandleCloseClicked);
        }

        public override void Open() {
            base.Open();

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

        #endregion // Handlers

    }
}

