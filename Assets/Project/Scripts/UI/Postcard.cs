using AstroLab;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Postcard : AbstractDraggable {

    [Header("Drop Shadow")]
    [SerializeField] private Graphic m_Shadow;
    [SerializeField] private float m_ShadowScaleFactor;
    [SerializeField] private float m_ShadowOffset;


    private void Update() {
        if (m_Grabbed) {
            //TODO: 
            //MoveWithMouse();
        }
    }

    protected override void SetGrabbed(bool grabbed, bool dragging) {
        base.SetGrabbed(grabbed, dragging);

        if (m_Shadow != null) {
            m_Shadow.transform.localScale *= grabbed ? m_ShadowScaleFactor : 1f / m_ShadowScaleFactor;
            m_Shadow.transform.localPosition += grabbed ? new Vector3(m_ShadowOffset, -m_ShadowOffset, 0f) : new Vector3(-m_ShadowOffset, m_ShadowOffset, 0f);
        }       
    }

}
