using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AstroLab
{
    public class NotebookTab : MonoBehaviour
    {
        public NotebookFlags Category;

        [SerializeField] private Button m_tabButton;

        private void Start()
        {
            m_tabButton.onClick.AddListener(HandleTabClicked);
        }

        private void HandleTabClicked()
        {
            GameMgr.Events.Dispatch(GameEvents.NotebookTabClicked, Category);
        }
    }
}