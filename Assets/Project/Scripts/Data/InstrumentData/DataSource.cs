using AstroLab;
using BeauPools;
using BeauUtil;
using BeauUtil.Debugger;
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
        public DataPayload (Color color) {
            Name = default;
            Coordinates = default;
            Color = color;
            Magnitude = default;
            Spectrum = default;
        }
        public DataPayload(float mag) {
            Name = default;
            Coordinates = default;
            Color = default;
            Magnitude = mag;
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
            psb.Builder.Append(RightAscension.x).Append("h ");
            psb.Builder.Append(RightAscension.y).Append("m ");
            psb.Builder.Append(RightAscension.z.ToString("F1")).Append("s\n");

            psb.Builder.Append(Declination.x).Append("\u00B0 ");
            psb.Builder.Append(Declination.y).Append("' ");
            psb.Builder.Append(Declination.z.ToString("F1")).Append("\"");
            return psb.Builder.Flush();
        }
        public override bool Equals(object obj) {
            if (obj is not EqCoordinates) return false;
            return RightAscension.Equals(((EqCoordinates)obj).RightAscension)
                && Declination.Equals(((EqCoordinates)obj).Declination);
        }
        public bool Zero() {
            return RightAscension.Equals(Vector3.zero)
                && Declination.Equals(Vector3.zero);
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
            Payload = payload;
            DisplayPayload();
        }

        private void DisplayPayload() {
            if (Payload.Name != null) {
                DataText.SetText(Payload.Name);
            } else if (!Payload.Magnitude.Equals(default)) {
                DataText.SetText(Payload.Magnitude.ToString());
            } else if (!Payload.Color.Equals(default)) {
                DataGraphic.color = Payload.Color;
            } else if (!Payload.Coordinates.Zero()) {
                DataText.SetText(Payload.Coordinates.ToString());
            } else if (!Payload.Spectrum.Equals(default)) {
                Log.Warn("[DataSource] Spectrum graphics unimplemented!");
            } else {
                Log.Warn("[DataSource] All default payload! Unimplemented?");
                return;
            }
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