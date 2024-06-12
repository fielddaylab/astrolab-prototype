using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class VirtualScreen : GraphicRaycaster
{
    public Transform screenTransform; // the transform of the screen with the render texture

    public Camera screenCamera; // Reference to the camera responsible for rendering the virtual screen's rendertexture

    public GraphicRaycaster screenCaster; // Reference to the GraphicRaycaster of the canvas displayed on the virtual screen

    // Called by Unity when a Raycaster should raycast because it extends BaseRaycaster.
    public override void Raycast(PointerEventData eventData, List<RaycastResult> resultAppendList)
    {
        var copyEventData = eventData; 
        Ray ray = eventCamera.ScreenPointToRay(copyEventData.position); // Mouse
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            Debug.Log("[VirtualScreen] raycast hit");

            if (hit.collider.transform == screenTransform)
            {
                // Figure out where the pointer would be in the second camera based on texture position or RenderTexture.
                Vector3 virtualPos = new Vector3(hit.textureCoord.x, hit.textureCoord.y);
                virtualPos.x *= screenCamera.targetTexture.width;
                virtualPos.y *= screenCamera.targetTexture.height;

                copyEventData.position = virtualPos;

                screenCaster.Raycast(copyEventData, resultAppendList);

                Debug.Log("[VirtualScreen] redirected to " + copyEventData.position);
            }
            else
            {
                Debug.Log("[VirtualScreen] hit but not screen transform");
            }
        }
        else
        {
            Debug.Log("[VirtualScreen] default cast to " + copyEventData.position);
            base.Raycast(copyEventData, resultAppendList);
        }
    }
}