using System;
using UnityEngine;

namespace AstroLab {
    [Serializable]
    public struct NotebookObject {
        public string Name;
        public ObjectInfo[] Parameters;
    }

    [Serializable]
    public struct ObjectInfo {
        public NotebookInfoType Type;
        public string Text;
        public Sprite Image;
        public Color Color;
    }

    public enum NotebookInfoType {
        None,
        Image,
        TempRange,
        Elements
    }

    [CreateAssetMenu(menuName = "AstroLab/Create CategoryNotebook Entry Data")]
    public class CategoryNotebookEntryData : ScriptableObject {
        [SerializeField] private string m_pageTitle;
        [SerializeField] private NotebookFlags m_category;
        [SerializeField] private NotebookObject[] m_items;

        public string Title { get { return m_pageTitle; } }
        public NotebookFlags Category { get { return m_category; } } 
        public NotebookObject[] Items { get { return m_items; } }
    }
}