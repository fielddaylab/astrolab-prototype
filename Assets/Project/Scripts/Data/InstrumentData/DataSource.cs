using AstroLab;
using BeauPools;
using BeauUtil;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace AstroLab {
    [Serializable]
    public struct DataPayload {
        public string Name;
        public EqCoordinates Coordinates;
        public Color Color;
        public float Magnitude;
        public float Spectrum;
        public DataPayload (string name) {
            Name = name;
            Coordinates = default;
            Color = default;
            Magnitude = default;
            Spectrum = default;
        }
        public DataPayload (EqCoordinates coords) {
            Name = default;
            Coordinates = coords; 
            Color = default;
            Magnitude = default;
            Spectrum = default;
        }
    }

    [Serializable]
    public struct EqCoordinates {
        public Vector3 RightAscension;
        public Vector3 Declination;

        public EqCoordinates(Vector3 both) {
            RightAscension = both;
            Declination = both;
        }
        public EqCoordinates(Vector3 ra, Vector3 d) {
            RightAscension = ra;
            Declination = d;
        }
        public override string ToString() {
            using PooledStringBuilder psb = PooledStringBuilder.Create();
            psb.Builder.Append(RightAscension.x).Append("h\t");
            psb.Builder.Append(RightAscension.y).Append("m\t");
            psb.Builder.Append(RightAscension.z.ToString("F1")).Append("s\n");

            psb.Builder.Append(RightAscension.x).Append("\u00B0\t");
            psb.Builder.Append(RightAscension.y).Append("'\t");
            psb.Builder.Append(RightAscension.z.ToString("F1")).Append("\"");
            return psb.Builder.Flush();
        }
        public override bool Equals(object obj) {
            if (obj is not EqCoordinates) return false;
            return RightAscension.Equals(((EqCoordinates)obj).RightAscension)
                && Declination.Equals(((EqCoordinates)obj).Declination);
        }
    }

    public class DataSource : AbstractDraggable {

        [SerializeField] private DataPayload Payload;
        [SerializeField] private DataDragVisual DragVisual;
        [SerializeField] private Graphic DataGraphic;
        [SerializeField] private TMP_Text DataText;

        private void Start() {
            InstantiateDraggingVisual();
        }

        public DataPayload GetPayload() {
            return Payload;
        }

        public void SetPayload(DataPayload payload) {
            // TODO: set, check type (?) and update text

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
}