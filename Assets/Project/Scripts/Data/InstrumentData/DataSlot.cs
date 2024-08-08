using AstroLab;
using BeauUtil.Debugger;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DataSlot : MonoBehaviour, IDropHandler {
    [SerializeField] private DraggableFlags SlotType;
    private DataPayload FilledData;
    public bool SlotFilled = false;


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
        if (eventData.pointerDrag.TryGetComponent<DataSource>(out DataSource dragged)) {
            if ((SlotType & dragged.DraggableFlags) != 0) {
                FilledData = dragged.GetPayload();
                SlotFilled = true;
                DisplayFilledData();
            }
        }
    }

    public void DisplayFilledData() {
        if (!SlotFilled) return;
        switch (SlotType) {
            case DraggableFlags.Name:
                Text.text = FilledData.Name;
                break;
            case DraggableFlags.Coords:
                Text.text = FilledData.Coordinates.ToString();
                break;
            case DraggableFlags.Color:
                Graphic.color = FilledData.Color;
                break;
            case DraggableFlags.Magnitude:
                Text.text = FilledData.Magnitude.ToString();
                break;
            case DraggableFlags.Spectrum:
                Text.text = FilledData.Spectrum.ToString();
                break;
        }
    }

    private void HandleDraggableGrabbed(DraggableFlags drag) {
        // all will match "Data" flag, need one other to be a match
        if ((drag & SlotType) != 0 && DropHighlight != null) {
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
