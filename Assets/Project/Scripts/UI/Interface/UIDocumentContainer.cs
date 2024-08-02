

using AstroLab;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIDocumentContainer : MonoBehaviour {
    [SerializeField] private Graphic Background;

    public void Start() {
        GameMgr.Events.Register(GameEvents.DraggableGrabbed, OnDocumentGrabbed);
        GameMgr.Events.Register(GameEvents.DraggableDropped, OnDocumentDropped);
        Background.raycastTarget = false;
    }

    private void OnDocumentGrabbed() {
        Background.raycastTarget = true;

    }

    private void OnDocumentDropped() {
        Background.raycastTarget = false;
    }
}