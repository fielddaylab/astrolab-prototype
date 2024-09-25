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
        public Image Represent2D;
        public Image Outline;
        public Button Button;

        public void Init(UIFocusable target, Sprite represent2D)
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

            Represent2D.sprite = represent2D;
            if (!represent2D) { Represent2D.enabled = false; }

            Outline.enabled = false;

            GameMgr.Events.Register(GameEvents.UnfocusDown, HandleUnfocus);
            GameMgr.Events.Register<UIFocusable>(GameEvents.FocusableClicked, HandleFocusableClicked);
        }

        private void LateUpdate()
        {
            // calculate this object's screen position
            var point = GameMgr.Instance.SkyboxCamera.WorldToScreenPoint(Target.transform.position);

            this.Rect.anchoredPosition = point;
        }

        private void HandleTargetBecameVisible(object sender, EventArgs args)
        {
            Debug.Log("[UIFocus] target " + Target.gameObject.name + " became visible");

            this.gameObject.SetActive(true);
        }

        private void HandleTargetBecameInvisible(object sender, EventArgs args)
        {
            Debug.Log("[UIFocus] target " + Target.gameObject.name + " became invisible");

            this.gameObject.SetActive(false);
        }

        private void HandleFocusClicked()
        {
            Debug.Log("Clicked on " + Target.CelestialObj.Data.Name + "!");
            GameMgr.Events.Dispatch(GameEvents.FocusableClicked, Target);

            Outline.enabled = true;
        }

        private void HandleUnfocus()
        {
            Outline.enabled = false;
        }

        private void HandleFocusableClicked(UIFocusable focusable)
        {
            if (!focusable.ID.Equals(this.Target.ID))
            {
                Outline.enabled = false;
            }
        }
    }
}