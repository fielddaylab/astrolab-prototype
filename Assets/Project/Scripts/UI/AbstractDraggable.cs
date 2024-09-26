
using AstroLab;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

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

    public abstract class AbstractDraggable : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler {

        private static float LIFT_DISTANCE = 5;

        [SerializeField] public DraggableFlags DraggableFlags;
        [SerializeField] protected CanvasGroup BodyGroup;
        [SerializeField] protected float GrabScaleFactor = 1f;


        [Header("Hover")]
        [SerializeField] protected bool m_ChangesCursor = true;
        [SerializeField] protected bool m_LiftOnHover = true;

        protected bool m_Grabbed = false;
        protected bool m_OverMain = false;
        protected float m_LiftOrigin;

        private Vector3 MouseOffset;

        #region Unity Callbacks

        private void Awake()
        {
            m_LiftOrigin = BodyGroup.transform.localPosition.y;
        }

        #endregion // UnityCallbacks

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
                transform.position = eventData.pointerCurrentRaycast.worldPosition + MouseOffset;
            } else {
                // TODO: make a raycast, move draggable to mouse pointer in canvas coords
            }
        }

        protected virtual void SetDefaultCursor() {
            GameConsts consts = FindObjectOfType<GameConsts>();

            Cursor.SetCursor(consts.DefaultCursor, Vector2.zero, CursorMode.Auto);
        }

        protected virtual void SetGrabCursor() {
            GameConsts consts = FindObjectOfType<GameConsts>();

            Cursor.SetCursor(consts.GrabCursor, new Vector2(11, 2), CursorMode.Auto);
        }

        protected virtual void SetLiftPos(bool lifted) {
            if (lifted) {
                Vector2 currPos = BodyGroup.transform.localPosition;
                currPos.y = m_LiftOrigin + LIFT_DISTANCE;
                BodyGroup.transform.localPosition = currPos;
            }
            else
            {
                Vector2 currPos = BodyGroup.transform.localPosition;
                currPos.y = m_LiftOrigin;
                BodyGroup.transform.localPosition = currPos;
            }
        }


        #region Pointer Events
        public virtual void OnBeginDrag(PointerEventData eventData) {
            MouseOffset = transform.position - eventData.pointerCurrentRaycast.worldPosition;
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

        public virtual void OnPointerEnter(PointerEventData eventData)
        {
            m_OverMain = true;

            if (m_ChangesCursor) {
                SetGrabCursor();
            }
            if (m_LiftOnHover) {
                SetLiftPos(true);
            }
        }

        public virtual void OnPointerExit(PointerEventData eventData)
        {
            m_OverMain = false;

            if (!m_Grabbed) {
                if (m_ChangesCursor) {
                    SetDefaultCursor();
                }
                if (m_LiftOnHover) {
                    SetLiftPos(false);
                }
            }
        }

        #endregion //Pointer Events
    }
}