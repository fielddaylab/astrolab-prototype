using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AstroLab
{
    public class FocusMgr : MonoBehaviour
    {
        public static FocusMgr Instance;

        public UIFocusable LastSelectedFocusable;

        [SerializeField] private Button m_unfocusButton;

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

            m_unfocusButton.onClick.AddListener(HandleUnfocusClicked);
        }

        private void HandleUnfocusClicked()
        {
            LastSelectedFocusable = null;

            // set focusable to null by default
            GameMgr.Events.Dispatch(GameEvents.Unfocus);
        }
    }
}