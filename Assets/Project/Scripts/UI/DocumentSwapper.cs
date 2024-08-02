

using AstroLab;
using BeauUtil.Debugger;
using UnityEngine;
using UnityEngine.EventSystems;

public enum SendTo {
    Module,
    MainScreen
}
public class DocumentSwapper : MonoBehaviour, IDropHandler {

    [SerializeField] private UIDocumentsModule DocModule;
    [SerializeField] private UIDocumentContainer MainScreen;

    [SerializeField] private SendTo SendTo;

    public void Start() {
        GameMgr.Events.Register<DraggableType>(GameEvents.DraggableGrabbed, OnDraggableGrabbed);
        GameMgr.Events.Register<DraggableType>(GameEvents.DraggableDropped, OnDraggableDropped);
        gameObject.SetActive(false);
    }

    public void OnDrop(PointerEventData eventData) {
        // send event to push a postcard to the main screen zone 
        if (eventData.pointerDrag.TryGetComponent<Postcard>(out Postcard postcard)) {
            Log.Msg("Dropped postcard {0}", postcard.transform.name);
            postcard.gameObject.SetActive(false);
            if (SendTo == SendTo.MainScreen) {
                postcard.transform.SetParent(MainScreen.transform, true);
                DocModule.Close();
                postcard.transform.localPosition = Vector3.zero;
                postcard.transform.localScale = new Vector3(DocModule.MainScreenScale, DocModule.MainScreenScale);
            } else if (SendTo == SendTo.Module) {
                DocModule.Open();
                postcard.transform.SetParent(DocModule.transform, true);
                postcard.transform.localPosition = Vector3.zero;
                postcard.transform.localScale = new Vector3(DocModule.ModuleScale, DocModule.ModuleScale);
            }
            postcard.gameObject.SetActive(true);

        }
    }

    public void OnDraggableGrabbed(DraggableType type) {
        if (type == DraggableType.Postcard) {
            gameObject.SetActive(true);
        }
    }

    public void OnDraggableDropped(DraggableType type) {
        if (type == DraggableType.Postcard) {
            gameObject.SetActive(false);
        }
    }
}