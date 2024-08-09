
using AstroLab;
using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace AstroLab {
    [Flags]
    public enum DraggableFlags {
        Postcard = 0x1,
        Name = 0x2,
        Coords = 0x4,
        Color = 0x8,
        Magnitude = 0x10,
        Spectrum = 0x20,
    }

    public abstract class AbstractDraggable : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerClickHandler {

        [SerializeField] public DraggableFlags DraggableFlags;
        [SerializeField] protected CanvasGroup BodyGroup;
        [SerializeField] protected float GrabScaleFactor = 1f;

        protected bool m_Grabbed = false;



        protected virtual void SetGrabbed(bool grabbed, bool dragging) {
            if (m_Grabbed == grabbed) return;
            m_Grabbed = grabbed;

            transform.localScale *= grabbed ? GrabScaleFactor : 1f / GrabScaleFactor;

            if (grabbed) {
                transform.SetAsLastSibling();
                GameMgr.Events.Dispatch(GameEvents.DraggableGrabbed, DraggableFlags);
            } else {
                GameMgr.Events.Dispatch(GameEvents.DraggableDropped, DraggableFlags);
            }

            BodyGroup.blocksRaycasts = dragging && !grabbed;
        }

        protected virtual void MoveWithMouse(PointerEventData eventData = null) {
            if (eventData != null) {
                transform.position = eventData.pointerCurrentRaycast.worldPosition;
            } else {
                // TODO: make a raycast, move draggable to mouse pointer in canvas coords
            }
        }


        #region Pointer Events
        public virtual void OnBeginDrag(PointerEventData eventData) {
            SetGrabbed(true, true);
        }

        public virtual void OnDrag(PointerEventData eventData) {
            MoveWithMouse(eventData);
        }

        public virtual void OnEndDrag(PointerEventData eventData) {
            SetGrabbed(false, true);
        }

        public virtual void OnPointerClick(PointerEventData eventData) {
            // TODO: SetGrabbed(!m_Grabbed, false)
        }
        #endregion //Pointer Events
    }
}