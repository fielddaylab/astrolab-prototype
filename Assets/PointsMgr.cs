using BeauUtil;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace AstroLab {
    public class PointsMgr : MonoBehaviour {

        public static PointsMgr Instance;

        [NonSerialized] public int CurrentXP;

        [SerializeField] private TMP_Text m_XPText;

        private void Awake() {
            if (Instance == null) {
                Instance = this;
            } else if (this != Instance ) {
                Destroy(gameObject);
                return;
            }
            AddXP(FindObjectOfType<GameConsts>().StartingXP);
        }

        public void AddXP(int add) {
            CurrentXP = Math.Max(0, CurrentXP + add);
            UpdateText();
        }

        public void UpdateText() {
            m_XPText.SetText(CurrentXP.ToStringLookup());
        }
    }
}
