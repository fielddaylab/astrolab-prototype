
using AstroLab;
using UnityEngine;
using UnityEngine.EventSystems;

public enum DraggableType {
    None,
    Postcard,
    Data
}

public abstract class AbstractDraggable : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerClickHandler {

    [SerializeField] protected DraggableType DraggableType;
    [SerializeField] protected CanvasGroup BodyGroup;
    [SerializeField] protected float GrabScaleFactor;

    protected bool m_Grabbed = false;



    protected virtual void SetGrabbed(bool grabbed, bool dragging) {
        if (m_Grabbed == grabbed) return;
        m_Grabbed = grabbed;

        transform.localScale *= grabbed ? GrabScaleFactor : 1f/GrabScaleFactor;

        if (grabbed) {
            transform.SetAsLastSibling();
            GameMgr.Events.Dispatch(GameEvents.DraggableGrabbed, this.DraggableType);
        } else {
            GameMgr.Events.Dispatch(GameEvents.DraggableDropped, this.DraggableType);
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