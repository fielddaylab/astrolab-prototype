using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AstroLab
{
    public class UIFocus : MonoBehaviour
    {
        public RectTransform Rect;
        public Button Button;

        private void Start()
        {
            if (Button)
            {
                Button.onClick.AddListener(HandleFocusClicked);
            }
        }

        private void HandleFocusClicked()
        {
            Debug.Log("Clicked!");
        }
    }
}