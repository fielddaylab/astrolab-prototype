using AstroLab;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum DataType {
    Name,
    Coordinates,
    Color,
    Luminosity,
    Spectrum
}

public class DataSlot : MonoBehaviour, IDropHandler {
    [SerializeField] private DataType SlotType;
    public bool SlotFilled = false;


    [SerializeField] private Graphic DropHighlight;
    [SerializeField] private Graphic Graphic;
    [SerializeField] private TMP_Text Text;


    private void Start() {
        GameMgr.Events.Register<DraggableType>(GameEvents.DraggableGrabbed, HandleDraggableGrabbed);
        GameMgr.Events.Register<DraggableType>(GameEvents.DraggableDropped, HandleDraggableDropped);
    }

    public void OnDrop(PointerEventData eventData) {
        if (eventData.pointerDrag.TryGetComponent<Postcard>(out Postcard dragged)) {

        }
    }

    private void HandleDraggableGrabbed(DraggableType type) {
        if (type == DraggableType.Data && DropHighlight != null) {
            DropHighlight.gameObject.SetActive(true);
        }
    }
    private void HandleDraggableDropped(DraggableType type) {
        if (type == DraggableType.Data && DropHighlight != null) {
            DropHighlight.gameObject.SetActive(false);
        }
    }
}
