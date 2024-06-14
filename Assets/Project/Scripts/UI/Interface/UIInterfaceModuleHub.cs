using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AstroLab
{
    public enum UIID : byte
    {
        // None
        None,

        // PVT Tablet
        Settings,
        Notebook,
        Tech,
        Instruments,
    }


    /// <summary>
    /// Coordinates opening/closing between various UI systems
    /// </summary>
    public class UIInterfaceModuleHub : MonoBehaviour
    {
        [SerializeField] private UIInterfaceModule[] m_registered;

        [SerializeField] private UIInterfaceModule m_initialUI;

        private bool m_initialized = false;

        private void Start()
        {
            InitializeRegistered();

            CloseAll();

            if (m_initialUI)
            {
                OpenUI(m_initialUI.ID);
            }
        }

        public void InitializeRegistered()
        {
            if (m_initialized)
            {
                return;
            }

            for (int i = 0; i < m_registered.Length; i++)
            {
                m_registered[i].Init();
            }

            m_initialized = true;
        }

        public void OpenUI(UIID id)
        {
            UIInterfaceModule toOpen = null;

            for (int i = 0; i < m_registered.Length; i++)
            {
                UIInterfaceModule currModule = m_registered[i];
                if (currModule.ID == id)
                {
                    toOpen = currModule;

                    if (currModule.LayoutBehavior == LayoutBehavior.ForceCloseOther)
                    {
                        for (int j = 0; j < m_registered.Length; j++)
                        {
                            if (m_registered[j].ID != id)
                            {
                                m_registered[j].Close();
                            }
                        }
                    }
                    break;
                }
            }

            toOpen.Open();

            GameMgr.Events.Dispatch(GameEvents.UISwitched);
        }

        private void CloseAll()
        {
            for (int i = 0; i < m_registered.Length; i++)
            {
                m_registered[i].Close();
            }
        }
    }
}

