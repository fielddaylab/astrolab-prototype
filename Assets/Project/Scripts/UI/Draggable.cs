using AstroLab;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Draggable : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerClickHandler {

    public float ScaleFactor;
    private bool m_Grabbed;

    [Header("Drop Shadow")]
    [SerializeField] private CanvasGroup m_ItemBody;
    [SerializeField] private Graphic m_Shadow;
    [SerializeField] private float m_ShadowScaleFactor;
    [SerializeField] private float m_ShadowOffset;


    private void Update() {
        if (m_Grabbed) {
            MoveWithMouse();
        }
    }

    private void SetGrabbed(bool grabbed, bool dragging) {
        if (m_Grabbed == grabbed) return;
        m_Grabbed = grabbed;
        transform.localScale *= grabbed ? ScaleFactor : 1f / ScaleFactor;
        if (m_Shadow != null) {
            m_Shadow.transform.localScale *= grabbed ? m_ShadowScaleFactor : 1f / m_ShadowScaleFactor;
            m_Shadow.transform.localPosition += grabbed ? new Vector3(m_ShadowOffset, -m_ShadowOffset, 0f) : new Vector3(-m_ShadowOffset, m_ShadowOffset, 0f);
        }
        if (grabbed) {
            transform.SetAsLastSibling();
            GameMgr.Events.Dispatch(GameEvents.DocumentGrabbed);
        } else {
            GameMgr.Events.Dispatch(GameEvents.DocumentDropped);
        }
        if (dragging) {
            m_ItemBody.blocksRaycasts = !grabbed;
        }

        
    }

    private void MoveWithMouse(PointerEventData eventData = null) {
        if (eventData != null) {
            transform.position = eventData.pointerCurrentRaycast.worldPosition;
        } else {
            // Move postcard to mouse pos in canvas coordinates
        }
    }

    #region Pointer Events

    public void OnBeginDrag(PointerEventData eventData) {
        SetGrabbed(true, true);
    }

    public void OnDrag(PointerEventData eventData) {
        MoveWithMouse(eventData);
    }

    public void OnEndDrag(PointerEventData eventData) {
        SetGrabbed(false, true);
    }

    public void OnPointerClick(PointerEventData eventData) {
        //SetSelected(!m_Selected, false);
    }
    #endregion // Pointer Events
}
