

using AstroLab;
using BeauRoutine;
using UnityEngine;
using UnityEngine.EventSystems;

public class DataSource : AbstractDraggable {

    [SerializeField] private DataType DataType;


    [SerializeField] private CanvasGroup DraggingVisual;

    private void Start() {
        InstantiateDraggingVisual();
    }

    private void InstantiateDraggingVisual() {
        if (DraggingVisual == null) {
            DraggingVisual = Instantiate(BodyGroup);
        } 
        DraggingVisual.transform.SetParent(transform);
        DraggingVisual.alpha = 0.5f;
        DraggingVisual.gameObject.SetActive(false);
    }

    protected override void SetGrabbed(bool grabbed, bool dragging) {
        if (m_Grabbed == grabbed) return;
        m_Grabbed = grabbed;

        if (grabbed) {
            transform.SetAsLastSibling();
            GameMgr.Events.Dispatch(GameEvents.DraggableGrabbed, this.DraggableType);
        } else {
            GameMgr.Events.Dispatch(GameEvents.DraggableDropped, this.DraggableType);
        }

        DraggingVisual.gameObject.SetActive(grabbed);
        DraggingVisual.transform.localScale *= grabbed ? GrabScaleFactor : 1f / GrabScaleFactor;

        DraggingVisual.blocksRaycasts = dragging && !grabbed;
    }

    protected override void MoveWithMouse(PointerEventData eventData = null) {
        if (eventData != null) {
            DraggingVisual.transform.position = eventData.pointerCurrentRaycast.worldPosition;
        }
    }
}