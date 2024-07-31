

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
        GameMgr.Events.Register(GameEvents.DocumentGrabbed, OnDocumentGrabbed);
        GameMgr.Events.Register(GameEvents.DocumentDropped, OnDocumentDropped);
        gameObject.SetActive(false);
    }

    public void OnDrop(PointerEventData eventData) {
        // send event to push a postcard to the main screen zone 
        if (eventData.pointerDrag.TryGetComponent<Draggable>(out Draggable postcard)) {
            Log.Msg("Dropped postcard {0}", postcard.transform.name);
            if (SendTo == SendTo.MainScreen) {
                postcard.transform.SetParent(MainScreen.transform, false);
                DocModule.Close();
                postcard.transform.localPosition = Vector3.zero;
                postcard.transform.localScale = new Vector3(DocModule.MainScreenScale, DocModule.MainScreenScale);
            } else if (SendTo == SendTo.Module) {
                DocModule.Open();
                postcard.transform.SetParent(DocModule.transform, false);
                postcard.transform.localPosition = Vector3.zero;
                postcard.transform.localScale = new Vector3(DocModule.ModuleScale, DocModule.ModuleScale);
            }

        }
    }

    public void OnDocumentGrabbed() {
        gameObject.SetActive(true);
    }

    public void OnDocumentDropped() {
        gameObject.SetActive(false);
    }
}