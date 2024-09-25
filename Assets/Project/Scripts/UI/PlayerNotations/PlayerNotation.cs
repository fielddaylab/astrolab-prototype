using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AstroLab
{
    public class PlayerNotation : MonoBehaviour
    {
        [SerializeField] private Button m_MainButton;
        [SerializeField] private Button m_DeleteButton;

        [SerializeField] private CanvasGroup m_NoteGroup;

        private bool m_IsOpened;

        public void Init()
        {
            m_IsOpened = true;

            m_MainButton.onClick.AddListener(OnMainClicked);
            m_DeleteButton.onClick.AddListener(OnDeleteClicked);
        }

        #region Handlers

        private void OnMainClicked()
        {
            if (m_IsOpened)
            {
                m_NoteGroup.alpha = 0;
                m_NoteGroup.interactable = false;
                m_NoteGroup.blocksRaycasts = false;
            }
            else
            {
                m_NoteGroup.alpha = 1;
                m_NoteGroup.interactable = true;
                m_NoteGroup.blocksRaycasts = true;
            }

            m_IsOpened = !m_IsOpened;
        }

        private void OnDeleteClicked()
        {
            Destroy(this.gameObject);
        }

        #endregion // Handlers
    }
}