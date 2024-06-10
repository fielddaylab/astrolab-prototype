using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AstroLab
{
    public enum LayoutBehavior
    {
        Default,            // no effect on other modules
        ForceCloseOther     // force close other modules
    }

    public class UIInterfaceModule : MonoBehaviour, IUIInterfaceModule
    {
        [Header("Base")]
        public UIID ID;
        public LayoutBehavior LayoutBehavior;

        [SerializeField] protected CanvasGroup m_rootGroup;

        private bool m_blocksRaycasts;

        #region IUIInterfaceModule

        public virtual void Init()
        {
            m_blocksRaycasts = m_rootGroup.blocksRaycasts;
        }

        public virtual void Open()
        {
            m_rootGroup.alpha = 1;
            m_rootGroup.blocksRaycasts = m_blocksRaycasts;
        }

        public virtual void Close()
        {
            m_rootGroup.alpha = 0;
            m_rootGroup.blocksRaycasts = false;
        }

        #endregion // IUIInterfaceModule
    }
}