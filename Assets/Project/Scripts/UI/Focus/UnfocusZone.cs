using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace AstroLab {
    public class UnfocusZone : Button, IPointerDownHandler, IPointerUpHandler
    {
        public UnityEvent onPointerDown = new UnityEvent();
        public UnityEvent onPointerUp = new UnityEvent();

        #region Pointer Events

        public override void OnPointerDown(PointerEventData data)
        {
            base.OnPointerDown(data);

            onPointerDown.Invoke();
        }

        public override void OnPointerUp(PointerEventData data)
        {
            base.OnPointerDown(data);

            onPointerUp.Invoke();
        }

        #endregion // Pointer Events
    }
}