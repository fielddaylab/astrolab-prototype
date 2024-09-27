

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
        GameMgr.Events.Register<DraggableFlags>(GameEvents.DraggableGrabbed, OnDraggableGrabbed);
        GameMgr.Events.Register<DraggableFlags>(GameEvents.DraggableDropped, OnDraggableDropped);
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
                postcard.transform.localScale = new Vector3(DocModule.MainScreenScale, DocModule.MainScreenScale, 1);
            } else if (SendTo == SendTo.Module) {
                DocModule.Open();
                postcard.transform.SetParent(DocModule.transform, true);
                postcard.transform.localPosition = Vector3.zero;
                postcard.transform.localScale = new Vector3(DocModule.ModuleScale, DocModule.ModuleScale, 1);
            }
            
            postcard.gameObject.SetActive(true);

        }
    }

    public void OnDraggableGrabbed(DraggableFlags flags) {
        if (flags.HasFlag(DraggableFlags.Postcard)) {
            gameObject.SetActive(true);
            this.transform.SetAsLastSibling();
        }
    }

    public void OnDraggableDropped(DraggableFlags flags) {
        if (flags.HasFlag(DraggableFlags.Postcard)) {
            gameObject.SetActive(false);
        }
    }
}