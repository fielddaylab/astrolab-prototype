

using BeauUtil.Debugger;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace AstroLab {
    public class DataSlotCoords : DataSlot {

        [Header("Coords Slot")]
        [SerializeField] private UIInstrumentsModule Instruments;
        [SerializeField] private Graphic m_Graphic;

        public override void OnDrop(PointerEventData eventData) {
            Log.Msg("Dropped item! {0}", eventData.pointerDrag.name);
            if (SlotLocked) return;
            if (eventData.pointerDrag.TryGetComponent<DataSource>(out DataSource dragged)) {
                if ((SlotType & dragged.DraggableFlags) != 0) {
                    FilledData = dragged.GetPayload();
                    SlotFilled = true;
                    GameMgr.Events.Dispatch(GameEvents.DataSlotFilled, SlotType);
                    Instruments.PopulateInputCoords(this.FilledData.Coordinates);
                    SlotFilled = false;
                }
            }
        }

        protected override void HandleDraggableGrabbed(DraggableFlags drag) {
            base.HandleDraggableGrabbed(drag);
            m_Graphic.raycastTarget = true;
        }

        protected override void HandleDraggableDropped(DraggableFlags drag) {
            base.HandleDraggableDropped(drag);
            m_Graphic.raycastTarget = false;
        }

        public override void OnPointerClick(PointerEventData eventData) {
            return;
        }
    }
}