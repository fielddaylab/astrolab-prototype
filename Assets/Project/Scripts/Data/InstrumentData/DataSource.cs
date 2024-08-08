

using AstroLab;
using BeauPools;
using BeauRoutine;
using BeauUtil;
using BeauUtil.Variants;
using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public enum DataType {
    Name,
    Coordinates,
    Color,
    Magnitude,
    Spectrum
}

[Serializable]
public struct DataPayload {
    public string Name;
    public Coordinates Coordinates;
    public Color Color;
    public float Magnitude;
    public float Spectrum;
}

[Serializable]
public struct Coordinates {
    public Vector3 RightAscension;
    public Vector3 Declination;

    public override string ToString() {
        using(PooledStringBuilder psb = PooledStringBuilder.Create()) {
            psb.Builder.Append(RightAscension.x).Append("h ");
            psb.Builder.Append(RightAscension.y).Append("m ");
            psb.Builder.Append(RightAscension.z).Append("s\n");

            psb.Builder.Append(RightAscension.x).Append("d ");
            psb.Builder.Append(RightAscension.y).Append("' ");
            psb.Builder.Append(RightAscension.z).Append("\"");
            return psb.Builder.Flush();
        }
    }
}

public class DataSource : AbstractDraggable {

    [SerializeField] private DataPayload Payload;
    [SerializeField] private DataDragVisual DragVisual;

    private void Start() {
        InstantiateDraggingVisual();
    }

    public DataPayload GetPayload() {
        return Payload;
    }

    private void InstantiateDraggingVisual() {
        DragVisual.transform.SetParent(transform);
        DragVisual.CanvasGroup.alpha = 0.5f;
        DragVisual.gameObject.SetActive(false);
    }

    protected override void SetGrabbed(bool grabbed, bool dragging) {
        if (m_Grabbed == grabbed) return;
        m_Grabbed = grabbed;

        if (grabbed) {
            transform.SetAsLastSibling();
            GameMgr.Events.Dispatch(GameEvents.DraggableGrabbed, DraggableFlags);
        } else {
            GameMgr.Events.Dispatch(GameEvents.DraggableDropped, DraggableFlags);
        }

        DragVisual.gameObject.SetActive(grabbed);

        DragVisual.transform.localScale = grabbed ? new Vector3(GrabScaleFactor, GrabScaleFactor, 1) : new Vector3(1f / GrabScaleFactor, 1f / GrabScaleFactor, 1);

        DragVisual.CanvasGroup.blocksRaycasts = dragging && !grabbed;
    }

    protected override void MoveWithMouse(PointerEventData eventData = null) {
        if (eventData != null) {
            DragVisual.transform.position = eventData.pointerCurrentRaycast.worldPosition;
        }
    }
}