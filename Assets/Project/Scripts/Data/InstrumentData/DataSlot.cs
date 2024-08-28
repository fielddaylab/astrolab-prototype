using AstroLab;
using BeauUtil.Debugger;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace AstroLab {

    public class DataSlot : MonoBehaviour, IDropHandler, IPointerClickHandler {
        private static DataPayload NullData = new DataPayload() {
            Color = new Color(1f, 1f, 1f, 0.2f)
        };

        public DraggableFlags SlotType;
        public DataPayload FilledData;
        public bool SlotFilled = false;
        public bool SlotLocked = false;


        [SerializeField] private Graphic DropHighlight;
        [SerializeField] private Graphic Graphic;
        [SerializeField] private TMP_Text Text;


        private void Start() {
            GameMgr.Events.Register<DraggableFlags>(GameEvents.DraggableGrabbed, HandleDraggableGrabbed);
            GameMgr.Events.Register<DraggableFlags>(GameEvents.DraggableDropped, HandleDraggableDropped);
            if (DropHighlight != null) {
                DropHighlight.raycastTarget = false;
            }
        }

        public void OnDrop(PointerEventData eventData) {
            Log.Msg("Dropped item! {0}", eventData.pointerDrag.name);
            if (SlotLocked) return;
            if (eventData.pointerDrag.TryGetComponent<DataSource>(out DataSource dragged)) {
                if ((SlotType & dragged.DraggableFlags) != 0) {
                    FilledData = dragged.GetPayload();
                    SlotFilled = true;
                    DisplayFilledData();
                }
            }
        }

        public void OnPointerClick(PointerEventData eventData) {
            if (!SlotFilled || SlotLocked) return;
            FilledData = NullData;
            DisplayFilledData();
            SlotFilled = false;
        }

        public void SetData(DataPayload payload, bool lockData = false) {
            FilledData = payload;
            SlotFilled = true;
            SlotLocked = lockData;
            DisplayFilledData();
        }

        public void DisplayFilledData() {
            if (!SlotFilled) return;
            switch (SlotType) {
                case DraggableFlags.Name:
                    Text.SetText(FilledData.Name);
                    Log.Msg("Name of star set to {0}", FilledData.Name);
                    break;
                case DraggableFlags.Coords:
                    Text.SetText(FilledData.Coordinates.ToString());
                    Log.Msg("Coords set to {0}", FilledData.Coordinates.ToString());
                    break;
                case DraggableFlags.Color:
                    Graphic.color = FilledData.Color;
                    Log.Msg("Color set to {0}", FilledData.Color.ToString());
                    break;
                case DraggableFlags.Magnitude:
                    Text.SetText(FilledData.Magnitude.ToString());
                    Log.Msg("Mag set to {0}", FilledData.Magnitude.ToString());
                    break;
                case DraggableFlags.Spectrum:
                    //Text.SetText(FilledData.Spectrum.ToString());
                    break;
            }
        }

        private void HandleDraggableGrabbed(DraggableFlags drag) {
            // all will match "Data" flag, need one other to be a match
            if ((drag & SlotType) != 0 && DropHighlight != null && !SlotFilled) {
                DropHighlight.gameObject.SetActive(true);
            }
        }
        private void HandleDraggableDropped(DraggableFlags drag) {
            // all will match "Data" flag, need one other to be a match
            if ((drag & SlotType) != 0 && DropHighlight != null) {
                DropHighlight.gameObject.SetActive(false);
            }
        }
    }
}
