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

        public override void Init()
        {
            base.Init();
         
            m_closeButton.onClick.AddListener(HandleCloseClicked);
        }

        public override void Open()
        {
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

        #endregion // Handlers
    }
}
