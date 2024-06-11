using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AstroLab
{
    public class FocusMgr : MonoBehaviour
    {
        public static FocusMgr Instance;

        public UIFocusable LastSelectedFocusable;

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