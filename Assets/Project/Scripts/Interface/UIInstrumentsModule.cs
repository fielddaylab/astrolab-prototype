using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AstroLab
{
    public class UIInstrumentsModule : UIInterfaceModule
    {
        [Space(5)]
        [Header("Instruments")]
        [SerializeField] private Button m_closeButton;

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

