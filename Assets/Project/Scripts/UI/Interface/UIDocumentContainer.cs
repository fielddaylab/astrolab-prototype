

using AstroLab;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIDocumentContainer : MonoBehaviour {
    [SerializeField] private Graphic Background;

    public void Start() {
        GameMgr.Events.Register(GameEvents.DocumentGrabbed, OnDocumentGrabbed);
        GameMgr.Events.Register(GameEvents.DocumentDropped, OnDocumentDropped);
        Background.raycastTarget = false;
    }

    private void OnDocumentGrabbed() {
        Background.raycastTarget = true;

    }

    private void OnDocumentDropped() {
        Background.raycastTarget = false;
    }
}