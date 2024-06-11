using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AstroLab
{
    public class UIFocus : MonoBehaviour
    {
        public UIFocusable Target;
        public RectTransform Rect;
        public Button Button;

        public void Init(UIFocusable target)
        {
            if (Button)
            {
                Button.onClick.AddListener(HandleFocusClicked);
            }

            Target = target;

            if (Target.Renderer)
            {
                this.gameObject.SetActive(Target.Renderer.isVisible);
            }
            else
            {
                this.gameObject.SetActive(false);
            }

            Target.BecameVisible += HandleTargetBecameVisible;
            Target.BecameInvisible += HandleTargetBecameInvisible;
        }

        private void LateUpdate()
        {
            // calculate this object's screen position
            var point = GameMgr.Instance.SkyboxCamera.WorldToScreenPoint(Target.transform.position);

            this.Rect.anchoredPosition = point;
        }

        private void HandleTargetBecameVisible(object sender, EventArgs args)
        {
            this.gameObject.SetActive(true);
        }

        private void HandleTargetBecameInvisible(object sender, EventArgs args)
        {
            this.gameObject.SetActive(false);
        }

        private void HandleFocusClicked()
        {
            Debug.Log("Clicked on " + Target.CelestialObj.Data.Name + "!");
            GameMgr.Events.Dispatch(GameEvents.FocusableClicked, Target);
        }
    }
}