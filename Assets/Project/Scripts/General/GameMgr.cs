using System.Collections;
using System.Collections.Generic;
using BeauUtil.Extensions;
using UnityEngine;

namespace AstroLab
{
    public class GameMgr : MonoBehaviour
    {
        public static GameMgr Instance;
        
        public Camera SkyboxCamera;
        public Camera InterfaceCamera;

        public GameConsts Consts;

        private readonly EventDispatcher<object> m_EventDispatcher = new EventDispatcher<object>();

        /// <summary>
        /// Global game event dispatcher.
        /// </summary>
        static public EventDispatcher<object> Events
        {
            get { return GameMgr.Instance.m_EventDispatcher; }
        }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else if (this != Instance)
            {
                Destroy(this.gameObject);
                return;
            }
        }
    }
}
