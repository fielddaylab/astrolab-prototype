using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AstroLab
{
    [CreateAssetMenu(menuName = "AstroLab/Create Notebook Entry Data")]
    public class NotebookEntryData : ScriptableObject
    {
        [SerializeField] private NotebookFlags m_category;
        [SerializeField] private string m_title;

        public NotebookFlags Category { get { return m_category; } }
        public string Title { get { return m_title; } }
    }
}

